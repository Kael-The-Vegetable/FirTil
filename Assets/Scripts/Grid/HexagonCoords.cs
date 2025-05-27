using System;
using Unity.VisualScripting;
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
    public static HexCoord zero => new HexCoord(0, 0);
	#endregion

    /// <summary>
    /// Starting with north-west going clockwise
    /// </summary>
    /// <returns></returns>
    public readonly HexCoord[] GetNeighbours()
        => new HexCoord[6]
        {
            new(q, r - 1),
            new(q + 1, r - 1),
            new(q + 1, r),
            new(q, r + 1),
            new(q - 1, r + 1),
            new(q - 1, r)
        };

    public readonly int DistanceTo(HexCoord end)
    {
        var v = this - end;
		return Math.Abs(v.q) + Math.Abs(v.r);
    }

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

	public static HexCoord operator -(HexCoord a, HexCoord b) => new(a.q - b.q, a.r - b.r);
	public static HexCoord operator +(HexCoord a, HexCoord b) => new(a.q + b.q, a.r + b.r);
    public static bool operator ==(HexCoord a, HexCoord b)
        => a.q == b.q && a.r == b.r;
    public static bool operator !=(HexCoord a, HexCoord b)
        => !(a == b);
	public override string ToString() => $"Q:{q}, R{r}, S:{s}";
}
