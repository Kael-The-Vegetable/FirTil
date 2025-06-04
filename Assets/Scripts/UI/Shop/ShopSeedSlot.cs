using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopSeedSlot : SeedSlot, IPointerDownHandler
{
	[SerializeField] private TextMeshProUGUI _priceText;
	private int _price;
	private int _selectedQuantity;
	public int SelectedQuantity
	{
		get => _selectedQuantity;
		set
		{
			_selectedQuantity = value;
			if (_selectedQuantity != 0)
			{
				seedQuantity.text = $"{_selectedQuantity}/{quantity}";
				seedQuantity.color = Color.deepPink;
			}
			else
			{
				seedQuantity.text = quantity.ToString();
				seedQuantity.color = Color.white;
			}
		}
	}
	public int Price
	{
		get => _price;
		set
		{
			_price = value;
			_priceText.text = _price.ToString();
		}
	}

	public PurchaseSeed purchaser;

	protected override void Awake()
	{
		base.Awake();
		_priceText = _priceText != null ? _priceText : GetComponentsInChildren<TextMeshProUGUI>()[^1]; // last in order
	}
	public override void Initialize(Seed seed)
	{
		base.Initialize(seed);
		Price = seed.VariableCost.Random();
		Quantity = seed.Availability.Random();
	}
	public void OnPointerDown(PointerEventData eventData)
	{
		SelectedQuantity = (SelectedQuantity + 1) % (quantity + 1);
		purchaser.AddToPurchase(this, SelectedQuantity);
	}
}
