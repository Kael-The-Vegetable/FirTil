using System.Collections;
using UnityEngine;

public class MelonPlant : PlantMain
{
	[SerializeField] LayerMask targetMask;

	[SerializeField] GameObject bulletPrefab;
	[SerializeField] GameObject target;

	[SerializeField] GameObject melonObj;

	public override void Awake()
	{
		base.Awake();
		bodyAnim = GetComponentInChildren<Animator>();

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

	}
	public override void Activate()
	{
		//Vector3 direction = target.transform.position - transform.position;
		//float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		//GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
		//bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);

		bodyAnim.SetTrigger("Attack");
		StartCoroutine(LaunchMelon());

	}
	
	public IEnumerator LaunchMelon()
	{
		
		yield return new WaitForSeconds(0.84f);
		melonObj.SetActive(false);
		GameObject melon = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
		melon.GetComponent<MelonBomb>().StartArcMovement(target.transform);
		yield return new WaitForSeconds(1);
		melonObj.SetActive(true);
	}
}
