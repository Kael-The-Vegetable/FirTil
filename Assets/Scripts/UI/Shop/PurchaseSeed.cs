using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PurchaseSeed : MonoBehaviour
{
	[SerializeField] private Button _purchase;
	[SerializeField] private Image _purchaseImage;
	[SerializeField] private TextMeshProUGUI _price;
	[SerializeField] private Button _cancel;
	[SerializeField] private List<Seed> _seeds;
	private List<ShopSeedSlot> _slots = new();

	private Dictionary<ShopSeedSlot, int> _seedsToBuy = new();

	private void Awake()
	{
		_slots.AddRange(GetComponentsInChildren<ShopSeedSlot>());
		foreach (ShopSeedSlot slot in _slots)
			slot.purchaser = this;

		_purchase.interactable = false;
		_cancel.interactable = false;

		AssignSeeds();
	}

	public void AddToPurchase(ShopSeedSlot seed, int amount)
	{
		_purchase.interactable = true;
		_cancel.interactable = true;
		if (!_seedsToBuy.TryAdd(seed, amount))
		{
			_seedsToBuy[seed] = amount;
		}

		if (amount == 0)
		{ // check if empty
			int i = 0;
			foreach (int a in _seedsToBuy.Values)
			{
				i += a;
			}
			if (i == 0)
			{ // empty
				_purchase.interactable = false;
				_cancel.interactable = false;
			}
		}

		ValidatePurchase();
	}

	public void ValidatePurchase()
	{
		int currentPrice = 0; 

		foreach (var seeds in _seedsToBuy)
		{
			currentPrice += seeds.Value * seeds.Key.Price;
		}
		_price.text = $"({currentPrice})";
		bool valid = EconomyManager.Instance.ValidPurchase(currentPrice);
		_purchaseImage.color = valid ? Color.white : Color.darkRed;
		_purchase.interactable = _purchase.interactable ? valid : _purchase.interactable;
	}

	public void ClearPurchase()
	{
		foreach (var seed in _seedsToBuy)
		{
			seed.Key.SelectedQuantity = 0;
		}
		_purchase.interactable = false;
		_cancel.interactable = false;
		_seedsToBuy.Clear();
		ValidatePurchase();
	}

	public void MakePurchase()
	{
		foreach (var seeds in _seedsToBuy)
		{
			if (seeds.Value > 0)
			{
				
				SeedInventory.instance.AddSeed(seeds.Key.Seed, seeds.Value);
				EconomyManager.Instance.RemovePoints(seeds.Value * seeds.Key.Price);
				seeds.Key.Quantity -= seeds.Key.SelectedQuantity;
			}
			
			
		}
		ClearPurchase();

		// Close Shop
		//SpawnerManager.Instance.StartNextDay();
		//SceneManager.UnloadSceneAsync("Store");
	}

	void AssignSeeds()
	{
		List<Seed> availableSeed = _seeds;
		foreach (var slot in _slots)
		{
			int randIndex = Random.Range(0, availableSeed.Count);
			slot.Seed = availableSeed[randIndex];
			slot.Quantity = Random.Range(availableSeed[randIndex].Availability.Min, availableSeed[randIndex].Availability.Max + 1);
			slot.Price = Random.Range(availableSeed[randIndex].VariableCost.Min, availableSeed[randIndex].VariableCost.Max);
			availableSeed.RemoveAt(randIndex);
		}
	}
}
