using Core.FightSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UIElements;

public class CharacterListController
{

    #region Members
    // UXML template for list entries
    // UI element references
    ListView m_CharacterList;
    Label m_CharClassLabel;
    Label m_CharNameLabel;
    VisualElement m_CharPortrait;
    Button m_SelectCharButton;
    VisualTreeAsset m_ListEntryTemplate;
    List<Character> m_AllCharacters;
    LocalizeStringEvent localizeStringevent;
    #endregion
    void EnumerateAllCharacters()
    {
        m_AllCharacters = new List<Character>();
        m_AllCharacters.AddRange(Resources.LoadAll<Character>("Character"));


    }

    public void InitializeCharacterList(VisualElement root, VisualTreeAsset listElementTemplate)
    {

        EnumerateAllCharacters();

        // Store a reference to the template for the list entries
        m_ListEntryTemplate = listElementTemplate;

        // Store a reference to the character list element
        m_CharacterList = root.Q<ListView>("CharacterList");

        // Store references to the selected character info elements
        m_CharClassLabel = root.Q<Label>("CharacterClass");
        m_CharNameLabel = root.Q<Label>("CharacterName");
        m_CharPortrait = root.Q<VisualElement>("CharacterPortrait");

        // Store a reference to the select button
        m_SelectCharButton = root.Q<Button>("SelectCharButton");

        FillCharacterList();
        // Register to get a callback when an item is selected
        m_CharacterList.selectionChanged += OnCharacterSelected;
    }

    void FillCharacterList()
    {
        // Set up a make item function for a list entry
        m_CharacterList.makeItem = () =>
        {
            // Instantiate the UXML template for the entry
            var newListEntry = m_ListEntryTemplate.Instantiate();

            // Instantiate a controller for the data
            var newListEntryLogic = new CharacterListEntryController();

            // Assign the controller script to the visual element
            newListEntry.userData = newListEntryLogic;

            // Initialize the controller script
            newListEntryLogic.SetVisualElement(newListEntry);

            // Return the root of the instantiated visual tree
            return newListEntry;
        };

        m_CharacterList.bindItem = (item, index) =>
        {
            (item.userData as CharacterListEntryController).SetCharacterData(m_AllCharacters[index]);
        };
        // Set a fixed item height
        m_CharacterList.fixedItemHeight = 45;

        // Set the actual item's source list/array
        m_CharacterList.itemsSource = m_AllCharacters;
    }

    public void OnCharacterSelected(IEnumerable<object> Selection)
    {
        // Get the currently selected item directly from the ListView
        var selectedCharacter = m_CharacterList.selectedItem as PlayableCharacter;

        // Handle none-selection (Escape to deselect everything)
        if (selectedCharacter == null)
        {
            // Clear
            m_CharClassLabel.text = "";
            m_CharNameLabel.text = "";
            m_CharPortrait.style.backgroundImage = null;

            // Disable the select button
            m_SelectCharButton.SetEnabled(false);

            return;
        }
         localizeStringevent = new LocalizeStringEvent();
        localizeStringevent.SetTable("GameValues");
        localizeStringevent.SetEntry(selectedCharacter.CharacterNameKey);
        // Fill in character details
        m_CharClassLabel.text = "undefined";
        m_CharNameLabel.text = localizeStringevent.StringReference.GetLocalizedString();
        m_CharPortrait.style.backgroundImage = new StyleBackground( selectedCharacter.Portrait );

        // Enable the select button
        m_SelectCharButton.SetEnabled(true);
    }

}