using UnityEngine;

[RequireComponent (typeof(Grid))]
public class GridRatio : Singleton<GridRatio>
{
	public static readonly Vector2 BASE_HEX_SIZE = new(0.866025403784f, 1);
	private Grid _grid;
	[field: SerializeField] public Vector2 MovementScalar { get; private set; } = Vector2.zero;
	private enum BaseSide { X, Y, Max, Min }
	[SerializeField] private BaseSide _baseSide;
	protected override void Initialize()
	{
		_grid = GetComponent<Grid>();
		Vector2 ratio = _grid.cellSize / BASE_HEX_SIZE;
		float baseSide = _baseSide switch
		{
			BaseSide.X => ratio.y,
			BaseSide.Y => ratio.x,
			BaseSide.Max => (ratio.y > ratio.x ? ratio.y : ratio.x),
			BaseSide.Min => (ratio.y < ratio.x ? ratio.y : ratio.x),
			_ => 1
		};

		MovementScalar = (_grid.cellSwizzle == GridLayout.CellSwizzle.XYZ ? new Vector2(ratio.x, ratio.y) : new Vector2(ratio.y, ratio.x)) / baseSide;
	}
}
