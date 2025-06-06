using UnityEngine;

public class HeadButtAttack : MonoBehaviour
{
	[SerializeField] private float damage = 2f;
	private void OnEnable()
	{
		Invoke("DisableSelf", 0.25f);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log(collision.gameObject.name);
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
