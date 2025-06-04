using UnityEngine;

public class FlyTrapAttack : MonoBehaviour
{
	[SerializeField] private float damage = 2;
	private void OnEnable()
	{
		Invoke("DisableSelf", 0.1f);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Enemy")
		{
			Debug.Log("Hit Enemy");
			if (collision.TryGetComponent<IDamagable>(out IDamagable enemy))
			{
				enemy.TakeDamage(2);
			}
		}
	}

	private void DisableSelf()
	{
		gameObject.SetActive(false);
	}
}
