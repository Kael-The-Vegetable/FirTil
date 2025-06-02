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

    [SerializeField] SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deployedItem = null;
        spriteRenderer.enabled = false;
        isPlaceable = false;
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

    public bool IsAlreadyTilled()
    {
        if (isPlaceable &&  deployedItem == null)
        {
            Debug.Log("IsTiled");
            return true;
        }
        else
        {
			Debug.Log("NotTiled");
            return false;
		}
    }

    public void Dig()
    {
        if ( !isPlaceable)
        {
            isPlaceable = true;
            spriteRenderer.enabled = true;
        }
        else if (deployedItem)
        {
			Destroy(deployedItem);
			deployedItem = null;
		}
        
    }
}
