using Core.FightSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(Character))]
public class CharactersDrawer : PropertyDrawer
{

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        // Create property container element.
        var container = new VisualElement();

        var intelligenceField = new PropertyField ( property.FindPropertyRelative("_intelligence") );
        var strengthField = new PropertyField(property.FindPropertyRelative("_strength"));

        var constField = new PropertyField();
        return container;
    }

}
