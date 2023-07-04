using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

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
    #endregion
    #endregion

    #region Getters
    public ICard Card => _baseCard;
    #endregion 

    #region Initialisation
    public void Init()
    {
        _currentMode = CardMode.NotInteractable;
    }
    #endregion

    #region Public Methods
    //_______________________________________________________

    public async Task Replace(Vector2 newposition, Quaternion newRotation, float replacementTime)
    {
        Vector2 originPosition = (transform as RectTransform).position;
        Quaternion originRotation = (transform as RectTransform).rotation;
        float t = 0f;
        while (t < replacementTime)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Slerp(originPosition, newposition, t / replacementTime);
            transform.rotation = Quaternion.Slerp(originRotation, newRotation, t / replacementTime);
            await Task.Delay(10);
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
    #endregion
}
