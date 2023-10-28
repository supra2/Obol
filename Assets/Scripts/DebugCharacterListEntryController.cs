using Core.FightSystem;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UIElements;

public class CharacterListEntryController
{
    Label m_NameLabel;
    LocalizeStringEvent localizeStringevent;

    public void SetVisualElement(VisualElement visualElement)
    {
        m_NameLabel = visualElement.Q<Label>("CharacterName");
    }

    public void SetCharacterData(Character characterData)
    {

        localizeStringevent = new LocalizeStringEvent();
        localizeStringevent.SetTable("GameValues");
        localizeStringevent.SetEntry(characterData.CharacterNameKey);
        m_NameLabel.text = localizeStringevent.StringReference.GetLocalizedString();
    }
}
