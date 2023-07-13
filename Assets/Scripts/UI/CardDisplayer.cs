using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using System;

public class CardDisplayer : MonoBehaviour, IPointerUpHandler
{

    #region enum
    public enum CardMode
    {
        NotInteractable,
        Display_Hand,
        Pickable,
        Playable
    }
    #endregion

    #region Members
    #region Visible 
    [SerializeField]
    protected ICard _baseCard;
    [SerializeField]
    protected Image _illustration;
    [SerializeField]
    protected Animator _animator;
    [SerializeField]
    protected Image _backgroundImage;
    [SerializeField]
    protected LocalizeStringEvent _titleLocalisation;
    [SerializeField]
    protected LocalizeStringEvent _descriptionLocalisation;
    [SerializeField]
    protected Highlight _highlight;
    [SerializeField]
    protected Color _colorSelectable;
    [SerializeField]
    protected Color _colorSelected;
    public UnityCardEvent CardPicked;
    public UnityCardEvent CardUnpicked;
    #endregion
    #region hidden
    protected CardMode _currentMode;
    protected ICard _underlyingCard;
    protected bool _picked;
    protected Action<CardDisplayer> _callback;
    #endregion
    #endregion

    #region Getters

    public ICard Card {

        get => _baseCard;
        set 
        {
            _baseCard = value;
            _illustration.sprite = _baseCard.GetIllustration();
            _titleLocalisation.SetEntry( _baseCard.TitleKey());
            _descriptionLocalisation.SetEntry(_baseCard.DescriptionKey());
        }
    }

    public Vector2 Size
    {
        get
        { 
           return _backgroundImage.rectTransform.sizeDelta; 
        }
    }

    #endregion 

    #region Initialisation

    public void Init()
    {
        _currentMode = CardMode.NotInteractable;
    }

    #endregion

    #region Public Methods
    //_______________________________________________________

    /// <summary>
    ///  animate the card displayer from its current position to the 
    ///  one passed as arguments ( same for orientation) in a given
    ///  duration
    /// </summary>
    /// <param name="newposition"> the target position  </param>
    /// <param name="newRotation"> the target rotation for card </param>
    /// <param name="replacementTime"> Time the animation last</param>
    /// <returns></returns>
    public async Task Replace(Vector2 newposition, 
        Quaternion newRotation, float replacementTime)
    {

        Vector2 output = newposition;

        Vector2 originPosition = 
            (transform as RectTransform).anchoredPosition;

        Quaternion originRotation = 
            (transform as RectTransform).rotation;

        float t = 0f;

        while (t < replacementTime)
        {

            t += Time.deltaTime;
            ((RectTransform)transform).localPosition = 
                Vector3.Slerp(originPosition, newposition, 
                t / replacementTime);
            transform.rotation = 
                Quaternion.Slerp(originRotation, newRotation,
                t / replacementTime);
            await Task.Yield();

        }
    }

    //_______________________________________________________

    public void ChangeMode(CardMode newMode)
    {
        switch (_currentMode)
        {
            case CardMode.Pickable:
                _picked = false;
                break;
            case CardMode.Playable:
                _picked = false;
                break;
        }
        _currentMode = newMode;
        UpdateState();
    }

    //_______________________________________________________

    public void OnPointerUp(PointerEventData eventData)
    {
        switch (_currentMode)
        {
            case CardMode.Pickable:
                TogglePick();
                break;
            case CardMode.Playable:
                TogglePick();
                break;

        }
        UpdateState();
    }

    //_______________________________________________________

    public void PlayDrawAnimation( Action<CardDisplayer> callback)
    {
        _animator.SetTrigger("Drawed");
        _callback = callback;
    }

    //________________________________________________________
    #endregion

    #region Private methods
    //_______________________________________________________

    protected void TogglePick()
    {
        _picked = !_picked;
        if(_picked)
        {
            CardPicked?.Invoke(_baseCard);
        }
        else
        {
            CardUnpicked?.Invoke(_baseCard);
        }
    }

    //_______________________________________________________

    protected void UpdateState()
    {
        switch(_currentMode)
        {
            case CardMode.Pickable:
                _highlight.Display(true);
                _highlight.SetColor(_picked? _colorSelected:_colorSelectable);
            break;
            case CardMode.Playable:
                _highlight.Display(true);
                break;
        }
    }

    //________________________________________________________

    public void AnimationEnded()
    {
        _callback?.Invoke(this);
    }

    //________________________________________________________
    #endregion

}
