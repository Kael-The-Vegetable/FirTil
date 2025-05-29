using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(PathSpriteState))]
public class PathSpriteStateEditor : PropertyDrawer
{
	public VisualTreeAsset vt;
	public override VisualElement CreatePropertyGUI(SerializedProperty property)
	{
		VisualElement root = new VisualElement();
		vt.CloneTree(root);

		var spriteDisplay = root.Q<VisualElement>("Display");
		var spriteProp = property.FindPropertyRelative("_sprite");
		if (spriteProp != null && spriteProp.objectReferenceValue is Sprite sprite)
		{
			spriteDisplay.style.backgroundImage = new StyleBackground(sprite.texture);
		}

		root.Q<VisualElement>("Sprite").RegisterCallback<ChangeEvent<Object>>(evt =>
		{
			var newSprite = evt.newValue as Sprite;
			spriteDisplay.style.backgroundImage = newSprite != null ? new StyleBackground(newSprite) : null;
		});

		return root;
	}

}