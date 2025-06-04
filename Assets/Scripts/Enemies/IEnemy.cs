using UnityEngine;

public interface IEnemy
{
	public enum EnemyState
	{
		Alive,
		Dying,
		Dead
	}

	public EnemyData EnemyData { get; set; }
	public float Health { get; set; }

	//public EnemyState State { get; }
	//public float Toughness { get; } // "armour" 
	//public float MoveSpeed { get; }
	//public float Damage { get; }
	//public float AttackSpeed { get; }
}