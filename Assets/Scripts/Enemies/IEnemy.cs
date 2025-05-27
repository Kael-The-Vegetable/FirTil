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
	public float MaxHealth { get; set; }
	public float CurrentHealth { get; set; }
}
