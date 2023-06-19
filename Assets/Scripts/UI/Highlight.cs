using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highlight : MonoBehaviour
{
    #region Members
    #region Visible
    [SerializeField]
    protected Transform _graphic;
    #endregion
    #endregion

    #region Public Members
    public void Display(bool display)
    {
        _graphic.gameObject.SetActive(display);
    }

    public void SetColor(Color color)
    {
        _graphic.GetComponent<Image>().color=color;
    }
    #endregion 
}
