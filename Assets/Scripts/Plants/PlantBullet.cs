using UnityEngine;

public class PlantBullet : MonoBehaviour
{
    [SerializeField] float speed = 4;
    [SerializeField] float destroyTime = 3;
    [SerializeField] private float damage = 2;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		rb.linearVelocity = transform.right * speed;
        Invoke("DestroyBullet", destroyTime);
	}


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Enemy")
        {
            if (collision.TryGetComponent<IDamagable>(out IDamagable enemy))
            {
                enemy.TakeDamage(damage);
            }
        }
	}

	private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
