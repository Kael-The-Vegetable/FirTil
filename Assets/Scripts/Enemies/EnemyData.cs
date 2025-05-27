using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public enum EnemyType
    {
        Basic,
        Support,
        Spawner,
        Swarm,
        Boss,
    }

    public EnemyType enemyType = EnemyType.Basic;
    public GameObject EnemyPrefab;

    public float CurrentHealt;
    public float MaxHealth;
    public float MoveSpeed;

}
