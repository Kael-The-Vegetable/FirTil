using UnityEngine;

public interface IEnemy
{
	public enum EnemyState
	{
		Alive,
		Dying,
		Dead
	}

	public EnemyState State { get; }
	public float Health { get; }
	public float Toughness { get; } // "armour" 
	public float MoveSpeed { get; }
	public float Damage { get; }
	public float AttackSpeed { get; }
}
