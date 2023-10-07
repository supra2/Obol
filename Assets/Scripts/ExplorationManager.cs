using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Core.Exploration.Tile;

namespace Core.Exploration
{

    [Serializable]
    public class UnityEventPosition: UnityEvent<Vector2>
    {

    }

    public class ExplorationManager : MonoBehaviour
    {

        #region Members
        #region Visible
        /// <summary>
        /// Grid view displaying tiles 
        /// </summary>
        [SerializeField]
        protected GridView _gridview;
        [SerializeField]
        protected TileDisplayer _tileDisplayer;
        /// <summary>
        ///  tileManager managing the tiles pool to instantiate
        /// </summary>
        [SerializeField]
        protected TileManager _tileManager;
        /// <summary>
        /// Pion
        /// </summary>
        [SerializeField]
        protected PartyPion _pion;
        #endregion
        #region Hidden
        protected Level _currentLevel;
        protected Deck<ExplorationEvent> _explorationEvents;
        protected Vector2 _playerPosition;
        #endregion
        #endregion

        #region Getters
        //--------------------------------------------------------

        protected Vector2 PlayerPosition
        {
            get => _playerPosition;
            set
            {
                _gridview.Place(_pion.transform, value);
                _playerPosition = value;
                OnPlayerMoved?.Invoke( _playerPosition );
            }
        }

        //--------------------------------------------------------
        #endregion

        #region Event
        //--------------------------------------------------------

        public UnityEventPosition OnPlayerMoved;

        //--------------------------------------------------------
        #endregion

        #region Members
        //--------------------------------------------------------

        /// <summary>
        ///  Initialise Exploration Manager
        /// </summary>
        /// <param name="levelToExplore"></param>
        public void Init(Level levelToExplore)  
        {
            _currentLevel = levelToExplore;
            if (_currentLevel.EventList != null)
            {
                _explorationEvents = new Deck<ExplorationEvent>();
                foreach ( ExplorationEvent Event in _currentLevel.EventList )
                {
                    int rarity_Multiplier = 0;
                        switch (Event.EventRarity)
                        {
                            case ExplorationEvent.Rarity.Common:
                                rarity_Multiplier = 5;
                                break;
                            case ExplorationEvent.Rarity.Uncommon:
                                rarity_Multiplier = 3;
                                break;
                            case ExplorationEvent.Rarity.Rare:
                            case ExplorationEvent.Rarity.Unique:
                                rarity_Multiplier = 1;
                                break;
                        }
                        for (int i = 0; i < rarity_Multiplier; i++)
                        {
                            _explorationEvents.AddTop(
                                ScriptableObject.Instantiate(Event) as ExplorationEvent );
                        }
                    }
                    _explorationEvents.Shuffle();
                }
            levelToExplore.Init();
            Place( PlayerPosition , levelToExplore.StartingTile );
            Explore( Vector2.zero  );
            PlayerPosition = Vector2.zero;
        }

        //--------------------------------------------------------

        public void Move(Vector2 Position, Tile tile)
        {
            _gridview.PlaceTile(_tileDisplayer, Vector2.zero);
        }

        //--------------------------------------------------------

        protected void Place( Vector2 position , Tile tile)
        {

            TileDisplayer tiledisplayer =
              GameObject.Instantiate(_tileDisplayer) as TileDisplayer;
            tiledisplayer.Tile = tile;
            tiledisplayer.gameObject.SetActive(true);
            _gridview.PlaceTile(tiledisplayer, position);

        }

        //--------------------------------------------------------

