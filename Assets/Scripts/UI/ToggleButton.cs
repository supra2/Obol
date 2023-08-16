using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ToggleButton : MonoBehaviour
{
    #region Members
    /// <summary>
    /// Background : 
    /// </summary>
    [SerializeField]
    protected Image _background;
    /// <summary>
    /// Selected : 
    /// </summary>
    [SerializeField]
    protected Sprite _Selected;
    /// <summary>
    /// UnSelected  Sprite : 
    /// </summary>
    [SerializeField]
    protected Sprite _Unselected;

    protected bool selected;
    #endregion

    #region Public Methods
    public void ToggleSelected()
    {
        selected = !selected;
        _background.sprite = selected ? _Selected : _Unselected;
    }

    #endregion


}
