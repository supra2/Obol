using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class CharacterDatabase : EditorWindow
{
    private Sprite m_DefaultItemIcon;

    [MenuItem("Obol/Character Database")]
    public static void Init()
    {
        CharacterDatabase wnd = GetWindow<CharacterDatabase>();
        wnd.titleContent = new GUIContent("CharacterDatabase");
    }

    public void CreateGUI()
    {
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>
            ("Assets/WUG/Editor/ItemDatabase.uxml");
        VisualElement rootFromUXML = visualTree.Instantiate();
        rootVisualElement.Add(rootFromUXML);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>
            ("Assets/WUG/Editor/ItemDatabase.uss");
        rootVisualElement.styleSheets.Add(styleSheet);

        m_DefaultItemIcon = (Sprite)AssetDatabase.LoadAssetAtPath(
            "Assets/WUG/Sprites/UnknownIcon.png", typeof(Sprite));
    }
}