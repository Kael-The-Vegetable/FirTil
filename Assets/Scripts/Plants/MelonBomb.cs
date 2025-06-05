using UnityEngine;

public class MelonBomb : MonoBehaviour
{
	[Header("Melon Arc")]
	public Transform target;
	public float travelTime = 1f;
	public float height = 2f;

	private Vector3 startPos;
	private Vector3 targetPos;
	private float timer;

	private bool moving = false;

	[Header("Damage")]
	[SerializeField] float explosionRange = 2;
	[SerializeField] float damage = 2;
	[SerializeField] LayerMask enemyMask;
	private Rigidbody2D rb;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	public void StartArcMovement(Transform newTarget)
	{
		target = newTarget;
		startPos = transform.position;
		targetPos = target.position;
		timer = 0f;
		moving = true;
	}

	void Update()
	{
		if (!moving) return;

		timer += Time.deltaTime;
		float progress = timer / travelTime;

		// Parabolic interpolation
		Vector3 current = Vector3.Lerp(startPos, targetPos, progress);
		float arc = height * 4 * (progress - progress * progress); // Parabola peak at t = 0.5
		current.y += arc;

		transform.position = current;

		if (progress >= 1f)
		{
			// Explode
			Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRange, enemyMask);

			foreach (Collider2D enemy in hitEnemies)
			{
				if (enemy.TryGetComponent<IDamagable>(out IDamagable target))
				{
					target.TakeDamage(damage);
				}
			}

			// Explosion Effects

			// Destroy
			Destroy(gameObject);
			moving = false;
		}
	}

	private void DestroyBullet()
	{
		Destroy(gameObject);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawSphere(transform.position, explosionRange);
	}
}
