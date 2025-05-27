
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDetector : MonoBehaviour
{
    public static TileDetector Instance;

    public Tilemap Ground;
    public Tilemap Placement;
    public Transform player;
    public Tile currentTile;
    public string PrefabName;
    public Vector3Int currentTilePos;

	private void Awake()
	{
		Instance = this;
	}
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTilePos = Ground.WorldToCell(player.position);
        TileBase tile = Ground.GetTile(currentTilePos);
        currentTile = (Tile)tile;

        if (currentTile != null && currentTile.gameObject)
        {
            PrefabName = currentTile.gameObject.name;
        }
        else PrefabName = "None";

        //Tile tiles;
        //tiles.gameObject.tag
    }

    public bool OnValidPlaceableTile()
    {
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
    public Vector2 GetCellPosVector2()
    {
        Vector3 pos = Ground.CellToWorld(currentTilePos);
        return new Vector2(pos.x, pos.y);
    }
}
