using System.Collections.Generic;
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
    public List<SeedItem> seeds = new();
	public int equippedSeed = 0;

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

	public void NextSeed()
	{
		if (equippedSeed + 1 == seeds.Count)
		{
			equippedSeed = 0;
		}
		else equippedSeed += 1;

		if(IsInventoryEmpty() == false)
		{
			SeedDisplay.Instance.UpdateDisplay(seeds[equippedSeed].seed.PlantImage, seeds[equippedSeed].seed.name, seeds[equippedSeed].quantity);
		}
	}

	public void PreviousSeed()
	{
		if (equippedSeed == 0)
		{
			equippedSeed = seeds.Count - 1;
		}
		else equippedSeed -= 1;

		if (IsInventoryEmpty() == false)
		{
			SeedDisplay.Instance.UpdateDisplay(seeds[equippedSeed].seed.PlantImage, seeds[equippedSeed].seed.name, seeds[equippedSeed].quantity);
		}
	}

	public void CheckEquippedSeed()
	{
		if (equippedSeed >= seeds.Count)
		{
			equippedSeed = 0;
		}

		if (IsInventoryEmpty() == false)
		{
			SeedDisplay.Instance.UpdateDisplay(seeds[equippedSeed].seed.PlantImage, seeds[equippedSeed].seed.name, seeds[equippedSeed].quantity);
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
			RemoveItem(seeds[equippedSeed].seed, 1);
			return plant;
		}
		
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
