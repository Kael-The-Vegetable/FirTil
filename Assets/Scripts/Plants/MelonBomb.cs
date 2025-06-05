using UnityEngine;

public class MelonBomb : MonoBehaviour
{
	[SerializeField] float speed = 4;
	[SerializeField] float destroyTime = 6;
	[SerializeField] float explosionRange = 2;
	[SerializeField] float damage = 2;
	[SerializeField] LayerMask enemyMask;
	private Rigidbody2D rb;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		rb.linearVelocity = transform.right * speed;
		Invoke("DestroyBullet", destroyTime);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Enemy")
		{
			Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRange, enemyMask);

			foreach (Collider2D enemy in hitEnemies)
			{
				if (enemy.TryGetComponent<IDamagable>(out IDamagable target))
				{
					target.TakeDamage(damage);
				}
			}

			// Deactivate
			Destroy(gameObject); // For Testing 
		}
	}

	private void DestroyBullet()
	{
		Destroy(gameObject);
	}
}
