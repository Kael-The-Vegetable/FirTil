using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Collections;

public class EnemyMain : MonoBehaviour, IDamagable
{
	[SerializeField] EnemyData enemyData;
	private float currentSpeed
	{
		get
		{
			return enemyData.MoveSpeed * tetherMult;
		}
	}
	float tetherMult = 1;

	float currentHealth;
	private Rigidbody2D rb;
	private Vector2 moveDir = Vector2.zero;
	private IEnemy.EnemyState currentState;
	

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
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        currentHealth = enemyData.Health;
		currentState = enemyData.State;
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
			Vector2 movementScalar = GridRatio.Instance.MovementScalar;
			moveDir = (actualPath[currentNode] - (Vector2)transform.position).normalized;

			transform.Translate(moveDir * currentSpeed * movementScalar * Time.deltaTime);

			if (Vector2.Distance((Vector2)transform.position, actualPath[currentNode]) <= DistanceBeforeSwitch)
			{
				NextNode();
			}
		}
    }

	void NextNode()
	{
		// Collider2D nodeHit = Physics2D.OverlapPoint(actualPath[currentNode + 1], blockageMask);
		currentNode += 1;
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
		if (currentHealth > enemyData.Health) currentHealth = enemyData.Health;
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
