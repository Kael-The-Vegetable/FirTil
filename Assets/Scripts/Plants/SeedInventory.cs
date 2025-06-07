using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SeedItem
{
    public Seed seed;
    public int quantity;

    public SeedItem(Seed seed, int quantity)
	{
		this.seed = seed;
		this.quantity = quantity;
	}
}

public class SeedInventory : MonoBehaviour
{
	public static SeedInventory instance;
    public List<SeedItem> seeds = new();
	public int equippedSeed = 0;


	private void Awake()
	{
		instance = this;
	}
	public void AddSeed(Seed seed, int amount)
	{
		SeedItem existingItem = seeds.Find(i => i.seed == seed);

		if (existingItem != null && existingItem.quantity < existingItem.seed.MaxStackSize)
		{
			existingItem.quantity += amount;
		}
		else
		{
			seeds.Add(new SeedItem(seed, amount));
		}

		if (IsInventoryEmpty() == false && SeedDisplay.Instance != null)
		{
			SeedDisplay.Instance.UpdateDisplay(seeds[equippedSeed].seed.PlantImage, seeds[equippedSeed].seed.Plant.PlantName, seeds[equippedSeed].quantity);
		}
	}

	public void RemoveItem(Seed seed, int amount)
	{
		SeedItem existingItem = seeds.Find(i => i.seed == seed);

		if (existingItem != null)
		{
			existingItem.quantity -= amount;
			if (existingItem.quantity <= 0)
			{
				seeds.Remove(existingItem);
			}
		}
		CheckEquippedSeed();
	}

	public int CheckSeedAmount(Seed seed)
	{
		SeedItem foundSeed = seeds.Find(i => i.seed == seed);
		if (foundSeed != null)
		{
			return foundSeed.quantity;
		}
		else return 0;
	}

	public void NextSeed()
	{
		if (equippedSeed + 1 == seeds.Count)
		{
			equippedSeed = 0;
		}
		else equippedSeed += 1;

		if(IsInventoryEmpty() == false && SeedDisplay.Instance != null)
		{
			SeedDisplay.Instance.UpdateDisplay(seeds[equippedSeed].seed.PlantImage, seeds[equippedSeed].seed.Plant.PlantName, seeds[equippedSeed].quantity);
		}
	}

	public void PreviousSeed()
	{
		if (equippedSeed == 0)
		{
			equippedSeed = seeds.Count - 1;
		}
		else equippedSeed -= 1;

		if (IsInventoryEmpty() == false && SeedDisplay.Instance != null)
		{
			SeedDisplay.Instance.UpdateDisplay(seeds[equippedSeed].seed.PlantImage, seeds[equippedSeed].seed.Plant.PlantName, seeds[equippedSeed].quantity);
		}
	}

	public void CheckEquippedSeed()
	{
		if (equippedSeed >= seeds.Count)
		{
			equippedSeed = 0;
		}

		if (IsInventoryEmpty() == false && SeedDisplay.Instance != null)
		{
			SeedDisplay.Instance.UpdateDisplay(seeds[equippedSeed].seed.PlantImage, seeds[equippedSeed].seed.Plant.PlantName, seeds[equippedSeed].quantity);
		}
		else if (IsInventoryEmpty() == true)
		{
			SeedDisplay.Instance.ResetDisplay();
		}
	}

	public PlantData GetPlant()
	{
		if (seeds.Count == 0)
		{
			return null;
		}
		else
		{
			PlantData plant = seeds[equippedSeed].seed.Plant;
			return plant;
		}
		
	}

	public void RemoveEquippedSeed(int amount)
	{
		RemoveItem(seeds[equippedSeed].seed, amount);
	}

	public bool IsInventoryEmpty()
	{
		if (seeds.Count == 0)
		{
			return true;
		}
		else return false;
	}
}
