using System.Collections.Generic;
using UnityEngine;

public class PathSection : MonoBehaviour
{
	[SerializeField] private SpriteRenderer _renderer;
	[SerializeField] private PathSprites _sprites;
	public HexCoord GridCoordinates { get; set; }
	public List<PathSection> connectedNeighbours = new List<PathSection>();
	[Space]
	[SerializeField] private GameObject _pathPlot;
	[SerializeField] private GameObject _grassPlot;

	public GameObject ActivePlot => _grassPlot.activeInHierarchy ? _grassPlot : _pathPlot;

	[field: SerializeField] public bool IsOccupied { get; set; } = false;

	[ContextMenu("DisplayProperState")]
	public void DisplayProperState()
	{
		CheckIfPath();
		Sprite p = _sprites.GetProperSprite(this, connectedNeighbours);
		if (p != null) _renderer.sprite = p;
	}

	private void CheckIfPath()
	{
		bool state = connectedNeighbours.Count > 0;
		_pathPlot.SetActive(state);
		_grassPlot.SetActive(!state);
	}
}
