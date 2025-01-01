using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Exploration
{

    [CreateAssetMenu(fileName = " Level",
                   menuName = "Exploration/ScriptedLevel",
                   order = 3)]
    public class ScriptedLevel: Level
    {

        #region Members
        #region Visible 
        [SerializeField]
        protected List<ExplorationEvent> _explorationEvent;
        [SerializeField]
        protected List<Tile> _Tiles;
        [SerializeField]
        protected List<Vector2> _positions;
        #endregion
        #endregion

        public override void Init(TimeManager timeManager)
        {
            int i = 0;
            _timeManager = timeManager;
            _currentTileDisplayer = ExplorationManager.Instance.GridView.CreateTile(_Tiles[0]);
            _startingTile = _currentTileDisplayer.Tile;
            foreach (var tiles in _Tiles)
            {
                TileDisplayer td = ExplorationManager.Instance.GridView.CreateTile(tiles);
                td.Position = _positions[i];
                td.Event = _explorationEvent[i];
                td.Event?.Init();
                if ( i == 0 || i == 1 || TileVisible(td))
                {
                    ExplorationManager.Instance.GridView.PlaceTileVisible(td);
                }
                else
                {
                    ExplorationManager.Instance.GridView.PlaceTileHidden(td);
                }
                if (i > 1)
                {
                    td.Visibility = 
                        TileDisplayer.VisibilityMode.Hidden;
                }
                i++;
            }
            InitEncounterDeck();
        }

        public override void Explore(TileDisplayer tiledisplayer, GridView gridview,
            Deck<ExplorationEvent> explorationDeck)
        {
                


        }

    }
}
