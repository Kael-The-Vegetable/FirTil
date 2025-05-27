using NUnit.Framework.Api;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using static UnityEngine.Rendering.DebugUI.Table;

[RequireComponent (typeof(Tilemap))]
public class PathGenerator : Singleton<PathGenerator>
{
	[SerializeField] private Tilemap _map;
	[SerializeField] private TileBase _pathTile;
	[Space] 
	[SerializeField] private RectInt _spawnBounds;
	private Dictionary<Vector2Int, TileBase> _tiles = new Dictionary<Vector2Int, TileBase>();
	private List<List<Vector2Int>> _paths = new List<List<Vector2Int>>();

	/// <summary>
	/// Starting is north-west going clockwise, 0 even, 1 odd
	/// </summary>
	private static Vector2Int[,] _NEIGHBOURS = new Vector2Int[2, 6]
	{ 
		{new(-1, 1), new(0, 1), new(1, 0), new(0, -1), new(-1, -1), new(-1, 0) }, 
		{new(0, 1), new(1, 1), new(1, 0), new(1, -1), new(0, -1), new(-1, 0)}
	};
	protected override void Initialize()
	{
		if (_map == null) _map = GetComponent<Tilemap>();
#if UNITY_EDITOR
		if (_pathTile == null) Debug.LogError("Need to set a path tile on Path Generator!");
#endif
	}
	private void Update()
	{
		Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
		mouseWorldPos.z = 0;
		Vector3Int cellPos = _map.WorldToCell(mouseWorldPos);
		_map.ClearAllTiles();
		_map.SetTile(cellPos, _pathTile);
		Debug.Log($"Position In Map -> {cellPos}");
		Debug.Log($"Position in Map After Conversions -> {HexCoord.UnityToHex(cellPos).ToUnity()}");
	}

	public void CreatePath(HexCoord startPoint = default)
	{
		if (_map == null || _pathTile == null) return;
		if (startPoint == HexCoord.zero) startPoint = new HexCoord(10, 10);
		var queue = new Queue<HexCoord>();
		queue.Enqueue(startPoint);
		var cameFrom = new Dictionary<HexCoord, HexCoord?> { { startPoint, null } };

	}

	private void OnDrawGizmosSelected()
	{
		var grid = GetComponentInParent<Grid>();
		Gizmos.color = Color.red;
		Gizmos.DrawLine(grid.CellToWorld((Vector3Int)_spawnBounds.position), grid.CellToWorld((Vector3Int)_spawnBounds.position + (Vector3Int)_spawnBounds.size));
	}
}
