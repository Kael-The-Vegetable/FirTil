using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

[RequireComponent (typeof(Tilemap))]
public class PathGenerator : Singleton<PathGenerator>
{
	[SerializeField] private Tilemap _map;
	[SerializeField] private TileBase _pathTile;
	[Space] 
	[SerializeField] private int _spawnRadius;
	[SerializeField, Range(0, 1000)] private float _perlinModifier;
	[SerializeField, Range(1, 5)] private float _perlinScale = 1;
	private Dictionary<Vector2Int, TileBase> _tiles = new Dictionary<Vector2Int, TileBase>();
	private List<List<Vector2Int>> _paths = new List<List<Vector2Int>>();

	protected override void Initialize()
	{
		if (_map == null) _map = GetComponent<Tilemap>();
#if UNITY_EDITOR
		if (_pathTile == null)
		{
			Debug.LogError("Need to set a path tile on Path Generator!");
			return;
		}
#endif
		HexCoord[] hexs = HexCoord.zero.AllPositionsWithinRange(_spawnRadius);
		for (int i = 0; i < hexs.Length; i++)
		{
			_map.SetTile(hexs[i], _pathTile);
		}
	}
	public void CreatePath(HexCoord startPoint = default)
	{
		if (_map == null || _pathTile == null) return;
		if (startPoint == HexCoord.zero) startPoint = new HexCoord(10, 10);
		var queue = new PriorityQueue<HexCoord, float>();
		queue.Enqueue(startPoint, 0);
		var cameFrom = new Dictionary<HexCoord, HexCoord?> { { startPoint, null } };
		var costHere = new Dictionary<HexCoord, float> { { startPoint, 0 } };
		
		HexCoord goal = HexCoord.zero;

		// A* searching through perlin noise to induce interesting paths.
		while (queue.Count > 0)
		{
			var current = queue.Dequeue();
			if (current == goal) break;

			foreach (var neighbour in CheckForMapNeighbours(current.GetNeighbours()))
			{
				float cost = costHere[current] + PerlinNoiseOfHex(neighbour);
				if (!costHere.ContainsKey(neighbour) || cost < costHere[neighbour])
				{
					if (!costHere.TryAdd(neighbour, cost)) costHere[neighbour] = cost;
					
					float priority = cost + goal.DistanceTo(neighbour);
					queue.Enqueue(neighbour, priority);

					if (!cameFrom.TryAdd(neighbour, current)) cameFrom[neighbour] = current;
				}
			}
		}

		_paths.Add(BackTrackPath(cameFrom, goal));
	}

	private List<Vector2Int> BackTrackPath(Dictionary<HexCoord, HexCoord?> map, HexCoord endPoint)
	{
		var list = new List<Vector2Int>();
		HexCoord? current = endPoint;
		while (current != null)
		{
			list.Add((HexCoord)current);
			current = map[(HexCoord)current];
		}
		return list;
	}

	public float PerlinNoiseOfHex(HexCoord point)
	{ // have 0 x at q = -_spawnRadius and 0 y at r = -spawnRadius
		float t = 1f / _spawnRadius * 2 * _perlinScale;
		return Mathf.PerlinNoise((point.q + _spawnRadius) * t, (point.r + _spawnRadius) * t) * _perlinModifier;
	}
	public HexCoord[] CheckForMapNeighbours(HexCoord[] tileNeighbours)
	{
		List<HexCoord> n = new List<HexCoord>();
		for (int i = 0; i < tileNeighbours.Length; i++)
		{
			if (tileNeighbours[i].DistanceTo(HexCoord.zero) >= _spawnRadius) continue;
			n.Add(tileNeighbours[i]);
		}
		return n.ToArray();
	}

	[ContextMenu("PlaceRandomPath")]
	public void PlaceRandomPath()
	{
		var start = HexCoord.zero.RandomPointAtDistance(_spawnRadius);
		_map.SetTile(start, _pathTile);
		CreatePath(start);
	}
	private void OnDrawGizmosSelected()
	{
		var grid = GetComponentInParent<Grid>();
		Gizmos.color = Color.red;
		HexCoord origin = HexCoord.zero;

		// Drawing Hexagon
		Gizmos.DrawLine(_map.CellToWorld(origin.NewAlongQ(_spawnRadius)), _map.CellToWorld(origin.NewAlongS(_spawnRadius)));
		Gizmos.DrawLine(_map.CellToWorld(origin.NewAlongS(_spawnRadius)), _map.CellToWorld(origin.NewAlongR(_spawnRadius)));
		Gizmos.DrawLine(_map.CellToWorld(origin.NewAlongR(_spawnRadius)), _map.CellToWorld(origin.NewAlongQ(-_spawnRadius)));
		Gizmos.DrawLine(_map.CellToWorld(origin.NewAlongQ(-_spawnRadius)), _map.CellToWorld(origin.NewAlongS(-_spawnRadius)));
		Gizmos.DrawLine(_map.CellToWorld(origin.NewAlongS(-_spawnRadius)), _map.CellToWorld(origin.NewAlongR(-_spawnRadius)));
		Gizmos.DrawLine(_map.CellToWorld(origin.NewAlongR(-_spawnRadius)), _map.CellToWorld(origin.NewAlongQ(_spawnRadius)));

		Gizmos.color = Color.blue;

		for (int i = 0; i < _paths.Count; i++)
		{
			for (int j = 1; j < _paths[i].Count; j++)
			{
				Gizmos.DrawLine(_map.CellToWorld((Vector3Int)_paths[i][j - 1]), _map.CellToWorld((Vector3Int)_paths[i][j]));
			}
		}
	}
}
