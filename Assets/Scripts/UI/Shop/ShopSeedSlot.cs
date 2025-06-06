using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSeedSlot : SeedSlot, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
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
	[Space]
	[SerializeField] private Image _image;
	private Sprite _original;
	[SerializeField] private Sprite _highlight;
	protected override void Awake()
	{
		base.Awake();
		_priceText = _priceText != null ? _priceText : GetComponentsInChildren<TextMeshProUGUI>()[^1]; // last in order
		_image = _image != null ? _image : GetComponentInChildren<Image>();

		_original = _image.sprite;
	}
	public override void Initialize(Seed seed)
	{
		base.Initialize(seed);
		Price = seed.VariableCost.Random();
		Quantity = seed.Availability.Random();
	}
	public void OnPointerDown(PointerEventData eventData)
	{
		if (Seed == null) return;

		if (SeedInventory.instance.CheckSeedAmount(Seed) + SelectedQuantity + 1 <= Seed.MaxStackSize )
		{
			SelectedQuantity = (SelectedQuantity + 1) % (quantity + 1);
		}
		else
		{
			SelectedQuantity = 0;
		}
		
		purchaser.AddToPurchase(this, SelectedQuantity);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		_image.sprite = _highlight;
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		_image.sprite = _original;
	}
}
