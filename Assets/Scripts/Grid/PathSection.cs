using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PathSection : MonoBehaviour
{
	private SpriteRenderer _renderer;
	public HexCoord GridCoordinates { get; private set; }
	private List<PathSection> _connectedNeighbours = new List<PathSection>();
}
