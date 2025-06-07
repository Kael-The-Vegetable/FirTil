using UnityEngine;

public class BeaverScript : EnemyMain
{
	[SerializeField] float attackTimeStamp;
	[SerializeField] internal LayerMask attackMask;

	// Update is called once per frame
	public override void Update()
	{
		base.Update();
		if (Time.time >= attackTimeStamp + attackSpeed && isStopped)
		{
			Attack();
		}
	}

	private void Attack()
	{
		attackTimeStamp = Time.time;
		bodyAnimator.SetBool("Attack", true);
		Invoke(nameof(StopAttack), 1.5f);
	}
	private void StopAttack()
	{
		// Checks that the tile is actually blocked
		if (currentNode + 1 <= actualPath.Count - 1)
		{
			Collider2D target = Physics2D.OverlapPoint(actualPath[currentNode + 1], attackMask);
			if (target != null && target.TryGetComponent<IDamagable>(out IDamagable hit))
			{
				hit.TakeDamage(damage);
			}
		}
		bodyAnimator.SetBool("Attack", false);
	}
}
