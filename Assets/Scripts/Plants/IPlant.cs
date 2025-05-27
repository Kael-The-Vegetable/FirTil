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

    public void AccelerateGrowth(float newGrowthRate, float duration);
}
