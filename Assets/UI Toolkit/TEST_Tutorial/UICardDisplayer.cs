using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UIElements;

public class UICardDisplayer
{

    #region Hidden
    protected VisualElement _visualElement;

    protected LocalizeStringEvent nametext;

    protected LocalizeStringEvent descriptionText;

    protected Sprite sprite;

    protected VisualElement Illustration;
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
        VisualElement Illustration = _visualElement.Q<VisualElement>("Illustration");
        _visualElement.Q<VisualElement>("Illustration");
        textName = _visualElement.Q<Label>("TitleText");
        textDescription = _visualElement.Q<Label>("TitleText");
    }

    public void SetCard(ICard card)
    {
        Illustration.style.backgroundImage = new StyleBackground(card.GetIllustration());
        nametext = new LocalizeStringEvent();
        nametext.SetTable("GameValues");
        nametext.SetEntry(card.TitleKey());
        descriptionText = new LocalizeStringEvent();
        descriptionText.SetTable("GameValues");
        descriptionText.SetEntry(card.DescriptionKey());
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
