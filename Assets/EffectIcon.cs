using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField]
    protected TextMeshProUGUI _counter;
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


    public void Setup(IAlteration alteration)
    {
      _icon.sprite =  GameObject.Instantiate( Resources.Load(alteration.GetIconPath())) as Sprite;
        UpdateAlteration(alteration);
    }

    public void UpdateAlteration(IAlteration alteration)
    {
        switch( alteration.AlterationType())
        {
            case AlterationType.Bleeding:
                Bleed bleed = (Bleed)alteration;
                _counter.text = bleed.BleedValue.ToString();
                break;
        }
    }
}
