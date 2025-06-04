using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoisonMushroom : PlantMain
{
	[SerializeField] LayerMask targetMask;

	[Header("Poison")]
	[SerializeField] float damagePerTick = 0.5f;
	[SerializeField] float poisonDuration = 3f;
	[SerializeField] int numOfTicks = 5;


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
			if (hit.TryGetComponent<IDamagable>(out IDamagable enemy))
			{
				enemy.TakeDotDamage(damagePerTick, numOfTicks, poisonDuration);
			}
		}
	}
}
