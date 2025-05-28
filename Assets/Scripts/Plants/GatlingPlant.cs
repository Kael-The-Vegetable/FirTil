using UnityEngine;

public class GatlingPlant : PlantMain
{
	[SerializeField] LayerMask targetMask;

	[SerializeField] GameObject bulletPrefab;
	[SerializeField] private float damage = 2;
	[SerializeField] private float maxBulletSpread = 5;
	[SerializeField] private float bulletSpreadGain = 0.5f;
	private float bulletSpread = 0;
	private float spreadLossTimer = 0;

	[SerializeField] GameObject target;
	public override void Update()
	{
		base.Update();

		if (currentStage != IPlant.GrowthStage.Full) return;

		// Reduce spread if there's no target in range
		if (bulletSpread > 0 && spreadLossTimer < Time.time && !target)
		{
			spreadLossTimer = Time.time + bulletSpreadGain;
			bulletSpread -= 1;
		}

		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, plantData.BaseRange, targetMask);

		if (hitEnemies.Length > 0)
		{
			target = GetClosestEnemy(hitEnemies, transform.position);
			TryActivate();
		}
		else target = null;

	}
	public override void Activate()
	{
		// Get Direction of target and apply spread
		Vector3 direction = target.transform.position - transform.position;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		angle += Random.Range(-bulletSpread, bulletSpread);
		GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
		bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);

		// Add spread for subsequent bullets
		bulletSpread += bulletSpreadGain;

	}

	GameObject GetClosestEnemy(Collider2D[] enemies, Vector2 referencePosition)
	{
		Collider2D closest = null;
		float minDistance = 1000;

		foreach (Collider2D enemy in enemies)
		{
			if (enemy == null) continue;

			float distance = Vector2.Distance(referencePosition, enemy.transform.position);
			if (distance < minDistance)
			{
				minDistance = distance;
				closest = enemy;
			}
		}

		return closest.gameObject;
	}
}
