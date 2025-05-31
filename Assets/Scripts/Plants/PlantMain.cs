using System.Collections;
using UnityEngine;

public class PlantMain : MonoBehaviour, IPlant, IDamagable
{
    public PlantData PlantData
    {
        get
        {
            return plantData;
        }
        set
        {
            plantData = value;
        }
    }
    public PlantData plantData;

    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value;
        }
    }
    public float currentHealth;

    public IPlant.GrowthStage currentStage = IPlant.GrowthStage.Seed;

    public float nextTimeToFire = 0;

    float growthProgress = 0;
    [SerializeField] bool watered = false;

    internal float currentFireRate, currentRange, currentGrowthRate;

	private void Awake()
	{
		currentHealth = plantData.MaxHealth;
        currentFireRate = plantData.BaseFireRate;
        currentRange = plantData.BaseRange;
        currentGrowthRate = plantData.BaseGrowthRate;
	}
	public virtual void Update()
	{
        if ( growthProgress < plantData.BaseGrowthTime && watered)
        {
			growthProgress += Time.deltaTime * currentGrowthRate;
		}
        
        // Advance to the next Growth Stage
        if (currentStage != IPlant.GrowthStage.Full && growthProgress >= plantData.BaseGrowthTime )
        {
            AdvanceGrowth();
        }
        
        
	}

    public void AdvanceGrowth()
    {
        switch( currentStage )
        {
            case IPlant.GrowthStage.Seed:
                currentStage = IPlant.GrowthStage.Half;
                watered = false;
                growthProgress = 0;

                // Change Appearance
                ChangeColor(Color.yellow);
                break;
            case IPlant.GrowthStage.Half:
                currentStage = IPlant.GrowthStage.Full;
                watered = false;
                growthProgress = plantData.BaseGrowthTime;

                // Change appearance
                ChangeColor(Color.green);
                break;
        }
    }

    // Check if the ability is on cooldown
	public virtual void TryActivate()
    {
		if (Time.time > nextTimeToFire)
		{
			nextTimeToFire = Time.time + (1 / currentFireRate);
            Activate();
		}
	}

    public virtual void Activate() { }

    // Accelerate plant growth for a set duration
	public void AccelerateGrowth(float newGrowthRate, float duration)
	{
        StopCoroutine(nameof(AcceleratedGrowth));
		StartCoroutine(AcceleratedGrowth(newGrowthRate, duration));
	}
    IEnumerator AcceleratedGrowth(float newGrowthRate, float duration)
    {
        currentGrowthRate = newGrowthRate;
        yield return new WaitForSeconds(duration);
        currentGrowthRate = plantData.BaseGrowthRate;
    }

    public void WaterPlant()
    {
        watered = true;
    }

	#region IDamagable Methods
    public void TakeDamage(float damage)
    {
        if (currentStage == IPlant.GrowthStage.Dead) return;


        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void TakeDotDamage(float damagePerTick, int numOfTicks, float duration)
    {
        StartCoroutine(DamageOverTime(damagePerTick, numOfTicks, duration));
    }

    public void HealDamage(float healAmount)
    {
        if (currentStage == IPlant.GrowthStage.Dead) return;

        currentHealth += healAmount;
        if (currentHealth > plantData.MaxHealth) currentHealth = plantData.MaxHealth;
    }

    private void Die()
    {

    }

	public void Tether(float SpeedMult) { }
	public void UnTether() { }

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
	#endregion

	// For Testing
	void ChangeColor(Color color)
    {
        GetComponentInChildren<SpriteRenderer>().color = color;
    }

	private void OnDrawGizmosSelected()
	{
        Gizmos.color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
		Gizmos.DrawSphere(transform.position, plantData.BaseRange);
	}
}
