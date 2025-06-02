using UnityEngine;

public class ThornBush : PlantMain
{
	[SerializeField] LayerMask targetMask;

	[SerializeField] float damage = 0.5f;


	public override void Update()
	{
		base.Update();
		if (currentStage != IPlant.GrowthStage.Full) return;


		TryActivate();
	}

	public override void Activate()
	{
		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, plantData.BaseRange, targetMask);

		foreach (Collider2D hit in hitEnemies)
		{
			hit.gameObject.GetComponent<IDamagable>()?.TakeDamage(damage);
		}
	}
}
