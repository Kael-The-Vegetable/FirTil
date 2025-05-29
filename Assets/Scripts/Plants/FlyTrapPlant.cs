using UnityEngine;

public class FlyTrapPlant : PlantMain
{
	[SerializeField] LayerMask targetMask;

	[SerializeField] GameObject attackCollider;
	[SerializeField] GameObject target;
	public override void Update()
	{
		base.Update();
		if (currentStage != IPlant.GrowthStage.Full) return;


		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, currentRange, targetMask);

		if (hitEnemies.Length > 0)
		{
			target = GetClosestEnemy(hitEnemies, transform.position);
			TryActivate();
		}
		else target = null;

	}
	public override void Activate()
	{
		Vector3 direction = target.transform.position - transform.position;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		attackCollider.transform.position = target.transform.position;
		attackCollider.transform.rotation = Quaternion.Euler(0f, 0f, angle);
		attackCollider.SetActive(true);

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
