using UnityEngine;

public class GatlingPlant : PlantMain
{
	[SerializeField] LayerMask targetMask;

	[SerializeField] GameObject bulletPrefab;
	[SerializeField] private float maxBulletSpread = 5;
	[SerializeField] private float bulletSpreadGain = 0.5f;
	[SerializeField] private float fireRateGain = 0.05f;
	private float bulletSpread = 0;
	private float spreadLossTimer = 0;
	private float maximumFireRate;

	[SerializeField] GameObject target;

	public override void Start()
	{
		base.Start();
		maximumFireRate = currentFireRate * 2;
	}
	public override void Update()
	{
		base.Update();

		if (currentStage != IPlant.GrowthStage.Full) return;

		

		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, plantData.BaseRange, targetMask);

		if (hitEnemies.Length > 0)
		{
			target = GetTarget(hitEnemies, transform.position);
			TryActivate();
		}
		else target = null;


		// Reduce spread if there's no target in range
		if (bulletSpread > 0 && spreadLossTimer < Time.time && !target)
		{
			spreadLossTimer = Time.time + bulletSpreadGain;
			bulletSpread -= 1;
		}

		if (currentFireRate > plantData.BaseFireRate && !target)
		{
			currentFireRate = plantData.BaseFireRate;
		}

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
		if (bulletSpread > maxBulletSpread)
		{
			bulletSpread = maxBulletSpread;
		}

		// Increase firerate with continous fire
		currentFireRate += fireRateGain;
		if (currentFireRate > maximumFireRate)
		{
			currentFireRate = maximumFireRate;
		}

	}
}
