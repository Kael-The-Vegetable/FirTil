using UnityEngine;

public class EnemyMain : MonoBehaviour, IEnemy
{

	public EnemyData EnemyData { get => enemyData; set => enemyData = value; }
	public EnemyData enemyData;

	public float Health { get => health; set => health = value; }
	public float health;

    public IEnemy.EnemyState currentLivingState = IEnemy.EnemyState.Alive;

	public float toughness, movespeed, damage, attackSpeed;

	private void Awake()
	{
		health = enemyData.health;
		toughness = enemyData.toughness;
		movespeed = enemyData.moveSpeed;
		damage = enemyData.damage;
		attackSpeed = enemyData.attackSpeed;
	}
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
