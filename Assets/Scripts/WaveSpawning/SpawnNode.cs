using System.Collections.Generic;
using UnityEngine;

public class SpawnNode : MonoBehaviour
{
	public List<GameObject> Enemies;
	private float _timeStamp;
	private int _enemiesSpawned; // Can keep track of both the index of Enemeies and how many have been spawned
	private SpawnerManager _spawnerManager;

	private void Awake()
	{
		_spawnerManager = GameObject.Find("SpawnerManager").GetComponent<SpawnerManager>();
		_spawnerManager.spawnNodes.Add(this);
	}
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		_enemiesSpawned = 0;
	}

	// Update is called once per frame
	void Update()
	{
		if (_enemiesSpawned < Enemies.Count)
		{
			_spawnerManager.SpawnWave(Enemies[_enemiesSpawned], transform, _timeStamp);
			_timeStamp = Time.time;
			_enemiesSpawned++;
		}
	}
}
