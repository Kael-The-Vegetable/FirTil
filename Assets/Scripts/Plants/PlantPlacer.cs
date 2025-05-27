using UnityEngine;
using UnityEngine.InputSystem;

public class PlantPlacer : MonoBehaviour
{
    enum EquippedItem
    {
        Shovel,
        Item1,
        Item2,
        Item3
    }
    [SerializeField] EquippedItem equippedItem = EquippedItem.Shovel;

    [SerializeField] PlantData Item1, Item2, Item3;

    [SerializeField] LayerMask plotMask;
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
						Physics2D.OverlapPoint(TileDetector.Instance.GetCellPosVector2(), plotMask).gameObject.GetComponent<TilePlot>().DigUpPlant();

					}
                    
                    break;
                case EquippedItem.Item1:
					if (Physics2D.OverlapPoint(TileDetector.Instance.GetCellPosVector2(), plotMask))
					{
                        Debug.Log("Placed Plant");
						Physics2D.OverlapPoint(TileDetector.Instance.GetCellPosVector2(), plotMask).gameObject.GetComponent<TilePlot>().PlaceNewPlant(Item1);

					}
					break;
                case EquippedItem.Item2:
					if (Physics2D.OverlapPoint(TileDetector.Instance.GetCellPosVector2(), plotMask))
					{
						Debug.Log("Placed Plant");
						Physics2D.OverlapPoint(TileDetector.Instance.GetCellPosVector2(), plotMask).gameObject.GetComponent<TilePlot>().PlaceNewPlant(Item2);

					}
					break;
                case EquippedItem.Item3:
					if (Physics2D.OverlapPoint(TileDetector.Instance.GetCellPosVector2(), plotMask))
					{
						Debug.Log("Placed Plant");
						Physics2D.OverlapPoint(TileDetector.Instance.GetCellPosVector2(), plotMask).gameObject.GetComponent<TilePlot>().PlaceNewPlant(Item3);

					}
					break;
            }
        }

        if (Keyboard.current.digit1Key.wasPressedThisFrame) equippedItem = EquippedItem.Shovel;
		if (Keyboard.current.digit2Key.wasPressedThisFrame) equippedItem = EquippedItem.Item1;
		if (Keyboard.current.digit3Key.wasPressedThisFrame) equippedItem = EquippedItem.Item2;
		if (Keyboard.current.digit4Key.wasPressedThisFrame) equippedItem = EquippedItem.Item3;
	}
}
