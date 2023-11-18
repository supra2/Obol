using System;
using UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UI.ItemSystem;

public class InventorySlot : Slot
{

    #region Members
    #region Visible
    [SerializeField]
    public OptionList _optionsList;
    #endregion
    #endregion

    #region Public Method

    public override void OnDrop(PointerEventData eventData)
    {
        base.OnDrop(eventData);
        Draggable draggableItem = _slot.GetComponent<Draggable>();
        RightClickHandler RCH = _slot.AddComponent<RightClickHandler>();
        if( RCH.RightClicked == null)
        {
            RCH.RightClicked = new UnityEvent();
        }
        RCH.RightClicked.AddListener( () => _optionsList.Show(true) );
        draggableItem._parentAfterDrag = transform;

        ItemDisplayer itemDisplayer = draggableItem.
            gameObject.GetComponent<ItemDisplayer>();
        GameManager.Instance.PartyManager.Inventory.Add( 
            itemDisplayer.Item );
    }


    /// <summary>
    /// Add an option in the right click menu
    /// </summary>
    /// <param name="action"></param>
    /// <param name="locKey"></param>
    public void AddRightClickOption(Action action, string locKey)
    {
        _optionsList.AddOptions( action, locKey );
    }

    
    /// <summary>
    /// remove option from contextual list ( accessible on right click/long press on mobile device)
    /// </summary>
    /// <param name="action"></param>
    public void RemoveRigthClickOption(Action action)
    {
        _optionsList.RemoveOption(action);
    }

    #endregion

    #region Inner Method

    public override void UnSlot(Draggable draggable)
    {
        ItemDisplayer itemDisplayer = draggable.gameObject.GetComponent<ItemDisplayer>();
        GameManager.Instance.PartyManager.Inventory.Remove( itemDisplayer.Item );
        Destroy(_slot.GetComponent<RightClickHandler>());
    }

    #endregion

}
