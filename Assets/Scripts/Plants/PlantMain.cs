using System.Collections;
using UnityEngine;

public class PlantMain : MonoBehaviour, IPlant
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

    private float currentFireRate, currentRange, currentGrowthRate;

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
			growthProgress += Time.deltaTime * plantData.BaseGrowthRate;
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

	public virtual void TryActivate()
    {
		if (Time.time > nextTimeToFire)
		{
			nextTimeToFire = Time.time + (1 / currentFireRate);
            Activate();
		}
	}

    public virtual void Activate() { }

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

    public void Damage(float damage)
    {

    }

    // For Testing
    void ChangeColor(Color color)
    {
        GetComponentInChildren<SpriteRenderer>().color = color;
    }
}
