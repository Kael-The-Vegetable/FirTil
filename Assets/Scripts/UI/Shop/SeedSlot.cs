using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SeedSlot : MonoBehaviour
{
	[SerializeField] private Seed _seed;
	[SerializeField] private bool _shopSlot;
	[Space]
	[SerializeField] private Image _seedImage;
	[SerializeField] private TextMeshProUGUI _seedQuantity;
	private int _quantity;
	public Seed Seed
	{
		get => _seed;
		set
		{
			_seed = value;
			_seedImage.sprite = _seed?.PlantImage;
			if (_seed == null)
			{
				_quantity = 0;
				_seedQuantity.text = string.Empty;
			}
		}
	}
	public int Quantity
	{
		get => _quantity;
		set
		{
			if (_quantity != value)
			{
				_quantity = value;
				_seedQuantity.text = _quantity.ToString();
			}
		}
	}

	private void Awake()
	{
		_seedImage = _seedImage != null ? _seedImage : GetComponentsInChildren<Image>()[^1]; // last one because background is before the image
		_seedQuantity = _seedQuantity != null ? _seedQuantity : GetComponentInChildren<TextMeshProUGUI>();

		Initialize(_seed);
	}

	public void Initialize(Seed seed)
	{
		Seed = seed;
		if (_shopSlot)
		{
			Quantity = seed.Availability.Random();
		}
	}
}
