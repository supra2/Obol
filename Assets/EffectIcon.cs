using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectIcon : MonoBehaviour
{

    #region Members
    /// <summary>
    /// Image : icon
    /// </summary>
    [SerializeField]
    protected Image _icon;
    #endregion 

    #region Getter
    /// <summary>
    /// Sprite
    /// </summary>
    public Sprite iconSprite
    {
        set
        {
            _icon.sprite = value;
        }
    }
    #endregion

}
