using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Place this script on an empty game object 
// This script deals with wave making and enemy spawing.

public class SpawnerManager : Singleton<SpawnerManager>
{
	[SerializeField] float waveCountdownTimer;
	public Wave[] waves;
	public List<SpawnNode> spawnNodes;
	public bool waveCanSpawn; // bool to allow enemies to spawn.
	private bool _waveReadyToCountDown; // bool to start wave countdown then send the wave.
    private HUDManager _hudManager; // Reference to the HUDManager (Can delete if unneeded)
	public int CurrentWaveIndex; /*{ get; set; }*/ // Use to display what wave is active (will have to add 1 to it)

	protected override void Initialize()
	{
		SceneManager.LoadSceneAsync("GameHUDScene", LoadSceneMode.Additive);
	}

	private void Start()
	{
		CurrentWaveIndex = 0;

		// Sets the robots left in each wave (robotsLeft is used when an enemy dies) 
		for (int i = 0; i < waves.Length; i++)
		{
			waves[i].enemiesLeft = waves[i].Enemies.Length;
		}

		waveCountdownTimer = waves[CurrentWaveIndex].WaveCountDownTime;
		
		_hudManager = FindFirstObjectByType<HUDManager>();
		if (_hudManager == null)
		{
			Debug.LogError("HUDManager not found");
		}
		_hudManager.waveNumberDisplayText.text = $"Wave: {CurrentWaveIndex + 1}";

		SetUpNextWave();
	}

	private void Update()
	{
		//if (CurrentWaveIndex >= waves.Length)
		//{
		//	if (hudManager != null)
		//	{
		//		hudManager.ShowWinPanel();
		//	}
		//	return;
		//}

		// enter key press is temporary until I hook it up to player input.
		if (Keyboard.current.enterKey.wasPressedThisFrame && _waveReadyToCountDown) 
		{
			_waveReadyToCountDown = false;
		}

		if (waveCountdownTimer <= 0)
		{
			waveCanSpawn = true;
			_hudManager.waveCountdownText.text = $"Wave Start!";
		}

		if (!_waveReadyToCountDown && waveCountdownTimer >= 0) 
		{
			waveCountdownTimer -= Time.deltaTime;
			_hudManager.waveCountdownText.text = Mathf.Round(waveCountdownTimer).ToString();
		}

		if (waves[CurrentWaveIndex].enemiesLeft <= 0)
		{
			CurrentWaveIndex++;
			if (CurrentWaveIndex < waves.Length)
			{
				waveCountdownTimer = waves[CurrentWaveIndex].WaveCountDownTime;
				SetUpNextWave();
			}
		}
	}

	private void SetUpNextWave()
	{
		waveCanSpawn = false;
		_waveReadyToCountDown = true;
		int spawnNodeIndexCounter = 0;

		for (int i = 0; i < waves[CurrentWaveIndex].Enemies.Length; i++)
		{
			spawnNodes[spawnNodeIndexCounter].Enemies.Clear();
			spawnNodes[spawnNodeIndexCounter].Enemies.Add(waves[CurrentWaveIndex].Enemies[i]);
			
			spawnNodeIndexCounter++;
			if (spawnNodeIndexCounter >= spawnNodes.Count)
			{
				spawnNodeIndexCounter = 0;
			}
		}

		for (int i = 0; i < spawnNodes.Count; i++)
		{
			spawnNodes[i].TimeBetweenEnemySpawns = waves[CurrentWaveIndex].TimeBetweenEnemySpawns;
		}

		_hudManager.waveCountdownText.text = $"[Enter] to start wave";
	}

	public void SpawnWave(GameObject enemy, Transform spawnNode)
	{
		#region Old Spawn Code
		// Checks if there are more waves left, if the enemy spawn time is off cooldown, and if there are still more robots to be spawned in the wave. 
		//if (CurrentWaveIndex < waves.Length && Time.time > timeStamp + waves[CurrentWaveIndex].TimeBetweenEnemySpawns && waves[CurrentWaveIndex].enemiesSpawned < waves[CurrentWaveIndex].robots.Length)
		//{
		//	if (SpawnPoint != null)
		//	{
		//		RobotScript robot = Instantiate(waves[CurrentWaveIndex].robots[waves[CurrentWaveIndex].robotsSpawned], transform);
		//		robot.transform.SetParent(SpawnPoint.transform);
		//	}

		//	timeStamp = Time.time;
		//	waves[CurrentWaveIndex].enemiesSpawned++;
		//}
		#endregion

		#region New Spawn Code (Gets Called)
		// Time check is commented out because it is now being checked in SpawnNode script but I want to keep it here for now just in case it has to be used later on some how.
		if (CurrentWaveIndex < waves.Length && /*Time.time > timeStamp + waves[CurrentWaveIndex].TimeBetweenEnemySpawns &&*/ waves[CurrentWaveIndex].enemiesSpawned < waves[CurrentWaveIndex].Enemies.Length)
		{
			if (spawnNode != null)
			{
				GameObject instantiatedEnemy = Instantiate(enemy, spawnNode.transform);
				instantiatedEnemy.transform.SetParent(spawnNode.transform);
			}
		}
		#endregion
	}

	
}

[System.Serializable]
public class Wave
{
	/// <summary>
	/// This is the holder of the enemies you want per wave, the delay between each enemy spawn, the time between waves (Customization is all done through the inspector)
	/// </summary>

	[Tooltip("List of enemies you want to spawn in the wave")]
	public GameObject[] Enemies; // Replace with the new enemy script used
	[SerializeField] float _timeBetweenEnemySpawns; // Enemy spawn cooldown
	[SerializeField] float _waveCountdownTime; // 
	[HideInInspector] public int enemiesSpawned = 0;
	[HideInInspector] public int enemiesLeft; // Used when an enemy dies (subract it), in whatever script is used to tell the enemy to die.
	public float TimeBetweenEnemySpawns
	{
		get => _timeBetweenEnemySpawns;
		set => _timeBetweenEnemySpawns = value;
	}
	public float WaveCountDownTime
	{
		get => _waveCountdownTime;
		set => _waveCountdownTime = value;
	}

}

