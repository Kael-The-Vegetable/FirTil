using System.Collections;
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

		StartCoroutine(WaterPlants());

	}

	IEnumerator WaterPlants()
	{
		bodyAnim.SetTrigger("Attack");
		yield return new WaitForSeconds(1.5f);
		Collider2D[] hitAllies = Physics2D.OverlapCircleAll(transform.position, currentRange, allyMask);

		if (hitAllies.Length > 0)
		{
			foreach (Collider2D ally in hitAllies)
			{
				ally.gameObject.GetComponent<IPlant>().WaterPlant();
			}
		}

		
	}
}
