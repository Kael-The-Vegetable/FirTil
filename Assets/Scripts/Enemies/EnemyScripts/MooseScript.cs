using UnityEngine;

public class MooseScript : EnemyMain
{
	[SerializeField] Animator animator;
	[SerializeField] float slamAttackSpeed;
	[SerializeField] float timeStamp;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (Time.time >= timeStamp + slamAttackSpeed)
		{
			timeStamp = Time.time;
			animator.SetBool("Attack", true);
			Invoke(nameof(StopSlam), 1.5f);
		}
	}

	private void StopSlam()
	{
		animator.SetBool("Attack", false);
	}
}
