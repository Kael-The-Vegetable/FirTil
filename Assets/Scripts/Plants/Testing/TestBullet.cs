using UnityEngine;

public class TestBullet : MonoBehaviour
{
    [SerializeField] float speed = 4;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		rb.linearVelocity = transform.right * speed;
	}

    // Update is called once per frame
    void Update()
    {
		
	}

	private void FixedUpdate()
	{
		
	}
}
