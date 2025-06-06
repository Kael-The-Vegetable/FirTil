using UnityEngine;

public class BunnyBurrowScript : EnemyMain
{
    [Tooltip ("How many seconds between each rabbit spawn")]
    [SerializeField] float spawnDelay;
    [SerializeField] float timeStamp;
    [SerializeField] GameObject BunnyPrefab;

	private void Start()
	{
        timeStamp = Time.time;
	}

	// Update is called once per frame
	void Update()
    {
        if(Time.time >= timeStamp + spawnDelay) 
        { 
            Instantiate(BunnyPrefab, transform);
            SpawnerManager.Instance.waves[SpawnerManager.Instance.currentWaveIndex].enemiesSpawned++;
			SpawnerManager.Instance.waves[SpawnerManager.Instance.currentWaveIndex].enemiesLeft++;
            timeStamp = Time.time;
		}
    }
}
