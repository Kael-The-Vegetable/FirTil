using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.UIElements.Experimental;

public class PlayerHealth : MonoBehaviour, IDamagable
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
		if (Keyboard.current.oKey.wasPressedThisFrame)
		{
            HealDamage(2);
		}

		if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            HealDamage(2);
        }
		#endregion
	}

    void UpdateHealthBar()
    {
        healthBar.transform.localScale = new Vector3((currentHealth /  maxHealth), 1f, 1f);
    }

	public void TakeDamage(float damage)
	{

		currentHealth -= damage;
		UpdateHealthBar();

		if (currentHealth <= 0)
		{
			currentHealth = 0;
			UpdateHealthBar();
			//game over method called
		}
	}

	public void TakeDotDamage(float damagePerTick, int numOfTicks, float duration)
	{
		StartCoroutine(DamageOverTime(damagePerTick, numOfTicks, duration));
	}

	public void HealDamage(float healAmount)
	{

		currentHealth += healAmount;
		UpdateHealthBar();

		if (currentHealth >= maxHealth)
		{
			currentHealth = maxHealth;
			UpdateHealthBar();
		}
	}

	IEnumerator DamageOverTime(float damagePerTick, int numOfTicks, float duration)
	{
		int currentTick = 0;
		float timePerTick = numOfTicks / duration;
		while (currentTick < numOfTicks)
		{
			yield return new WaitForSeconds(timePerTick);
			TakeDamage(damagePerTick);
			currentTick++;
		}
	}

	public void Tether(float speedMult) { }

    public void UnTether() { }
}
