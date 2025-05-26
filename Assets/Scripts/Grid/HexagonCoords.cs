using UnityEngine;

public struct HexCoord
{
    public int q;
    public int r;
    public int s => -q - r;

	#region Constructors
	public HexCoord(int q, int r)
    {
        this.q = q;
        this.r = r;
    }

    /// <summary>
    /// Assuming x is q and y is r
    /// </summary>
    /// <param name="v"></param>
    public HexCoord(Vector2Int v)
    {
        q = v.x; 
        r = v.y;
	}
	#endregion

	public static HexCoord UnityToHex(Vector2Int v)
    {
        v.y *= -1; // because unity is stupid
        return new(v.x - (v.y - (v.y & 1)) / 2, v.y);
    }
    public static HexCoord UnityToHex(Vector3Int v3) => UnityToHex((Vector2Int)v3);

    public readonly Vector2Int ToUnity()
    {
        var col = q + (r - (r & 1)) / 2;
        var row = r;
        return new(col, -row); // negative because unity is stupid
	}

    public override string ToString() => $"Q:{q}, R{r}, S:{s}";
}