        /// <summary>
        /// Exploration
        /// </summary>
        /// <param name="position"></param>
        public void Explore(Vector2 position)
        {
           TileDisplayer tiledisplayer =  _gridview.GetTileDisplayer(position);
            foreach (Tile.Direction direction in System.Enum.GetValues(typeof(Tile.Direction)))
            {
                if (tiledisplayer.Tile.DirectionAvailable(direction))
                {
                    Vector2 deplacement = Vector2.zero;
                    switch (direction)
                    {
                        case Tile.Direction.Bottom:
                            deplacement = new Vector2( 0 , 1 );
                            break;
                        case Tile.Direction.Left:
                            deplacement = new Vector2(-1, 0);
                            break;
                        case Tile.Direction.Right:
                            deplacement = new Vector2(1, 0);
                            break;
                        case Tile.Direction.Up:
                            deplacement = new Vector2(0, -1);
                            break;
                    }
                    Vector2 newposition = position + deplacement;
                    ExplorationEvent events = _explorationEvents.Draw();
                    PlaceConnectedTiles(newposition);
                }
            }
        }

        //--------------------------------------------------------

        /// <summary>
        ///  Move Player
        /// </summary>
        /// <param name="direction"></param>
        public void MovePlayer(int direction)
        {

            Tile.Direction g = (Tile.Direction)( 1 << direction );

            Vector2 displacement = Vector3.zero;

            switch( g)
            {
                case Tile.Direction.Bottom:
                    displacement = new Vector2(0, 1);
                    break;
                case Tile.Direction.Left:
                    displacement = new Vector2(-1, 0);
                    break;
                case Tile.Direction.Right:
                    displacement = new Vector2(1, 0);
                    break;
                case Tile.Direction.Up:
                    displacement = new Vector2(0, -1);
                    break;
            }

            PartyManager.Instance.Party.ChangeFoodLevel();

            PlayerPosition += displacement;

            Explore(PlayerPosition);
        }

        //--------------------------------------------------------

        /// <summary>
        /// Place connected tiles connectied to new
        /// </summary>
        /// <param name="newposition"></param>
        /// <param name=""></param>
        protected void PlaceConnectedTiles( Vector2 newposition )
        {
            List<Vector2> list = new List<Vector2>() {
                new Vector2(0,-1) , new Vector2(0, 1) ,
                        new Vector2(-1, 0) , new Vector2(1,0) };

            List<Tuple<Direction, bool>> tuples = new List<Tuple<Direction, bool>>();

            for ( int i = 0 ; i < 4 ; i ++ )
            {
               TileDisplayer tileDisplayer = _gridview.Tiles.Find((X) =>
               (newposition + list[i]) == X.Position);
                if (tileDisplayer != null)
                {
                    bool walkable;
                    switch ( (Tile.Direction)(1 << i) )
                    {
                        case Tile.Direction.Bottom :
                             walkable = tileDisplayer.Tile.DirectionAvailable( Tile.Direction.Up );
                            tuples.Add( new Tuple<Direction, bool>( Tile.Direction.Bottom, walkable ) );
                            break;
                        case Tile.Direction.Up :
                            walkable = tileDisplayer.Tile.DirectionAvailable( Tile.Direction.Bottom );
                            tuples.Add( new Tuple<Direction, bool>( Tile.Direction.Up, walkable ) );
                            break;
                        case Tile.Direction.Left :
                            walkable = tileDisplayer.Tile.DirectionAvailable( Tile.Direction.Right );
                            tuples.Add( new Tuple<Direction, bool>( Tile.Direction.Left, walkable ) );
                            break;
                        case Tile.Direction.Right :
                            walkable = tileDisplayer.Tile.DirectionAvailable( Tile.Direction.Left );
                            tuples.Add(new Tuple<Direction, bool>( Tile.Direction.Right, walkable ));
                            break;
                    }

                    List<Tile> tiles = _tileManager.GetListOfTiles(tuples);
                    int randomId = SeedManager.NextInt(0, tiles.Count);
                    Tile PickedTile = tiles[randomId];

                    TileDisplayer tiledisplayer =
                    GameObject.Instantiate(_tileDisplayer) as TileDisplayer;
                    tiledisplayer.Tile = PickedTile;
                    tiledisplayer.gameObject.SetActive(true);

                    _gridview.PlaceTile(tiledisplayer, newposition);
                }
            }


        }

        //--------------------------------------------------------
        #endregion

    }
}
