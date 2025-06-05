using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()

[CreateAssetMenu(fileName = "Seed", menuName = "Plant/Seed")]
public class Seed : ScriptableObject
{
	[field: SerializeField] public PlantData Plant { get; private set; }
	[field: SerializeField] public Sprite PlantImage { get; private set; }
	[field: SerializeField] public IntRange Availability { get; private set; }
	[field: SerializeField] public IntRange VariableCost { get; private set; }
	[field: SerializeField] public int MaxStackSize { get; private set; }

	public static bool operator ==(Seed a, Seed b)
	{
		return a?.Plant == b?.Plant && a?.name == b?.name;
	}
	public static bool operator !=(Seed a, Seed b)
	{
		return !(a == b);
	}
}

#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)