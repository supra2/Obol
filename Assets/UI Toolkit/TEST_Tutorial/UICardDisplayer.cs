using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UIElements;

public class UICardDisplayer
{

    #region Hidden
    protected VisualElement _visualElement;
   
    protected LocalizedString nametext;

    protected LocalizedString descriptionText;

    protected Sprite sprite;

    protected VisualElement _illustration;
    /// <summary>
    /// Text Name
    /// </summary>
    protected Label textName;
    /// <summary>
    /// TextDescription 
    /// </summary>
    protected Label textDescription;
    #endregion

    #region Initialisation

    public UICardDisplayer(VisualElement root)
    {
        _visualElement = root;
         _illustration = _visualElement.Q<VisualElement>("Illustration");
        _visualElement.Q<VisualElement>("Illustration");
        textName = _visualElement.Q<Label>("TitleText");
        textDescription = _visualElement.Q<Label>("Description");
        nametext = new LocalizedString();
        nametext.StringChanged += UpdateName;
        descriptionText = new LocalizedString();
        descriptionText.TableReference="GameValues";
        nametext.StringChanged += UpdateDescription;
    }

    public void SetCard(ICard card)
    {
        _illustration.style.backgroundImage = new StyleBackground(card.GetIllustration());
        
        nametext.TableReference ="GameValues";
        nametext.TableEntryReference = card.TitleKey();
        
        descriptionText.TableReference ="GameValues";
        descriptionText.TableEntryReference = card.DescriptionKey();
        UpdateName(nametext.GetLocalizedString());
        UpdateDescription(descriptionText.GetLocalizedString());

    }

    public void UpdateName(string name)
    {
        textName.text
            = name;
    }

    public void UpdateDescription(string description)
    {
        textDescription.text
           = description;
    }
    public void Show (  )
    {
        _visualElement.visible = true;
    }

    public void Hide()
    {
        _visualElement.visible = false;
    }
    #endregion
}
