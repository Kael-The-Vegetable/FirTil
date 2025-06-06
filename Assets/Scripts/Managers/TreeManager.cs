using UnityEngine;

public class TreeManager : Singleton<TreeManager>
{
    private const float MaxHealth = 200;
    [SerializeField] private float currentHealth;

    public Vector3 Position => transform.position;

    protected override void Initialize() { }
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        currentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Lose Health
        if (collision.tag == "Enemy" && TryGetComponent<IEnemy>(out IEnemy enemy))
        {
            LoseHealth(enemy.EnemyData.treeDamage);
            Destroy(collision.gameObject);
        }
	}

    void LoseHealth(float amount)
    {
        currentHealth -= amount;

        if (currentHealth < 0)
        {
            // End Game
        }
    }

	void GainHealth(float amount)
	{
		currentHealth += amount;

        if (currentHealth > MaxHealth) currentHealth = MaxHealth;
	}
}
