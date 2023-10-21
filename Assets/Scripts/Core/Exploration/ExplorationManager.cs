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

    public class ExplorationManager : Singleton<ExplorationManager>
    {

        #region Members
        #region Visible
        /// <summary>
        /// Grid view displaying tiles 
        /// </summary>
        [SerializeField]
        protected GridView _gridview;
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
        /// <summary>
        /// Current Player Position
        /// </summary>
        protected Vector2 _playerPosition;
        #endregion
        #endregion

        #region Getters
        //--------------------------------------------------------

        public GridView GridView => _gridview;
        //--------------------------------------------------------

        public Vector2 PlayerPosition
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

        /// <summary>
        /// Current Level
        /// </summary>
        public Level CurrentLevel
        {
            get => _currentLevel;
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
            if ( _currentLevel.EventList != null )
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
                            ScriptableObject.Instantiate(Event) 
                            as ExplorationEvent );
                    }
                }
                _explorationEvents.Shuffle();
            }
            levelToExplore.Init();
            Place( PlayerPosition , levelToExplore.StartingTile );
            _currentLevel.Explore( Vector2.zero , _gridview , _explorationEvents );
            PlayerPosition = Vector2.zero;
        }

        //--------------------------------------------------------

        public void Move(Vector2 Position, Tile tile)
        {
            TileDisplayer instance = _gridview.CreateTile(tile);
            _gridview.PlaceTile(instance, Vector2.zero );
        }

        //--------------------------------------------------------

        protected void Place( Vector2 position , Tile tile)
        {
           TileDisplayer instance = _gridview.CreateTile(tile);
            _gridview.PlaceTile(instance, position);

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
            _currentLevel.Explore(PlayerPosition,_gridview,_explorationEvents);
        }

        //--------------------------------------------------------
        #endregion

    }
}
