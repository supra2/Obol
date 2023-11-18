using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
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
        ///  Manage the passing of time
        /// </summary>
        [SerializeField]
        protected TimeManager _timeManager;
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
        /// <summary>
        /// last Direction walked by the player
        /// </summary>
        protected Direction _lastDirectionWalked = Direction.None;
        /// <summary>
        /// last Direction walked by the player
        /// </summary>
        protected TileDisplayer _currentTile;
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

        #region Methods
        //--------------------------------------------------------

        /// <summary>
        ///  Initialise Exploration Manager
        /// </summary>
        /// <param name="levelToExplore"></param>
        public void Init(Level levelToExplore)  
        {

            _tileManager.Init();

            _lastDirectionWalked = Direction.None;

            _timeManager.SetTime(0);

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

            levelToExplore.Init( _timeManager );

            levelToExplore.PlaceStartingTile( PlayerPosition , levelToExplore.StartingTile,_gridview );

            _currentLevel.PlayerMove( Vector2.zero , Direction.None,
                _gridview , _explorationEvents );

            PlayerPosition = Vector2.zero;

        }

        //--------------------------------------------------------

        public void Load( Level levelToExplore , 
                          Vector2 saved_PlayerPosition )
        {

            _lastDirectionWalked = Direction.None;

            _currentLevel = levelToExplore;

            levelToExplore.Init(_timeManager);
          
            _currentLevel.PlayerMove(Vector2.zero, Direction.None,
                _gridview, _explorationEvents);

            PlayerPosition = saved_PlayerPosition;

        }
        
        //--------------------------------------------------------

        /// <summary>
        ///  Move Player 
        /// </summary>
        /// <param name="direction"> direction player move</param>
        public void MovePlayer(int direction)
        {
            _lastDirectionWalked = 
                (Tile.Direction)( 1 << direction );

            Vector2 displacement = Vector3.zero;

            switch(_lastDirectionWalked)
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

            GameManager.Instance.PartyManager.Party.ChangeFoodLevel();
            PlayerPosition += displacement;

            if (_currentTile != null)
                _currentTile.Event.Leave(_lastDirectionWalked);

            _currentLevel.PlayerMove(PlayerPosition,
                                    _lastDirectionWalked,
                                    _gridview,
                                    _explorationEvents);
        }

        //--------------------------------------------------------

        public void ReloadLevel()
        {
              string content =  File.ReadAllText(
              Application.persistentDataPath + "\\Save\\CurrentLevel.json");
              _currentLevel =  JsonUtility.FromJson<Level>( content );
              
        }

        //--------------------------------------------------------

        public void Save(   
                          Vector2 saved_PlayerPosition)
        {
           string jsoncontent = JsonUtility.ToJson(CurrentLevel);
           File.WriteAllText(Application.persistentDataPath+
               "\\Save\\CurrentLevel.json",jsoncontent);
          

        }

        //--------------------------------------------------------
        #endregion

    }
}
