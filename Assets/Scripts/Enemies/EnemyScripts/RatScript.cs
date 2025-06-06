using UnityEngine;

public class RatScript : EnemyMain
{
	[SerializeField] float attackTimeStamp;

    // Update is called once per frame
    void Update()
    {
		if (Time.time >= attackTimeStamp + attackSpeed && isStopped)
		{
			Attack();
		}
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
