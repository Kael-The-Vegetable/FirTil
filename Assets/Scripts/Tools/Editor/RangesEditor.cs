#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(FloatRange))]
public class RangesEditor : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);

		// Draw label
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

		// Don't make child fields be indented
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;
		EditorGUILayout.BeginHorizontal();
		// Calculate rects
		var min = new Rect(position.x, position.y, 50, position.height);
		var max = new Rect(position.x + 55, position.y, 50, position.height);

		EditorGUILayout.EndHorizontal();
		// Draw fields - pass GUIContent.none to each so they are drawn without labels
		EditorGUI.PropertyField(min, property.FindPropertyRelative("_min"), GUIContent.none);
		EditorGUI.PropertyField(max, property.FindPropertyRelative("_max"), GUIContent.none);

		// Set indent back to what it was
		EditorGUI.indentLevel = indent;

		EditorGUI.EndProperty();
	}
}
#endif