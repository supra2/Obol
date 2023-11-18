using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BaseCardDisplayer))]
public class Draggable : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{

    #region Members
    #region hidden
    public static BaseCardDisplayer _cardDisplayer;

    public static bool _dragged;

    protected RectTransform _originalParent;

    protected Vector3 _originalLocalPosition;

    [HideInInspector] public Transform _parentAfterDrag;
    #endregion
    #endregion

    #region Event
    /// <summary>
    /// Start Drag
    /// </summary>
    public UnityEvent _onStartDrag;
    /// <summary>
    /// End Drag
    /// </summary>
    public UnityEvent _onEndDrag;
    /// <summary>
    /// End Slot
    /// </summary>
    public UnityEvent _onEndSlot;
    /// <summary>
    /// current Slot
    /// </summary>
    public Slot _slot;
    #endregion

    #region InterfaceImplementation 
    //-------------------------------------------------------


    /// <summary>
    /// On Begin Drag 
    /// </summary>
    /// <param name="eventData"> Event Data </param>
    public void OnBeginDrag( PointerEventData eventData)
    {
        _cardDisplayer = GetComponent<BaseCardDisplayer>();
        _originalParent = (RectTransform)_cardDisplayer.transform.parent;
        _originalLocalPosition = ((RectTransform)_cardDisplayer.transform).anchoredPosition;
        _cardDisplayer.transform.SetParent(
            transform.GetComponentInParent<Canvas>().transform );
        BaseCardDisplayer baseCardDisplayer =
            GetComponent<BaseCardDisplayer>();
        baseCardDisplayer.SetInterceptRayCast(false);
        _dragged = true;
        _onStartDrag?.Invoke();
    }

    //----------------------------------------------------------------------------------

    /// <summary>
    /// On Drag 
    /// </summary>
    /// <param name="eventData"> Event Data </param>
    public void OnDrag( PointerEventData eventData )
    {
        if (_cardDisplayer == null)
            return;

        _cardDisplayer.transform.position = Input.mousePosition;
    }

    //----------------------------------------------------------------------------------

    /// <summary>
    /// On End Drag 
    /// </summary>
    /// <param name="eventData"> Event Data </param>
    public void OnEndDrag(PointerEventData eventData)
    {
        if( _slot != null )
        {
            _slot.UnSlot(this);
        }
        _dragged = false;
        BaseCardDisplayer baseCardDisplayer =
            GetComponent<BaseCardDisplayer>();
        baseCardDisplayer.SetInterceptRayCast(true);
        if( _parentAfterDrag != null &&
            _parentAfterDrag.name!= "LootContent")
        {
            transform.SetParent(_parentAfterDrag);
            ((RectTransform)_cardDisplayer.transform).anchorMin = new Vector2(0, 0);
            ((RectTransform)_cardDisplayer.transform).anchorMax = new Vector2(1, 1);
            ((RectTransform)_cardDisplayer.transform).offsetMin = new Vector2(0, 0);
            ((RectTransform)_cardDisplayer.transform).offsetMax = new Vector2(0, 0);
        }
        else
        {
            transform.SetParent(_originalParent);
            
        }
        _slot = transform.GetComponent<Slot>();
        _onEndDrag?.Invoke();
    }

    //----------------------------------------------------------------------------------

    /// <summary>
    /// draggable unsloted
    /// </summary>
    /// <param name="draggable"></param>
    public void UnSlot( Draggable draggable )
    {
        _onEndSlot?.Invoke();
    }

    //----------------------------------------------------------------------------------
    #endregion

}
