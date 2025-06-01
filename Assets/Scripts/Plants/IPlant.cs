using UnityEngine;

public interface IPlant 
{
    public enum GrowthStage
    {
        Dead,
        Seed,
        Half,
        Full
    }

    public PlantData PlantData { get; set; }

    public float CurrentHealth { get; set; }

    public void AccelerateGrowth(float newGrowthRate, float duration);

    public void WaterPlant();
}
