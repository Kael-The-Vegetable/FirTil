using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(IntRange))]
public class IntRangeEditor : PropertyDrawer
{
	public VisualTreeAsset vt;
	public override VisualElement CreatePropertyGUI(SerializedProperty property)
	{
		VisualElement root = new VisualElement();
		vt.CloneTree(root);
		var nameContainer = root.Q<Label>("Name");
		nameContainer.text = ObjectNames.NicifyVariableName(property.name);
		return root;
	}
}
