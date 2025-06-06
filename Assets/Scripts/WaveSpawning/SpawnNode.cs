using System.Collections.Generic;
using UnityEngine;

public class SpawnNode : MonoBehaviour
{
	[Tooltip ("They will get the enemies from the SpawnManager")]
	public List<GameObject> Enemies;
	private float _timeStamp;
	private int _enemiesSpawned; // Can keep track of both the index of Enemeies and how many have been spawned
	public float TimeBetweenEnemySpawns { get; set; } // will have to get after every wave

	private void Awake()
	{
		SpawnerManager.Instance.spawnNodes.Add(this);
	}
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		_enemiesSpawned = 0;
		_timeStamp = Time.time;
	}

	// Update is called once per frame
	void Update()
	{
		if (SpawnerManager.Instance.waveCanSpawn && _enemiesSpawned < Enemies.Count && Time.time > _timeStamp + TimeBetweenEnemySpawns)
		{
			SpawnerManager.Instance.SpawnWave(Enemies[_enemiesSpawned], transform);
			_timeStamp = Time.time;
			_enemiesSpawned++;
		}
	}

	public void Reset()
	{
		_enemiesSpawned = 0;
		Enemies.Clear();
	}
}
