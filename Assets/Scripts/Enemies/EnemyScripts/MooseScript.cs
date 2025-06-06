using UnityEngine;

public class MooseScript : EnemyMain
{
	[SerializeField] float slamAttackTimeStamp;
	[SerializeField] float chargeTimeStamp, chargeDuration ,chargeCoolDown;
	[SerializeField] bool charging;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (!charging && Time.time >= chargeTimeStamp + chargeCoolDown)
		{
			Charge();
		}

		if (!charging && Time.time >= slamAttackTimeStamp + attackSpeed)
		{
			slamAttackTimeStamp = Time.time;
			animator.SetBool("Attack", true);
			Invoke(nameof(StopSlam), 1.5f);
		}
	}

	private void StopSlam()
	{
		animator.SetBool("Attack", false);
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
}
