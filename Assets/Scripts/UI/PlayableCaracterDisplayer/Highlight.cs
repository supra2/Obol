using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highlight : MonoBehaviour,IHighlight
{
    #region Members
    #region Visible
    [SerializeField]
    protected Image _graphic;
    #endregion
    #region Hidden
    protected bool _show;
    protected Color _color;
    #endregion
    #endregion

    #region Public Members

    public void Display(bool display)
    {
        _show = display;  
    }

    public void SetColor(Color color)
    {
        _color = color; 
    }

    void Update()
    {
        _graphic.color = _color;
        _graphic.enabled =_show;
    }

    #endregion 
}
