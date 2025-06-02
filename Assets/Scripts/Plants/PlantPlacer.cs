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
            Collider2D plotHit = Physics2D.OverlapPoint(TileDetector.Instance.GetCellPosVector2(), plotMask);
            Collider2D plantHit = Physics2D.OverlapPoint(TileDetector.Instance.GetCellPosVector2(), plotMask);
			if (plantHit != null || plotHit != null)
            {
				switch (equippedItem)
				{
					case EquippedItem.Shovel:
                        if (plantHit != null && plotHit != null)
                        {
                            Debug.Log("Dig Up Plant");
							plotHit.gameObject.GetComponent<TilePlot>().Dig();
						}
                        else if (plotHit != null)
                        {
                            if (plotHit.gameObject.GetComponent<TilePlot>().IsAlreadyTilled())
                            {
								Debug.Log("Plant");
								plotHit.gameObject.GetComponent<TilePlot>().PlaceNewPlant(plants[equippedPlant]);
                            }
                            else
                            {
								Debug.Log("Tile");
								plotHit.gameObject.GetComponent<TilePlot>().Dig();
							}
                        }
							

						break;
					case EquippedItem.WateringCan:
						if (plantHit != null)
						{
							plantHit.gameObject.GetComponent<IPlant>().WaterPlant();
						}
						break;
					case EquippedItem.Fertilizer:
						if (plantHit != null)
						{
							plantHit.gameObject.GetComponent<IPlant>().AccelerateGrowth(1.5f, 10);
						}
						break;

				}
			}

			
        }

        // Switch equipped item
        if (Keyboard.current.digit1Key.wasPressedThisFrame) equippedItem = EquippedItem.Shovel;
        if (Keyboard.current.digit2Key.wasPressedThisFrame) equippedItem = EquippedItem.WateringCan;
        if (Keyboard.current.digit3Key.wasPressedThisFrame) equippedItem = EquippedItem.Fertilizer;
		if (Keyboard.current.digit4Key.wasPressedThisFrame) PreviousPlant();
		if (Keyboard.current.digit5Key.wasPressedThisFrame) NextPlant();
		

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
