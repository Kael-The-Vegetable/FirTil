using UnityEngine;

public class SnakeTower : PlantMain
{
	[SerializeField] LayerMask targetMask;

	[SerializeField] GameObject bulletPrefab;
	[SerializeField] GameObject target;

	private Animator animator;

	public override void Awake()
	{
		base.Awake();
		animator = GetComponentInChildren<Animator>();
	}
	public override void Update()
	{
		base.Update();
		if (currentStage != IPlant.GrowthStage.Full) return;


		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, plantData.BaseRange, targetMask);

		if (hitEnemies.Length > 0)
		{
			target = GetTarget(hitEnemies, transform.position);
			AnimateLook();
			TryActivate();
		}
		else target = null;

	}
	public override void Activate()
	{
		animator.SetTrigger("Attack");
		Vector3 direction = target.transform.position - transform.position;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
		bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);

	}

	void AnimateLook()
	{
		Vector3 animDirection = (target.transform.position - transform.position).normalized;
		animator.SetFloat("LookX", animDirection.x);
		animator.SetFloat("LookY", animDirection.y);
		if (animDirection.x < 0)
		{
			bodySprite.flipX = true;
		}
		else bodySprite.flipX = false;
	}
}
