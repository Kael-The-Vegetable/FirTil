using UnityEngine;

public class TilePlot : MonoBehaviour
{
    [SerializeField] GameObject deployedItem;
    [SerializeField] bool isPlaceable;

    enum PlotType
    {
        Soil,
        Lane
    }
    [SerializeField] PlotType type = PlotType.Soil;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deployedItem = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaceNewPlant(PlantData plant)
    {
        
        if (deployedItem == null && isPlaceable)
        {
			if (plant.placeableOn == PlantData.PlaceableOn.Soil && type == PlotType.Soil)
			{
				deployedItem = Instantiate(plant.PlantPrefab, transform);
			}
            else if (plant.placeableOn == PlantData.PlaceableOn.Lane && type == PlotType.Lane)
            {
				deployedItem = Instantiate(plant.PlantPrefab, transform);
			}
            else if (plant.placeableOn == PlantData.PlaceableOn.Both)
            {
				deployedItem = Instantiate(plant.PlantPrefab, transform);
			}
			
        }
    }

    public void DigUpPlant()
    {
        if (deployedItem)
        {
			Destroy(deployedItem);
			deployedItem = null;
		}
        
    }
}
