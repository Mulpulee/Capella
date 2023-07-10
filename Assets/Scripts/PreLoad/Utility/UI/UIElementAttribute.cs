using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


#if UNITY_EDITOR
namespace UnityEditor
{
    [CustomPropertyDrawer(typeof(UIElementAttribute), true)]
    public class UIElementAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = !Application.isPlaying && ((UIElementAttribute)attribute).runtimeOnly;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
}
#endif

[AttributeUsage(AttributeTargets.Field)]
public class UIElementAttribute: PropertyAttribute
{
    public readonly bool runtimeOnly;

    public UIElementAttribute(bool runtimeOnly = false)
    {
        this.runtimeOnly = runtimeOnly;
    }
}