using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using Core.CardSystem;
using UnityEngine.Localization;

[Serializable]
public class UnityTooltipEvent:UnityEvent<string,Vector2>
{

}

public class DeckDisplayer : MonoBehaviour,IPointerUpHandler,IPointerDownHandler,IPointerEnterHandler, IPointerExitHandler
{

    #region Members
    #region Visible
    [SerializeField]
    protected Image image;
    [SerializeField]
    protected float hoverTime;
    [SerializeField]
    protected LocalizedString _tooltipLocalisation;

    public UnityTooltipEvent ShowToolTip;
    public UnityEvent HideTooltip;
    #endregion
    #region Hidden
    protected bool pointerDown;
    protected bool _hover;
    protected Deck<PlayerCard> _deck;
    protected Coroutine _coroutine;
    #endregion
    #endregion

    #region Getter
    public Deck<PlayerCard> Deck => _deck;
    #endregion

    public void Init(Deck<PlayerCard> deck)
    {
        _deck = deck;
    }

    public void OnEnable()
    {
        ShowToolTip.AddListener(TooltipManager.Instance.ShowTooltip);
        HideTooltip.AddListener(TooltipManager.Instance.HideTooltip);

    }

    public void OnDisable()
    {
        ShowToolTip.RemoveListener(TooltipManager.Instance.ShowTooltip);
        HideTooltip.RemoveListener(TooltipManager.Instance.HideTooltip);

    }

    #region UnityEvent 
    public void OnPointerDown(PointerEventData eventData)
    {
#if UNITY_ANDROID && UNITY_IOS && !UNITY_EDITOR
         if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(ShowDeckTooltip());
#endif
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    #if UNITY_STANDALONE || UNITY_EDITOR  
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _coroutine = StartCoroutine(ShowDeckTooltip());
#endif
    }

    public void OnPointerExit(PointerEventData eventData)
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        HideTooltip?.Invoke();
#endif
    }

    public void OnPointerUp(PointerEventData eventData)
    {
#if UNITY_ANDROID && UNITY_IOS && !UNITY_EDITOR
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        HideTooltip?.Invoke();
#endif
    }
    #endregion

    #region Protected method

    protected IEnumerator ShowDeckTooltip()
    {
        yield return new WaitForSeconds(hoverTime);
        ShowToolTip?.Invoke(
            string.Format( _tooltipLocalisation.GetLocalizedString() ,
            _deck.Count ) , (Vector2)((RectTransform)transform).position);
    }

    #endregion

}
