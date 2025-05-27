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

    public IPlant.GrowthStage currentStage = IPlant.GrowthStage.Seed;

    public float nextTimeToFire = 0;

    float growthProgress = 0;

    private float currentFireRate, currentRange;

	public virtual void Update()
	{
        if ( growthProgress < plantData.BaseGrowthTime)
        {
			growthProgress += Time.deltaTime * plantData.BaseGrowthRate;
		}
        
        // Advance to the next Growth Stage
        if (currentStage == IPlant.GrowthStage.Seed && growthProgress >= plantData.BaseGrowthTime )
        {
            growthProgress = plantData.BaseGrowthTime;
            currentStage = IPlant.GrowthStage.Full;

            // Testing
            ChangeColor();
        }
        
        
	}

	public virtual void TryActivate()
    {
		if (Time.time > nextTimeToFire)
		{
			nextTimeToFire = Time.time + (1 / plantData.BaseFireRate);
            Activate();
		}
	}

    public virtual void Activate() { }

	public void AccelerateGrowth(float newGrowthRate, float duration)
	{
		throw new System.NotImplementedException();
	}

    void ChangeColor()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.red;
    }
}
