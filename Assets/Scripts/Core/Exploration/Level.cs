using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
        #endregion
        #region Hidden
        #endregion
        #endregion

        #region Getter
        public List<ExplorationEvent> EventList=> _eventList;

        public ExplorationEvent StartingEvent => _startingEvent;
        public Tile StartingTile => _startingTile;
        #endregion


        #region Methods

        public void  Init( )
        {
            _startingTile.Init( _startingEvent );
        }

        #endregion

    }
}
