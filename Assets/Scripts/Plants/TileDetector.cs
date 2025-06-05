
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDetector : MonoBehaviour
{
    public static TileDetector Instance;

    public Tilemap Ground;
    public Transform player;


	private void Awake()
	{
		Instance = this;
	}
    // Check if the player is on a valid tile to place a plant
    public bool OnValidPlaceableTile()
    {
		Vector3Int currentTilePos = Ground.WorldToCell(player.position);
		TileBase tile = Ground.GetTile(currentTilePos);
		Tile currentTile = (Tile)tile;
		if (currentTile != null && currentTile.gameObject && currentTile.gameObject.tag == "Plot")
        {
            Debug.Log("Valid Tile");
            return true;
        }
        else
        {
			Debug.Log("Not Valid Tile");
			return false;
        }
    }

    // Get the center of the current tile the player is on
    public Vector2 GetCellPosVector2()
    {
		Vector3Int currentTilePos = Ground.WorldToCell(player.position);
		Vector3 pos = Ground.CellToWorld(currentTilePos);
        return new Vector2(pos.x, pos.y);
    }
}
