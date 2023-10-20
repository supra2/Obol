using Core.FightSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Core.Exploration.Tile;

namespace Core.Exploration
{

    [CreateAssetMenu( fileName = " Level",
                      menuName = "Exploration/Level",
                      order = 2)]
    [Serializable]
    public class Level:ScriptableObject
    {

        #region Members
        #region Visible 
        /// <summary>
        /// List of event composing a Level
        /// </summary>
        [SerializeField]
        List<ExplorationEvent> _eventList;
        /// <summary>
        /// Tile displayed when entering the level
        /// </summary>
        [SerializeField]
        Tile _startingTile;
        /// <summary>
        /// Event Played on first 
        /// </summary>
        [SerializeField]
        ExplorationEvent _startingEvent;
        /// <summary>
        /// List of adversaire associated to the level
        /// </summary>
        [SerializeReference]
        List<Adversaire> _adversairesList;
        #endregion
        #region Hidden
        /// <summary>
        /// Deck of adversaire associated to the level
        /// </summary>
        Deck<Adversaire> _adversaireDeck;
        #endregion
        #endregion

        #region Getter

        public Deck<Adversaire> EncounterDeck => _adversaireDeck;

        public List<ExplorationEvent> EventList=> _eventList;

        public ExplorationEvent StartingEvent => _startingEvent;

        public Tile StartingTile => _startingTile;

        #endregion

        #region Methods

        public void  Init( )
        {
            _startingTile.Init( _startingEvent );
            _adversaireDeck = new Deck<Adversaire>();
            
            foreach( Adversaire adversaire in _adversairesList )
            {
                int rarity_Multiplier = 0;
                switch (adversaire.Rarity)
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
                for(int i =0; i< rarity_Multiplier;i++)
                {
                    Adversaire adv = ScriptableObject.Instantiate<Adversaire>(adversaire);
                    adv.Init();
                    _adversaireDeck.AddTop(adv);
                }
            }
            _adversaireDeck.Shuffle();
        }

        #endregion

        #region Public Methods
        //--------------------------------------------------------

        /// <summary>
        /// Exploration
        /// </summary>
        /// <param name="position"></param>
        public virtual void Explore(Vector2 position,GridView gridview , Deck<ExplorationEvent> explorationDeck  )
        {
            TileDisplayer tiledisplayer = gridview.GetTileDisplayer(position);
            foreach (Tile.Direction direction in System.Enum.GetValues(typeof(Tile.Direction)))
            {
                if ( tiledisplayer.Tile.DirectionAvailable( direction ) && )
                {
                    Vector2 deplacement = Vector2.zero;
                    switch (direction)
                    {
                        case Tile.Direction.Bottom:
                            deplacement = new Vector2(0, 1);
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
                   if( gridview.Tiles.Find((X) =>
                    newposition == X.Position))
                    { 
                        ExplorationEvent events = explorationDeck.Draw();
                        PlaceConnectedTiles(newposition, gridview);
                    }
                }
            }
        }


        //--------------------------------------------------------

        /// <summary>
        /// Place connected tiles connectied to new
        /// </summary>
        /// <param name="newposition"></param>
        /// <param name=""></param>
        protected void PlaceConnectedTiles( Vector2 newposition
            ,GridView gridview)
        {

            List<Vector2> list = new List<Vector2>() {
                new Vector2(0,-1) , new Vector2(0, 1) ,
                        new Vector2(-1, 0) , new Vector2(1,0) };

            List<Tuple<Direction, bool>> tuples = new List<Tuple<Direction, bool>>();

            for (int i = 0; i < 4; i++)
            {
                TileDisplayer tileDisplayer = gridview.Tiles.Find((X) =>
                (newposition + list[i]) == X.Position);
                if (tileDisplayer != null)
                {
                    bool walkable;
                    switch ((Tile.Direction)(1 << i))
                    {
                        case Tile.Direction.Bottom:
                            walkable = tileDisplayer.Tile.DirectionAvailable(Tile.Direction.Up);
                            tuples.Add(new Tuple<Direction, bool>(Tile.Direction.Bottom, walkable));
                            break;
                        case Tile.Direction.Up:
                            walkable = tileDisplayer.Tile.DirectionAvailable(Tile.Direction.Bottom);
                            tuples.Add(new Tuple<Direction, bool>(Tile.Direction.Up, walkable));
                            break;
                        case Tile.Direction.Left:
                            walkable = tileDisplayer.Tile.DirectionAvailable(Tile.Direction.Right);
                            tuples.Add(new Tuple<Direction, bool>(Tile.Direction.Left, walkable));
                            break;
                        case Tile.Direction.Right:
                            walkable = tileDisplayer.Tile.DirectionAvailable(Tile.Direction.Left);
                            tuples.Add(new Tuple<Direction, bool>(Tile.Direction.Right, walkable));
                            break;
                    }

                    List<Tile> tiles = gridview.TileManager.GetListOfTiles(tuples);
                    int randomId = SeedManager.NextInt(0, tiles.Count);
                    Tile PickedTile = tiles[randomId];

                    TileDisplayer tiledisplayer = gridview.CreateTile(PickedTile);
                    gridview.PlaceTile(tiledisplayer, newposition);
                }

            }


        }

        //--------------------------------------------------------
        #endregion

    }
}
