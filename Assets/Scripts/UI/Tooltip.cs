using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{

    #region members

    [SerializeField]
    protected TextMeshProUGUI _text;

    [SerializeField]
    protected Animator _animator;

    #endregion

    #region Public Methods

    public void SetContent( string content )
    {
        _text.text = content;
    }

    public void SetPosition( Vector2 position )
    {
        ((RectTransform) transform).position = position;
    }

    public void Show ( )
    {
        ResetAllTriggers();
        _animator.SetTrigger("Show");
    }

    public void Hide()
    {
        ResetAllTriggers();
        _animator.SetTrigger("Hide");
    }

    public void ResetAllTriggers()
    {
        _animator.ResetTrigger("Hide");
        _animator.ResetTrigger("Show");
    }
    #endregion

}
