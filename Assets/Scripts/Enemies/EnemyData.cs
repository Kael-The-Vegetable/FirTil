using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public enum EnemyType
    {
        Basic,    // Nothing special
        Support,  // Heal / Buff allies or debuff plants (Ex. Turtle / Tortise to provide a defensive buff in an area)
        Spawner,  // Constantly spawn enemies if not killed / destroyed (Ex. Bunny burrow)
        Swarm,    // Group of enemies that will split on low hp (Ex. Locusts or bees)
        Flyer,    // Not constricted to the path and will try to take out plants / seeds
        Burrower, // Will stay underground for a while before popping out for a few seconds to look around then go back under
        Hurdler,  // Jump over obstacles
        Tiny,     // Can bypass lane obstacles with little to no damage taken, but are very easy to kill.
		Boss,
    }

    public EnemyType enemyType = EnemyType.Basic;

    public GameObject EnemyPrefab;

    public float CurrentHealt;
    public float MaxHealth;
    public float Toughness; // "armour" 
    public float MoveSpeed;
    public float Damage;
    public float AttackSpeed;
}
