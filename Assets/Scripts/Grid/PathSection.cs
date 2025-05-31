using System.Collections.Generic;
using UnityEngine;

public class PathSection : MonoBehaviour
{
	[SerializeField] private SpriteRenderer _renderer;
	public HexCoord GridCoordinates { get; private set; }
	private List<PathSection> _connectedNeighbours = new List<PathSection>();
}
