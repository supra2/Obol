using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;
using Core.Items;

public class BaseCardDisplayer : MonoBehaviour
{

    #region Members
    #region Visible
    [Header(" UI WIDGET")]
    [SerializeField]
    protected TextMeshProUGUI _title;
    [SerializeField]
    protected Image _image;
    [SerializeField]
    protected TextMeshProUGUI _description;
    [SerializeField]
    protected LocalizeStringEvent _titleKeyString;
    [SerializeField]
    protected LocalizeStringEvent _descriptionKeyString;
    #endregion
    #endregion

    #region Protected Members

    protected void Set(string DescriptionKeyString , string TitleKeyString ,Sprite Illustration)
    {
        _descriptionKeyString.SetEntry(DescriptionKeyString);
        _titleKeyString.SetEntry(TitleKeyString);
        _image.sprite = Illustration;
    }

    public void SetInterceptRayCast( bool intercept)
    {
        Image[] graphics = transform.GetComponentsInChildren<Image>();
    
        foreach(Image img in graphics)
        {
            img.raycastTarget = intercept;
        }
        TextMeshProUGUI[] textmeshpro = transform.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI textmesh in textmeshpro)
        {
            textmesh.raycastTarget = intercept;
        }
    }

    #endregion

}

namespace UI.ItemSystem

{ 
    public class ItemDisplayer: BaseCardDisplayer
    {

        #region Members
        #region hidden
        protected Item _item;
        #endregion
        #endregion

        #region Getters

        public Item Item
        {
            get => _item;
            set
            {
                _item = value;
                Set(_item.DescriptionKey, _item.TitleKey, _item.Illustration);
            }
        }

        #endregion

    }
}