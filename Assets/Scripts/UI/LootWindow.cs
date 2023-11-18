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
        [SerializeField]
        protected LootPanel _lootPanel;
        #endregion
        #region Hidden
        public  List<Adversaire> _adversaireFought;
        #endregion
        #endregion
        public List<Adversaire> AdversaireFought {

           get => _adversaireFought;
            set => _adversaireFought = value;
        }

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
        protected List<Item> RollLoot(List<Adversaire> adversaires )
        {
            List<Item> itemLooted = new List<Item>();
            foreach( Adversaire adv in adversaires)
            {
                Dictionary<int, Item> item = adv.LootTable;
                int totalProbRange = 0;
                foreach( int proba in item.Keys )
                {
                    totalProbRange += proba;
                }
                int roll = SeedManager.NextInt(0, totalProbRange-1);
                int totalDellProbRange = 0 ;
                foreach ( KeyValuePair<int,Item> pair in item )
                {
                    totalDellProbRange += pair.Key;
                    if( totalDellProbRange  > roll )
                    {
                        itemLooted.Add(pair.Value);
                    }
                }
            }
            return itemLooted;
        }

        //------------------------------------------------------------------------

        public void InitLoot( List<Adversaire> adversaires )
        {
            List <Item> items = RollLoot(adversaires);
            foreach(Item item in items)
            {
                _lootPanel.AddNewObject(item);
            }
        }

        //------------------------------------------------------------------------
        #endregion

        public void ReturnToMap()
        {

            GameManager.Instance.ReturnToMap();
        }
    }
}