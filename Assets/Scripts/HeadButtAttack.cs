using UnityEngine;

public class HeadButtAttack : MonoBehaviour
{
	[SerializeField] private float damage = 2f;
	private void OnEnable()
	{
		Invoke("DisableSelf", 0.1f);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Enemy")
		{
			collision.GetComponent<IDamagable>().TakeDamage(damage);
			Debug.Log("Hit Enemy");
			
		}
	}

	private void DisableSelf()
	{
		gameObject.SetActive(false);
	}
}
