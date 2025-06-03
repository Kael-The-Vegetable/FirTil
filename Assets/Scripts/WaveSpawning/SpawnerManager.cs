using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Place this script on an empty game object 
// This script deals with wave making and enemy spawing.

public class SpawnerManager : Singleton<SpawnerManager>
{
	[SerializeField] float waveCountdownTimer;
	public List<Wave> waves;
	public List<SpawnNode> spawnNodes;
	public bool waveCanSpawn; // bool to allow enemies to spawn.
	private bool _waveReadyToCountDown; // bool to start wave countdown then send the wave.
	private HUDManager _hudManager; // Reference to the HUDManager 
	public int currentWaveIndex; /*{ get; set; }*/ // Use to display wave (add 1 to it) & scale difficulty (will need to add 1 for proper scaling)
	public int currentDay;
	public int customDifficultyScale;
	public EnemyLibrary enemyLibrary;
	private int wavesPerDay;

	protected override void Initialize()
	{
		SceneManager.LoadSceneAsync("GameHUDScene", LoadSceneMode.Additive);
	}

	private void Start()
	{
		wavesPerDay = 15;

		BuildAllWaves();

		currentWaveIndex = 0;

		// Sets the robots left in each wave (robotsLeft is used when an enemy dies) 
		for (int i = 0; i < waves.Count; i++)
		{
			waves[i].enemiesLeft = waves[i].Enemies.Count;
		}

		waveCountdownTimer = waves[currentWaveIndex].WaveCountDownTime;

		_hudManager = FindFirstObjectByType<HUDManager>();

		if (_hudManager == null)
		{
			Debug.LogError("HUDManager not found");
		}
		//_hudManager.waveNumberDisplayText.text = $"Wave: {currentWaveIndex + 1}";
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
			//_hudManager.waveCountdownText.text = $"Wave Start!";
		}

		if (!_waveReadyToCountDown && waveCountdownTimer >= 0)
		{
			waveCountdownTimer -= Time.deltaTime;
			//_hudManager.waveCountdownText.text = Mathf.Round(waveCountdownTimer).ToString();
		}

		if (waves[currentWaveIndex].enemiesLeft <= 0)
		{
			currentWaveIndex++;
			if (currentWaveIndex < waves.Count)
			{
				waveCountdownTimer = waves[currentWaveIndex].WaveCountDownTime;
				SetUpNextWave();
			}
		}
	}

	private void BuildAllWaves()
	{
		float limitedDifficultyRating;
		List<EnemyData> tempEnenmylist = new();
		waves.Clear();

		for (int i = 0; i < wavesPerDay; i++)
		{
			tempEnenmylist.Clear();
			waves.Add(new Wave());

			// Max wave difficulty rating = base wave difficulty * current day * (1 + half of current wave #) * custom difficulty scale
			waves[i].waveDifficultyRating = waves[i].baseWaveDifficulty * currentDay * (1 + ((i + 1) / 2)) * customDifficultyScale;

			// Can limit the Difficulty rating by multiplying it by a decimaled percentage to lower the amount of enemies (ex. 70% => 0.7)
			limitedDifficultyRating = waves[i].waveDifficultyRating * 0.5f;

			// Use the limited difficulty rating as a condition to not add any enemey that is above the rating threshold. It's the check to see if the constructed wave's total enenmy DR (difficulty rating) is equal or greater than the wave's limited DR 
			waves[i].Enemies.Clear();
			
			for (int j = 0; j < enemyLibrary.Enemies.Count; j++)
			{
				if (enemyLibrary.Enemies[j].DifficultyRating < limitedDifficultyRating)
				{
					tempEnenmylist.Add(enemyLibrary.Enemies[j]);
				}
			}

			float totalWaveDR = 0;
			do
			{
				int rnd = Random.Range(0, tempEnenmylist.Count);
				waves[i].Enemies.Add(tempEnenmylist[rnd].EnemyPrefab);
				totalWaveDR += tempEnenmylist[rnd].DifficultyRating;
			}while (totalWaveDR < limitedDifficultyRating);
		}

		SetUpNextWave();
	}

	private void SetUpNextWave()
	{
		waveCanSpawn = false;
		_waveReadyToCountDown = true;
		int spawnNodeIndexCounter = 0;

		for (int i = 0; i < spawnNodes.Count; i++)
		{
			spawnNodes[i].Enemies.Clear();
		}

		for (int i = 0; i < waves[currentWaveIndex].Enemies.Count; i++)
		{
			spawnNodes[spawnNodeIndexCounter].Enemies.Add(waves[currentWaveIndex].Enemies[i]);

			spawnNodeIndexCounter++;
			if (spawnNodeIndexCounter >= spawnNodes.Count)
			{
				spawnNodeIndexCounter = 0;
			}
		}

		for (int i = 0; i < spawnNodes.Count; i++)
		{
			spawnNodes[i].TimeBetweenEnemySpawns = waves[currentWaveIndex].TimeBetweenEnemySpawns;
		}

		//_hudManager.waveCountdownText.text = $"[Enter] to start wave";
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
		if (currentWaveIndex < waves.Count && /*Time.time > timeStamp + waves[CurrentWaveIndex].TimeBetweenEnemySpawns &&*/ waves[currentWaveIndex].enemiesSpawned < waves[currentWaveIndex].Enemies.Count)
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
	public List<GameObject> Enemies = new(); // Replace with the new enemy script used
	[SerializeField] float _timeBetweenEnemySpawns = 2; // Enemy spawn cooldown
	[SerializeField] float _waveCountdownTime = 3;
	public int baseWaveDifficulty = 5;
	[HideInInspector] public float waveDifficultyRating; // Used as the check for adding enemies to the enenmy list 
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

