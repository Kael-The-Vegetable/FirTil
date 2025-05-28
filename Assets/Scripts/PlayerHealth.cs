using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float maxHealth;
    [SerializeField] float currentHealth;

    [SerializeField] Image healthBar;
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
		#region healing and damage testing
		if (Input.GetKeyUp(KeyCode.UpArrow))
		{
            ApplyHealing(2);
		}

		if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            ApplyDamage(2);
        }
		#endregion
	}

	public void ApplyDamage(float damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            //game over method called
        }
    }

    public void ApplyHealing(float healing)
    {
        currentHealth += healing;
        UpdateHealthBar();

        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    void UpdateHealthBar()
    {
        healthBar.transform.localScale = new Vector3((currentHealth /  maxHealth), 1f, 1f);
    }
}
