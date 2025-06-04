using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Collections;

public class EnemyMain : MonoBehaviour, IEnemy, IDamagable
{
	public EnemyData EnemyData { get => enemyData; set => enemyData = value; }
	public EnemyData enemyData;

	public float Health { get => health; set => health = value; }
	public float health;

	public IEnemy.EnemyState currentLivingState = IEnemy.EnemyState.Alive;

	public float toughness, movespeed, damage, attackSpeed;
	private float CurrentSpeed
	{
		get
		{
			return movespeed * tetherMult;
		}
	}
	float tetherMult = 1;

	float currentHealth;
	private Rigidbody2D rb;
	private Vector2 moveDir = Vector2.zero;
	private IEnemy.EnemyState currentState;
	bool isStopped = false;

	[Header("Pathing")]
	[SerializeField] List<Vector2Int> recievedPath;
	[SerializeField] List<Vector2> actualPath;
	[SerializeField] int currentNode;
	[SerializeField] float DistanceBeforeSwitch = 1;
	[SerializeField] LayerMask blockageMask;
	[SerializeField] Tilemap pathMap;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();

		health = enemyData.health;
		toughness = enemyData.toughness;
		movespeed = enemyData.moveSpeed;
		damage = enemyData.damage;
		attackSpeed = enemyData.attackSpeed;
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		foreach (Vector2Int node in recievedPath)
		{
			Vector2 nodePos = pathMap.GetCellCenterWorld((Vector3Int)node);
			actualPath.Add(nodePos);
		}
    }

    // Update is called once per frame
    void Update()
    {
        if (currentNode <= actualPath.Count - 1)
		{
			if (!isStopped)
			{
				Vector2 movementScalar = GridRatio.Instance.MovementScalar;
				moveDir = (actualPath[currentNode] - (Vector2)transform.position).normalized;

				transform.Translate(moveDir * CurrentSpeed * movementScalar * Time.deltaTime);
			}
			

			if (Vector2.Distance((Vector2)transform.position, actualPath[currentNode]) <= DistanceBeforeSwitch)
			{
				NextNode();
			}
		}
    }

	void NextNode()
	{
		int nextNode = currentNode + 1;
		if (nextNode <= actualPath.Count - 1)
		{
			if (PathGenerator.Instance.GetPathSectionFromGridPosition(recievedPath[nextNode]).IsOccupied)
			{
				// Stop moving
				isStopped = true;
			}
			else
			{
				// Keep moving
				isStopped = false;
				PathGenerator.Instance.GetPathSectionFromGridPosition(recievedPath[currentNode]).IsOccupied = false;
				PathGenerator.Instance.GetPathSectionFromGridPosition(recievedPath[nextNode]).IsOccupied = true;
				currentNode = nextNode;
			}
		}
		
	}

	#region IDamagable Methods
	public void TakeDamage(float damage)
	{
		if (currentState == IEnemy.EnemyState.Dead) return;


		currentHealth -= damage;
		if (currentHealth <= 0)
		{
			currentHealth = 0;
			Die();
		}
	}

	public void TakeDotDamage(float damagePerTick, int numOfTicks, float duration)
	{
		StartCoroutine(DamageOverTime(damagePerTick, numOfTicks, duration));
	}

	public void HealDamage(float healAmount)
	{
		if (currentState == IEnemy.EnemyState.Dead) return;

		currentHealth += healAmount;
		if (currentHealth > enemyData.health) currentHealth = enemyData.health;
	}

	internal virtual void Die()
	{
		gameObject.SetActive(false);
	}

	IEnumerator DamageOverTime(float damagePerTick, int numOfTicks, float duration)
	{
		int currentTick = 0;
		float timePerTick = numOfTicks / duration;
		while (currentTick < numOfTicks)
		{
			yield return new WaitForSeconds(timePerTick);
			TakeDamage(damagePerTick);
			currentTick++;
		}
	}

	public void Tether(float SpeedMult)
	{
		tetherMult = SpeedMult;
	}
	public void UnTether()
	{
		tetherMult = 1;
	}
	#endregion

}
