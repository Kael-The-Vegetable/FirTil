using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

// Place this script on an empty game object 
// This script deals with wave making and enemy spawing.

public class SpawnerManager : Singleton<SpawnerManager>
{
	[Tooltip ("Time between waves")]
	[SerializeField] float gracePeriodDuration;
	[SerializeField] GameObject spawnNodePrefab;
	private float gracePeriod;
	public List<Wave> waves;
	public List<SpawnNode> spawnNodes;
	public bool waveCanSpawn; // bool to allow enemies to spawn.
	private bool _inGracePeriod; // bool to countdown the grace period then send the wave.
	public int currentWaveIndex; // Use to display wave (add 1 to it) & scale difficulty (will need to add 1 for proper scaling)
	public int currentDay;
	public int customDifficultyScale;
	public EnemyLibrary enemyLibrary;
	private int wavesPerDay;

	public List<List<Vector2Int>> paths = new();
	public Tilemap pathMap;

	protected override void Initialize()
	{
		GameManager.Instance.LoadScene("GameHUDScene");
	}

	private void Start()
	{
		#region Temp
		pathMap = PathGenerator.instance.GetPathTileMap();
		CreateNewPath();
		#endregion

		wavesPerDay = 15;

		BuildAllWaves();

		currentWaveIndex = 0;

		// Sets the robots left in each wave (robotsLeft is used when an enemy dies) 
		for (int i = 0; i < waves.Count; i++)
		{
			waves[i].enemiesLeft = waves[i].Enemies.Count;
		}

		gracePeriod = gracePeriodDuration;

	}

	private void Update()
	{
		if (!HUDManager.HasInstance) return;

		if (currentWaveIndex > waves.Count)
		{
			HUDManager.Instance.gracePeriodTimeText.text = $"Waves Complete!";
			currentDay++;
			// Go to Shop
			return;
		}

			if (_inGracePeriod)
		{
			_inGracePeriod = false;
		}

		if (gracePeriod <= 0)
		{
			waveCanSpawn = true;
			HUDManager.Instance.gracePeriodTimeText.text = $"Wave Start!";
		}

		if (!_inGracePeriod && gracePeriod >= 0)
		{
			gracePeriod -= Time.deltaTime;
			HUDManager.Instance.gracePeriodTimeText.text = Mathf.Round(gracePeriod).ToString();
		}

		if (waves[currentWaveIndex].enemiesLeft <= 0)
		{
			currentWaveIndex++;
			if (currentWaveIndex < waves.Count)
			{
				gracePeriod = gracePeriodDuration;
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
				if (enemyLibrary.Enemies[j].difficultyRating < limitedDifficultyRating)
				{
					tempEnenmylist.Add(enemyLibrary.Enemies[j]);
				}
			}

			float totalWaveDR = 0;
			do
			{
				int rnd = Random.Range(0, tempEnenmylist.Count);
				waves[i].Enemies.Add(tempEnenmylist[rnd].EnemyPrefab);
				totalWaveDR += tempEnenmylist[rnd].difficultyRating;
			}
			while (totalWaveDR < limitedDifficultyRating);
		}

		SetUpNextWave();
	}

	private void SetUpNextWave()
	{
		if (HUDManager.HasInstance) HUDManager.Instance.waveNumberDisplayText.text = $"Wave: {currentWaveIndex + 1}";

		if ((currentDay == 1 && currentWaveIndex == 0) || (currentWaveIndex + 1) % 5 == 0)
		{
			PathGenerator.Instance.PlaceRandomPath();
		}

		waveCanSpawn = false;
		_inGracePeriod = true;
		int spawnNodeIndexCounter = 0;
		waves[currentWaveIndex].enemiesSpawned = 0;

		for (int i = 0; i < spawnNodes.Count; i++)
		{
			spawnNodes[i].Reset();
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

		if (HUDManager.HasInstance) HUDManager.Instance.gracePeriodTimeText.text = $"[Enter] to start wave";
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
		if (currentWaveIndex < waves.Count && waves[currentWaveIndex].enemiesSpawned < waves[currentWaveIndex].Enemies.Count)
		{
			if (spawnNode != null)
			{
				GameObject instantiatedEnemy = Instantiate(enemy, spawnNode.transform);
				instantiatedEnemy.transform.SetParent(spawnNode.transform);

				waves[currentWaveIndex].enemiesSpawned++;

			}
		}
		#endregion
	}

	public List<Vector2Int> GetClosestPath(Transform EnemyTransform)
	{
		float minDistance = 1000;
		List<Vector2Int> closestPath = new List<Vector2Int>();

		foreach (List<Vector2Int> path in paths)
		{
			if (path == null) continue;

			float distance = Vector2.Distance(EnemyTransform.position, path[0]);
			if (distance < minDistance)
			{
				minDistance = distance;
				closestPath = path;
			}
		}

		return closestPath;
	}

	void CreateNewPath()
	{
		PathGenerator.instance.PlaceRandomPath();
		List<Vector2Int> newPath = PathGenerator.instance.GetLastPath();
		newPath.Reverse();
		Instantiate(spawnNodePrefab, pathMap.CellToWorld((Vector3Int)newPath[0]), Quaternion.identity);
		paths = PathGenerator.instance.GetPaths();
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
	public int baseWaveDifficulty = 5;
	[HideInInspector] public float waveDifficultyRating; // Used as the check for adding enemies to the enenmy list 
	[HideInInspector] public int enemiesSpawned = 0;
	[HideInInspector] public int enemiesLeft; // Used when an enemy dies (subract it), in whatever script is used to tell the enemy to die.

	public float TimeBetweenEnemySpawns
	{
		get => _timeBetweenEnemySpawns;
		set => _timeBetweenEnemySpawns = value;
	}
	[SerializeField] float _timeBetweenEnemySpawns = 2; // Enemy spawn cooldown

	//public float WaveCountDownTime
	//{
	//	get => _waveCountdownTime;
	//	set => _waveCountdownTime = value;
	//}
	//[SerializeField] float _waveCountdownTime = 3;
}

