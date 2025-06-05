using UnityEngine;

public class ShooterPlant : PlantMain
{
	[SerializeField] LayerMask targetMask;

	[SerializeField] GameObject bulletPrefab;
	[SerializeField] GameObject target;
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
		
	}
	public override void Activate()
	{
		Vector3 direction = target.transform.position - transform.position;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
		bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);
		
	}
}
