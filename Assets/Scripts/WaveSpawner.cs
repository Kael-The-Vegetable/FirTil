using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Place this script on an empty game object 
// This script deals with wave making and enemy spawing.

public class WaveSpawner : MonoBehaviour
{
	[SerializeField] float waveCountdownTimer;
	public Wave[] waves;
	//private UIManager uiManager; // Reference to the UIManager (Can delete if unneeded)
	private float timeStamp; // Used for enemy spawn timing.
	private bool _readyToCountdownNextWave; // bool to check if the next wave is able to be sent / counted down.
	public int CurrentWaveIndex { get; set; } // Use to display what wave is active (will have to add 1 to it)
	public GameObject SpawnPoint { get; set; }
	


	private void Start()
	{
		_readyToCountdownNextWave = true;
		CurrentWaveIndex = 0;

		// Sets the robots left in each wave (robotsLeft is used when an enemy dies) 
		for (int i = 0; i < waves.Length; i++)
		{
			//waves[i].robotsLeft = waves[i].robots.Length;
		}

		waveCountdownTimer = waves[CurrentWaveIndex].WaveCountDownTime;

		//uiManager = FindObjectOfType<UIManager>();
		//if (uiManager == null)
		//{
		//    Debug.LogError("UIManager not found in the scene!");
		//}
	}

	private void Update()
	{
		if (CurrentWaveIndex >= waves.Length)
		{
			//if (uiManager != null/* && AllEnemiesDeadCheck()*/)
			//{
			//    uiManager.ShowWinPanel();
			//}

			return;
		}

		if (_readyToCountdownNextWave)
		{
			waveCountdownTimer -= Time.deltaTime;
		}

		if (waveCountdownTimer <= 0)
		{
			_readyToCountdownNextWave = false;
			SpawnWave();
		}

		if (waves[CurrentWaveIndex].robotsLeft <= 0)
		{
			_readyToCountdownNextWave = true;
			//CurrentWaveIndex++;

			/// auto spawn waves when all enemies are defeated from previous wave
			//if (CurrentWaveIndex < waves.Length)
			//{
			//	waveCountdownTimer = waves[CurrentWaveIndex].WaveCountDownTime;
			//}
		}
	}

	public void SpawnWave()
	{
		// Checks if there are more waves left, if the enemy spawn time is off cooldown, and if there are still more robots to be spawned in the wave. 
		//if (CurrentWaveIndex < waves.Length && Time.time > timeStamp + waves[CurrentWaveIndex].TimeBetweenRobotSpawns && waves[CurrentWaveIndex].robotsSpawned < waves[CurrentWaveIndex].robots.Length)
		//{
		//	if (SpawnPoint != null)
		//	{
		//		RobotScript robot = Instantiate(waves[CurrentWaveIndex].robots[waves[CurrentWaveIndex].robotsSpawned], transform);
		//		robot.transform.SetParent(SpawnPoint.transform);
		//	}

		//	timeStamp = Time.time;
		//	waves[CurrentWaveIndex].robotsSpawned++;
		//}
	}
}

[System.Serializable]
public class Wave
{
	/// <summary>
	/// This is the holder of the enemies you want per wave, the delay between each enemy spawn, the time between waves (Customization is all done through the inspector)
	/// </summary>

	[Tooltip("List of enemies you want to spawn in the wave")]
	//public RobotScript[] Robots { get; set; } // Replace with the new enemy script used
	[SerializeField] float _timeBetweenEnemySpawns; // Enemy spawn cooldown
	[SerializeField] float _waveCountdownTime; // 
	[HideInInspector] public int robotsSpawned = 0; 
	[HideInInspector] public int robotsLeft; // Used when an enemy dies (subract it), in whatever script is used to tell the enemy to die.
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

