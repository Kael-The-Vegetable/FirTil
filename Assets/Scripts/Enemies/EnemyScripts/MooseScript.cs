using UnityEngine;

public class MooseScript : EnemyMain
{
	[SerializeField] float attackTimeStamp;
	[SerializeField] float chargeTimeStamp, chargeDuration ,chargeCoolDown;
	[SerializeField] bool charging;

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
		animator.SetBool("Attack", true);
		Invoke(nameof(StopAttack), 1.5f);
	}
	private void StopAttack()
	{
		animator.SetBool("Attack", false);
	}
}
