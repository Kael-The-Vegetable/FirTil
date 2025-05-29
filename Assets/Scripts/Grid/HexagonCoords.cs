using System;
using UnityEngine;
using Random = UnityEngine.Random;
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
        var v = end - this;
        return (Math.Abs(v.q) + Math.Abs(v.r) + Math.Abs(v.s)) / 2;
    }
    public readonly int Magnitude() => (Math.Abs(q) + Math.Abs(r) + Math.Abs(s)) / 2;

	#region Modification Methods
	/// <summary>
	/// Positive is "top-left" (negative r)
	/// </summary>
	/// <param name="n"></param>
	public void MoveAlongQ(int n) => r -= n;
    /// <inheritdoc cref="MoveAlongQ(int)"/>
	public HexCoord NewAlongQ(int n)
    {
        var h = Copy();
        h.r -= n;
        return h;
    }

	/// <summary>
	/// Positive is "right" (positive q)
	/// </summary>
	/// <param name="n"></param>
	public void MoveAlongR(int n) => q += n;
	/// <inheritdoc cref="MovedAlongR(int)"/>
	public HexCoord NewAlongR(int n)
	{
		var h = Copy();
		h.q += n;
		return h;
	}

	/// <summary>
	/// Positive is "top-right" (positive q & negative r)
	/// </summary>
	/// <param name="n"></param>
	public void MoveAlongS(int n) { q += n; r -= n; }
	/// <inheritdoc cref="MovedAlongS(int)"/>
	public HexCoord NewAlongS(int n)
	{
		var h = Copy();
		h.r -= n;
        h.q += n;
		return h;
	}
	#endregion

	public HexCoord Copy() => new(q, r);

    public HexCoord RandomPointAtDistance(int distance)
    {
        HexCoord h = new HexCoord();
        int hexID = Random.Range(0, distance - 1);
        int section = Random.Range(0, 6);
        int sign = section % 2 == 0 ? 1 : -1;
        switch(section % 3)
        {
            case 0: // top or bottom
                h = NewAlongQ(sign * distance);
                h.MoveAlongR(sign * hexID);
                break;
            case 1:
                h = NewAlongR(sign * distance);
                h.MoveAlongS(-sign * hexID);
                break;
            case 2:
                h = NewAlongS(sign * distance);
                h.MoveAlongQ(-sign * hexID);
                break;
        }
        return h;
    }

	#region Conversions
	public static HexCoord UnityToHex(Vector2Int v)
    {
        v.y *= -1; // because unity is stupid
        return new(v.x - (v.y - (v.y & 1)) / 2, v.y);
    }
    public static HexCoord UnityToHex(Vector3Int v3) => UnityToHex((Vector2Int)v3);

    public readonly Vector2Int ToUnity()
    {
        var col = q + (r - (r & 1)) / 2; // is q + the result of => r - (1 if odd or 0 if even to ensure it is even because we are going to odd-r) then divided by 2 because the column is technically 2 columns side by side.
        var row = r;
        return new(col, -row); // negative because unity is stupid
	}
	#endregion

	#region Operators
	public static HexCoord operator -(HexCoord a, HexCoord b) => new(a.q - b.q, a.r - b.r);
	public static HexCoord operator +(HexCoord a, HexCoord b) => new(a.q + b.q, a.r + b.r);
    public static bool operator ==(HexCoord a, HexCoord b) => a.q == b.q && a.r == b.r;
    public static bool operator !=(HexCoord a, HexCoord b) => !(a == b);
    public static implicit operator Vector3Int(HexCoord a) => (Vector3Int)a.ToUnity();
    public static implicit operator Vector2Int(HexCoord a) => a.ToUnity();
	#endregion

	#region Overrides
	public override string ToString() => $"Q:{q}, R{r}, S:{s}";
	public override readonly bool Equals(object obj)
	{
		return base.Equals(obj);
	}
	public override readonly int GetHashCode()
	{
		return HashCode.Combine(q, r);
	}
	#endregion
}
