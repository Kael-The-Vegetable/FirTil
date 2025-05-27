using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    [SerializeField] private float movespeed = 2f;
    private Rigidbody2D rb;

    [SerializeField] Transform checkpoint;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	private void FixedUpdate()
	{
		Vector2 direction = (checkpoint.position - transform.position).normalized;
        rb.linearVelocity = direction * movespeed;
	}
}
