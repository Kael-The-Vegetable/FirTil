using UnityEngine;

public class FlyTrapAttack : MonoBehaviour
{
	private void OnEnable()
	{
		Invoke("DisableSelf", 0.1f);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Enemy")
		{
			Debug.Log("Hit Enemy");
			// Damage
		}
	}

	private void DisableSelf()
	{
		gameObject.SetActive(false);
	}
}
