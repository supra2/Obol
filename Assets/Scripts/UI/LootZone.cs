using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LootZone : MonoBehaviour, IDropHandler
{

    #region Member
    [SerializeField]
    protected RectTransform _contentTransform;
    #endregion

    #region Interface Implementation

    /// <summary>
    /// OnDrop
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop( PointerEventData eventData )
    {
        GameObject dropped = eventData.pointerDrag;
        Draggable draggableItem = dropped.GetComponent<Draggable>();
        draggableItem._parentAfterDrag = _contentTransform;
    }

    #endregion

}