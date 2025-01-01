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
        protected List<ExplorationEvent> _eventList;
        /// <summary>
        /// Tile displayed when entering the level
        /// </summary>
        [SerializeField]
        protected Tile _startingTile;
        /// <summary>
        /// Event Played on first 
        /// </summary>
        [SerializeField]
        protected ExplorationEvent _startingEvent;
        /// <summary>
        /// List of adversaire associated to the level
        /// </summary>
        [SerializeReference]
        protected List<Adversaire> _adversairesList;
        [SerializeField]
        protected UndirectedGenericGraph<TileDisplayer> _tileDisplayer;
        #endregion
        #region Hidden
        /// <summary>
        /// Deck of adversaire associated to the level
        /// </summary>
        Deck<Adversaire> _adversaireDeck;
        [SerializeReference]
        /// <summary>
        /// CurrentTile
        /// </summary>
        protected TileDisplayer  _currentTileDisplayer;
        /// <summary>
        ///time Manager reference
        /// </summary>
        protected TimeManager _timeManager;
        #endregion
        #endregion

        #region Getter

        public Deck<Adversaire> EncounterDeck => _adversaireDeck;

        public List<ExplorationEvent> EventList=> _eventList;

        public ExplorationEvent StartingEvent => _startingEvent;

        public Tile StartingTile => _startingTile;

        #endregion

        #region Methods
        //--------------------------------------------------------

        public virtual void  Init(TimeManager timeManager )
        {

            _timeManager = timeManager;
            _startingTile.Init(_startingEvent);
            InitEncounterDeck();

        }

        //--------------------------------------------------------

        public void PlaceStartingTile(Vector2 position, Tile tile,
                                        GridView gridview)
        {
            TileDisplayer instance = gridview.CreateTile(tile);
            instance.Position = position;
            _tileDisplayer.AddVertex(new Vertex<TileDisplayer>(instance) );
            gridview.PlaceTileVisible(instance);
        }

        //--------------------------------------------------------

        protected void InitEncounterDeck()
        {
            _adversaireDeck = new Deck<Adversaire>();

            foreach (Adversaire adversaire in _adversairesList)
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
                for (int i = 0; i < rarity_Multiplier; i++)
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
        /// Player Move
        /// </summary>
        /// <param name="position"> position </param>
        /// <param name="movementDirection"> Move Direction</param>
        /// <param name="gridview"> Grid view </param>
        /// <param name="explorationDeck">Exploration Deck</param>
        public virtual void  PlayerMove(Vector2 position, 
                                        Direction movementDirection,
                                        GridView gridview, 
                                        Deck<ExplorationEvent> explorationDeck)
        {

            if( _currentTileDisplayer !=null)
                _currentTileDisplayer.Event?.Leave(movementDirection);

            _currentTileDisplayer = gridview.GetTileDisplayer(position);
            if (_currentTileDisplayer.Visibility == 
                TileDisplayer.VisibilityMode.Hidden)
            {
                _currentTileDisplayer.Visibility = 
                    TileDisplayer.VisibilityMode.ShownLit;
                _currentTileDisplayer.Event.Reveal();
            }
            _currentTileDisplayer.Event?.Enter(movementDirection);

            Explore(_currentTileDisplayer, gridview, explorationDeck);
        }

        //--------------------------------------------------------

        /// <summary>
        /// Exploration
        /// </summary>
        /// <param name="position"></param>
        public virtual void Explore( TileDisplayer tiledisplayer , 
            GridView gridview , 
            Deck<ExplorationEvent> explorationDeck  )
        {
            
            foreach (Tile.Direction direction in 
                System.Enum.GetValues(typeof(Tile.Direction)))
            {
                if ( tiledisplayer.Tile.DirectionAvailable( direction )  )
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

                    Vector2 newposition = _currentTileDisplayer.
                        Position + deplacement;

                    if(!gridview.Tiles.Find((X) =>
                        newposition == X.Position) )
                    { 
                        ExplorationEvent events = explorationDeck.Draw();
                        PlaceAdjacentTilesRecursively(newposition, gridview, new List<TileDisplayer> { tiledisplayer }, GameManager.Instance.PartyManager.VisionRange);
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
        protected void PlaceAdjacentTilesRecursively(Vector2 newposition
            , GridView gridview, List<TileDisplayer> visited, int lightDistance)
        {
            List<Vector2> list = new List<Vector2>() {
                new Vector2(0,-1) , new Vector2(0, 1) ,
                        new Vector2(-1, 0) , new Vector2(1,0) };
            List<Tuple<Direction, bool>> tuples = new List<Tuple<Direction, bool>>();
            List<TileDisplayer> neighbours = new List<TileDisplayer>();

            for (int i = 0; i < 9; i++)
            {
                TileDisplayer tileDisplayer = gridview.Tiles.Find((X) =>
                (newposition + list[i]) == X.Position);
                neighbours.Add(tileDisplayer);
                if (tileDisplayer != null)
                {
                    if (visited.Contains(tileDisplayer))
                        return;

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
                }
            }
            visited.AddRange(neighbours);
            PlaceRandomTileAtPosition(gridview, tuples, newposition, neighbours);
            lightDistance--;
            if(lightDistance > 0)
            {
                    for (int i = 0; i < 4; i++)
                    {
                        var pos = newposition;
                        switch ((Tile.Direction)(1 << i))
                        {
                            case Tile.Direction.Bottom:
                                pos += Vector2.down;
                                break;
                            case Tile.Direction.Up:
                                pos += Vector2.up;
                                break;
                            case Tile.Direction.Left:
                                pos += Vector2.left;
                                break;
                            case Tile.Direction.Right:
                                pos += Vector2.right;
                                break;
                        }
                        PlaceAdjacentTilesRecursively(pos, gridview, visited, lightDistance);
                    }
                }
        }

        //--------------------------------------------------------

        /// <summary>
        /// Place random tiles at position respecting a set of contraints
        /// defined by a tuble of direction ant boolean indicating a connection or a block constraints
        /// </summary>
        /// <param name="gridview"></param>
        /// <param name="constraints">Direction _ bool tuple </param>
        private void PlaceRandomTileAtPosition(GridView gridview,
            List<Tuple<Direction, bool>> constraints,Vector2 posi,List<TileDisplayer> displayer)
        {
            List<Tile> tiles = gridview.TileManager.GetListOfTiles(constraints);
            int randomId = SeedManager.NextInt(0, tiles.Count);
            Tile PickedTile = tiles[randomId];
            TileDisplayer tiledisplayer = gridview.CreateTile(PickedTile);
            tiledisplayer.Position = posi;

            foreach (TileDisplayer td in displayer)
            {
                Vertex<TileDisplayer> vertex = new Vertex<TileDisplayer>(tiledisplayer);
                _tileDisplayer.AddVertex(vertex);
                _tileDisplayer.AddEdge(vertex, _tileDisplayer.Vertices.Find((X) => X.Value == td));
            }
           
            if (TileVisible(tiledisplayer))
            {
                gridview.PlaceTileVisible(tiledisplayer);
            }
            else
            {
                gridview.PlaceTileHidden(tiledisplayer);
            }

        }

        //--------------------------------------------------------

        /// <summary>
        ///Tile visible
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        public bool TileVisible( TileDisplayer tile )
        {
            return  (tile.Event!=null  && tile.Event.Lit) || 
                ( _timeManager.IsDay() && 
            ( _currentTileDisplayer.Position - tile.Position).magnitude 
                <= GameManager.Instance.PartyManager.VisionRange );
        }

        //--------------------------------------------------------
        #endregion

    }
}
