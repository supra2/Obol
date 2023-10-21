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
        protected List<TileDisplayer> _tileDisplayer;

        [SerializeField]
        protected List<ExplorationEvent> _explorationEvent;
        #endregion
        #endregion
        public override void Init()
        {
            foreach(TileDisplayer tiledisplayer in _tileDisplayer )
            {
                ExplorationManager.Instance.GridView.PlaceTile(GameObject.Instantiate(tiledisplayer),
                    tiledisplayer.Position);
            }
        }

        public override void Explore(Vector2 position, GridView gridview, 
            Deck<ExplorationEvent> explorationDeck )
        {



        }
    }
}
