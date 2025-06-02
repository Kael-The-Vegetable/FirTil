using UnityEngine;
using System.Collections;

public class EnemyTest : MonoBehaviour, IDamagable
{
    [SerializeField] private float movespeed = 2f;
	private float currentSpeed
	{
		get
		{
			return movespeed * tetherMult;
		}
	}
	float tetherMult = 1;

	[SerializeField] private float currentHealth;
	[SerializeField] private float maxHealth = 4;
    private Rigidbody2D rb;

    [SerializeField] Transform checkpoint;

	bool dead = false;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        currentHealth = maxHealth;
    }

	private void FixedUpdate()
	{
		Vector2 direction = (checkpoint.position - transform.position).normalized;
        rb.linearVelocity = direction * currentSpeed;
	}

	public void TakeDamage(float damage)
	{
		if (dead) return;


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
		if (dead) return;

		currentHealth += healAmount;
		if (currentHealth > maxHealth) currentHealth = maxHealth;
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
}
