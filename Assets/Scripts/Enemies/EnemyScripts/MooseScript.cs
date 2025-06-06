using UnityEngine;

public class MooseScript : EnemyMain
{
	[SerializeField] float attackTimeStamp;
	[SerializeField] float chargeTimeStamp, chargeDuration ,chargeCoolDown;
	[SerializeField] bool charging;
	[SerializeField] internal LayerMask attackMask;

	// Update is called once per frame
	void Update()
	{
		if (!charging && Time.time >= chargeTimeStamp + chargeCoolDown)
		{
			Charge();
		}

		if (!charging && Time.time >= attackTimeStamp + attackSpeed && isStopped)
		{
			Attack();
		}
	}
	private void Charge()
	{
		charging = true;
		movespeed *= 2;
		Invoke(nameof(StopCharging), chargeDuration);
	}
	private void StopCharging()
	{
		charging = false;
		movespeed /= 2;
		chargeTimeStamp = Time.time;
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
