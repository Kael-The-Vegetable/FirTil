using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject, IEnemy
{
	public enum EnemyType
	{
		Basic,    // Nothing special.
		Support,  // Heal / Buff allies or debuff plants. (Ex. Turtle / Tortise to provide a defensive buff in an area)
		Spawner,  // Constantly spawn enemies if not killed / destroyed. (Ex. Bunny burrow)
		Swarm,    // Group of enemies that will split on low hp. (Ex. Locusts or bees)
		Flyer,    // Not constricted to the path and will try to take out plants / seeds.
		Burrower, // Will stay underground for a while before popping out for a few seconds to look around then go back under.
		Hurdler,  // Jump over obstacles.
		Tiny,     // Can bypass lane obstacles with little to no damage taken, but are very easy to kill.
		Charger,  // Periodically increases speed and charges, dealing damage to any lane obstacles in it's path.
		Boss,
	}

	public EnemyType enemyType;

	public string EnemyName;
	public GameObject EnemyPrefab;

	[SerializeField] IEnemy.EnemyState _enemyState;
	[SerializeField] float _health;
	[SerializeField] float _toughness;
	[SerializeField] float _moveSpeed;
	[SerializeField] float _damage;
	[SerializeField] float _attackSpeed;
	[SerializeField] int _difficultyRating;

	public IEnemy.EnemyState State { get => _enemyState; }
	public float Health { get => _health; }
	public float Toughness { get => _toughness; } // "armour" 
	public float MoveSpeed { get => _moveSpeed; }
	public float Damage { get => _damage; }
	public float AttackSpeed { get => _attackSpeed; }
	public float DifficultyRating { get => _difficultyRating; }
}
