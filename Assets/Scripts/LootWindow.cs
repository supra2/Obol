using Core.FightSystem;
using Core.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.ItemSystem
{ 
    /// <summary>
    /// Loot Window
    /// </summary>
    public class LootWindow : MonoBehaviour
    {

        #region Members
        #region Visible
        [SerializeField]
        protected InventoryUIManager _inventoryUIManager;
        #endregion
        #region Hidden
        public static List<Adversaire> _adversaireFought;
        #endregion
        #endregion

        #region Inner Members
        //------------------------------------------------------------------------
        protected void OnEnable()
        {
            _inventoryUIManager.Init( InventoryUIManager.Mode._LootMode);
       
        }

        //------------------------------------------------------------------------


        /// <summary>
        /// Determine the content of the loot from the Combat State 
        /// </summary>
        protected void RollLoot()
        {
            foreach( Adversaire adv in _adversaireFought)
            {
                Dictionary<int, Item> item = adv.LootTable;
                foreach( int proba in item.Keys)
                { 
                }
                SeedManager.NextInt(0, 1);
            }

        }

        //------------------------------------------------------------------------
        #endregion
    }
}