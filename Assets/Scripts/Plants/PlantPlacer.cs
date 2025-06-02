using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlantPlacer : MonoBehaviour
{
    enum EquippedItem
    {
        Shovel,
        WateringCan,
        Fertilizer,
        Seed
    }
    [SerializeField] EquippedItem equippedItem = EquippedItem.Shovel;

    [SerializeField] List<PlantData> plants = new();
    [SerializeField] private int equippedPlant = 0;

    [SerializeField] LayerMask plotMask, plantMask;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && TileDetector.Instance.OnValidPlaceableTile())
        {
            switch(equippedItem)
            {
                case EquippedItem.Shovel:
                    
                    if (Physics2D.OverlapPoint(TileDetector.Instance.GetCellPosVector2(), plotMask))
                    {
						Physics2D.OverlapPoint(TileDetector.Instance.GetCellPosVector2(), plotMask).gameObject.GetComponent<TilePlot>().Dig();

					}
                    
                    break;
                case EquippedItem.WateringCan:
                    if (Physics2D.OverlapPoint(TileDetector.Instance.GetCellPosVector2(), plantMask))
                    {
                        Physics2D.OverlapPoint(TileDetector.Instance.GetCellPosVector2(), plantMask).gameObject.GetComponent<IPlant>().WaterPlant();
                    }
                    break;
                case EquippedItem.Fertilizer:
					if (Physics2D.OverlapPoint(TileDetector.Instance.GetCellPosVector2(), plantMask))
					{
						Physics2D.OverlapPoint(TileDetector.Instance.GetCellPosVector2(), plantMask).gameObject.GetComponent<IPlant>().AccelerateGrowth(1.5f, 10);
					}
					break;
                case EquippedItem.Seed:
					if (Physics2D.OverlapPoint(TileDetector.Instance.GetCellPosVector2(), plotMask))
					{
                        Debug.Log("Placed Plant");
						Physics2D.OverlapPoint(TileDetector.Instance.GetCellPosVector2(), plotMask).gameObject.GetComponent<TilePlot>().PlaceNewPlant(plants[equippedPlant]);

					}
					break;
            }
        }

        // Switch equipped item
        if (Keyboard.current.digit1Key.wasPressedThisFrame) equippedItem = EquippedItem.Shovel;
        if (Keyboard.current.digit2Key.wasPressedThisFrame) equippedItem = EquippedItem.WateringCan;
        if (Keyboard.current.digit3Key.wasPressedThisFrame) equippedItem = EquippedItem.Fertilizer;
		if (Keyboard.current.digit3Key.wasPressedThisFrame) equippedItem = EquippedItem.Seed;
		if (Keyboard.current.digit5Key.wasPressedThisFrame) NextPlant();
		if (Keyboard.current.digit6Key.wasPressedThisFrame) PreviousPlant();

	}

    void NextPlant()
    {
        if (equippedPlant + 1 == plants.Count)
        {
            equippedPlant = 0;
        }
        else equippedPlant += 1;
    }

    void PreviousPlant()
    {
        if (equippedPlant == 0)
        {
            equippedPlant = plants.Count - 1;
        }
        else equippedPlant -= 1;
    }
}
