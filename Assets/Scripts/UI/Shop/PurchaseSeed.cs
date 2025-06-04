using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseSeed : MonoBehaviour
{
	[SerializeField] private Button _purchase;
	[SerializeField] private Button _cancel;
	private List<ShopSeedSlot> _slots = new();

	private Dictionary<ShopSeedSlot, int> _seedsToBuy = new();

	private void Awake()
	{
		_slots.AddRange(GetComponentsInChildren<ShopSeedSlot>());
		foreach (ShopSeedSlot slot in _slots)
			slot.purchaser = this;

		_purchase.interactable = false;
		_cancel.interactable = false;
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
			Debug.Log(i);
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
	}
}
