using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Attack))]
[CanEditMultipleObjects]
public class AttackDrawer : Editor
{

    #region Members
    SerializedProperty Stamina;
    SerializedProperty DescriptionKey;
    SerializedProperty Effect;
    SerializedProperty NameKey;
    SerializedProperty Illustration;
    #endregion

    #region Initialisation
    void OnEnable()
    {
        // Fetch the objects from the GameObject script to display in the inspector
        Stamina = serializedObject.FindProperty("_stamina");
        DescriptionKey = serializedObject.FindProperty("_descriptionKey");
        NameKey = serializedObject.FindProperty("_descriptionKey");
        Effect = serializedObject.FindProperty("_descriptionKey");
        Illustration = serializedObject.FindProperty("_illustration");
    }
    #endregion

    #region Inspector GUI
    public override void OnInspectorGUI()
    {
        //The variables and GameObject from the MyGameObject script are displayed in the Inspector with appropriate labels
        EditorGUILayout.PropertyField(Stamina, new GUIContent("Int Field"), GUILayout.Height(20));
        EditorGUILayout.PropertyField(NameKey, new GUIContent("Name "));
        EditorGUILayout.PropertyField(DescriptionKey, new GUIContent("DescriptionKey "));
        EditorGUILayout.PropertyField(Effect, new GUIContent("Effect "), GUILayout.Height(100));
        EditorGUILayout.PropertyField(Illustration, new GUIContent("Illustration"));

        // Apply changes to the serializedProperty - always do this at the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();
    }
    #endregion

}
