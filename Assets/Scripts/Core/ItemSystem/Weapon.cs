using System;
using System.Collections;
using System.Collections.Generic;
using UI.ItemSystem;
using UnityEngine;

namespace Core.Items
{
    [CreateAssetMenu(fileName = "Weapon",
          menuName = "Obol/Items/Weapon",
          order = 1)]
    [Serializable]
    public class Weapon : Item
    {

        #region Members
        #region Visible
        [SerializeField]
        protected int _cardID;
        #endregion
        #endregion

        #region UI Callback
        public void Slotted( InventorySlot slot )
        {
            slot.AddRightClickOption( Equip,Localisation_UI_Constants.EQUIPKEY);
        }
        public void UnSlotted(InventorySlot slot)
        {
            InventoryUIManager inventoryUIManager =
                slot.transform.GetComponentInParent<InventoryUIManager>();
            slot.RemoveRigthClickOption(Equip);
        }
        #endregion

        #region InnerMethod

        protected void Equip()
        { 
        
        }
        protected void DeEquip()
        {

        }

        #endregion
    }

}
