using UnityEngine;
using UnityEditor;

public class ShowOnlyAttribute : PropertyAttribute
{
}

[CustomPropertyDrawer(typeof(ShowOnlyAttribute))]
public class ShowOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        string valueStr;

        switch (prop.propertyType)
        {
            case SerializedPropertyType.Integer:
                valueStr = $"{prop.intValue}";
                break;
            case SerializedPropertyType.Boolean:
                valueStr = $"{prop.boolValue}";
                break;
            case SerializedPropertyType.Float:
                valueStr = $"{prop.floatValue}";
                break;
            default:
                valueStr = "Not Supported";
                break;
        }

        EditorGUI.LabelField(position, label.text, valueStr);
    }
}
