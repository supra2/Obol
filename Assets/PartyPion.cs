using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PartyPion : MonoBehaviour
{

    #region Members
    #region Visible

    [SerializeField]
    protected SpriteRenderer _image;

    #endregion
    #endregion

    #region Getter
    protected Sprite Portrait
    {

        get => _image.sprite;
        set => _image.sprite = value;
    }
    #endregion

    #region Public Method


   
    #endregion

}
