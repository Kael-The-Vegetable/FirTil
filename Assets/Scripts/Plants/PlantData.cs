using UnityEngine;

[CreateAssetMenu(fileName = "NewPlantData", menuName = "Plant/PlantData")]
public class PlantData : ScriptableObject
{

	public enum PlantType
	{
		Attack,
		Support,
		Buff,
		Debuff
	}
	public PlantType type = PlantType.Attack;

	public enum PlaceableOn
	{
		Soil,
		Lane,
		Both
	}
	public PlaceableOn placeableOn = PlaceableOn.Soil;
	public GameObject PlantPrefab;

	public string PlantName;

	public float BaseGrowthTime;
	public float BaseGrowthRate;

	public float BaseFireRate;
	public float BaseRange;

}
