using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core.Items;
namespace UI.ItemSystem
{ 
    /// <summary>
    /// Manage Inventory Content
    /// </summary>
    public class InventoryUIManager : MonoBehaviour
    {

        #region Enum

        public enum Mode
        {
            _LootMode,
            _TradeMode,
            _inventoryMode
        }

        #endregion

        #region Members
        #region Visible
        [SerializeField]
        protected GridLayoutGroup _gridLayout;
        [SerializeField]
        protected RectTransform _dropZone;
        [SerializeField]
        protected ItemDisplayer _itemDisplayer;
        [SerializeField]
        protected RectTransform _contentReference;
        #endregion
        #region Hidden

        /// <summary>
        /// Item List
        /// </summary>
        protected List<Item> _items;

        protected List<RectTransform> _dropzones;

        protected Mode _currentMode;
        #endregion
        #endregion


        #region Getter 

        #endregion

        #region Public Methods
        //------------------------------------------------

        public void Init(Mode mode)
        {
            _dropzones = new List<RectTransform>();
            int inventorySize = PartyManager.Instance.InventorySize;
            for( int j =0 ; j < inventorySize ; j++ )
            {
                RectTransform instance= GameObject.Instantiate(_dropZone, _contentReference) as RectTransform;
                instance.gameObject.SetActive(true);
                _dropzones.Add(instance );
            }
            int i = 0;
            foreach ( Item item in PartyManager.Instance.Inventory )
            {
                ItemDisplayer itemDisplayer= Instantiate( _itemDisplayer );
                itemDisplayer.item = item;
                itemDisplayer.transform.SetParent( _dropzones[i] );
                i++;
            }
        }

        //------------------------------------------------
        #endregion

    }
}