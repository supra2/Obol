using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Core.FightSystem;
using System.IO;
using System;


public class CharacterDatabase : EditorWindow
{

    #region Members
    private Sprite _defaultItemIcon;
    private List<PlayableCharacter> _characterDatabase;
    private static VisualTreeAsset _itemRowTemplate;
    private ListView _itemListView;
    private float _itemHeight;
    private VisualElement _itemsTab;
    #endregion

    #region Initialization

    [MenuItem("Obol/Character Database ")]
    public static void Init()
    {
        CharacterDatabase wnd =  GetWindow<CharacterDatabase>();
        wnd.titleContent = new GUIContent("CharacterDatabase");
    }

    #endregion

    #region  GUI_Create

    public void CreateGUI()
    {

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>
            ("Assets/UI Toolkit/Tooling/CharacterDatabase.uxml");
   
        VisualElement rootFromUXML = visualTree.Instantiate();

        rootVisualElement.Add(rootFromUXML);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>
            ("Assets/UI Toolkit/Tooling/CharacterDatabase.uss");
     
        rootVisualElement.styleSheets.Add(styleSheet);
        _defaultItemIcon = (Sprite)AssetDatabase.LoadAssetAtPath(
           "Assets/Textures/UI/ToolingIcon/UnknownIcon.png", typeof(Sprite));

       
        _itemsTab = rootVisualElement.Q<VisualElement>("CharacterTab");
        LoadAllItems();
        GenerateListView();

    }

    private void LoadAllItems()
    {

        if(_characterDatabase == null)
        _characterDatabase = new List<PlayableCharacter>();
        _characterDatabase.Clear();

        string[] allPaths = Directory.GetFiles("Assets/Resources/Character/", "*.asset", 
            SearchOption.AllDirectories);

        foreach (string path in allPaths)
        {
            string cleanedPath = path.Replace("\\", "/");
            _characterDatabase.Add((PlayableCharacter)AssetDatabase.LoadAssetAtPath( cleanedPath,
                typeof(PlayableCharacter) ));
        }
    }
    
    private void GenerateListView()
    {
      
        Func<VisualElement> makeItem = () => _itemRowTemplate.CloneTree();
        Action<VisualElement, int> bindItem =(e,i ) => 
        {
            e.Q<VisualElement>("Icon").style.backgroundImage = 
            _characterDatabase[i].Portrait == null ? _defaultItemIcon.texture :
            _characterDatabase[i].Portrait.texture;
            // Todo : Localisation
            e.Q<Label>("Name").text = _characterDatabase[i].CharacterNameKey;
        };
         _itemListView= new ListView(_characterDatabase, 35, makeItem, bindItem);
         _itemListView.selectionType = SelectionType.Single;
         _itemListView.style.height = _characterDatabase.Count * _itemHeight;
         _itemsTab.Add(_itemListView);
        
    }

    #endregion
}