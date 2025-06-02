using System.Collections.Generic;
using UnityEngine;

public class PathSection : MonoBehaviour
{
	[SerializeField] private SpriteRenderer _renderer;
	[SerializeField] private PathSprites _sprites;
	[field: SerializeField] public HexCoord GridCoordinates { get; set; }
	public List<PathSection> connectedNeighbours = new List<PathSection>();

	[ContextMenu("DisplayProperState")]
	public void DisplayProperState()
	{
		Sprite p = _sprites.GetProperSprite(this, connectedNeighbours);
		Debug.Log(p);
		if (p != null) _renderer.sprite = p;
	}
}
