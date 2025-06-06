using UnityEngine;

public class BunnyBurrowScript : EnemyMain
{
    [Tooltip ("How many seconds between each rabbit spawn")]
    [SerializeField] float spawnDelay;
    [SerializeField] float timeStamp;
    [SerializeField] GameObject BunnyPrefab;

	public override void Start()
	{
        base.Start();
        timeStamp = Time.time;
	}

	// Update is called once per frame
	public override void Update()
    {
        base.Update();
        if(Time.time >= timeStamp + spawnDelay) 
        { 
            Instantiate(BunnyPrefab, transform);
            SpawnerManager.Instance.waves[SpawnerManager.Instance.currentWaveIndex].enemiesSpawned++;
			SpawnerManager.Instance.waves[SpawnerManager.Instance.currentWaveIndex].enemiesLeft++;
            timeStamp = Time.time;
		}
    }
}
