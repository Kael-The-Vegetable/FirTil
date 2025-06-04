using System.Collections.Generic;
using UnityEngine;

public class PathSection : MonoBehaviour
{
	[SerializeField] private SpriteRenderer _renderer;
	[SerializeField] private PathSprites _sprites;
	[field: SerializeField] public HexCoord GridCoordinates { get; set; }
	public List<PathSection> connectedNeighbours = new List<PathSection>();

	[field: SerializeField] public bool IsOccupied { get; set; } = false;

	[ContextMenu("DisplayProperState")]
	public void DisplayProperState()
	{
		Sprite p = _sprites.GetProperSprite(this, connectedNeighbours);
		if (p != null) _renderer.sprite = p;
	}
}
