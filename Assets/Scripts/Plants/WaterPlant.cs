using UnityEngine;

public class WaterPlant : PlantMain
{
	[SerializeField] LayerMask allyMask;

	public override void Update()
	{
		base.Update();
		if (currentStage != IPlant.GrowthStage.Full) return;


		TryActivate();

	}
	public override void Activate()
	{
		Collider2D[] hitAllies = Physics2D.OverlapCircleAll(transform.position, currentRange, allyMask);

		if (hitAllies.Length <= 0) return;

		foreach (Collider2D ally in hitAllies)
		{
			ally.gameObject.GetComponent<IPlant>().WaterPlant();
		}

	}
}
