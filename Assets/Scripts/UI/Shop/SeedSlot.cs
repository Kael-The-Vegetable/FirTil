using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SeedSlot : MonoBehaviour
{
	[SerializeField] private Seed _seed;
	[Space]
	[SerializeField] private Image _seedImage;
	[SerializeField] protected TextMeshProUGUI seedQuantity;
	protected int quantity;
	public Seed Seed
	{
		get => _seed;
		set
		{
			_seed = value;
			_seedImage.sprite = _seed?.PlantImage;
			if (_seed == null)
			{
				quantity = 0;
				seedQuantity.text = string.Empty;
			}
		}
	}
	public int Quantity
	{
		get => quantity;
		set
		{
			if (quantity != value)
			{
				quantity = value;
				seedQuantity.text = quantity.ToString();
			}
		}
	}

	protected virtual void Awake()
	{
		_seedImage = _seedImage != null ? _seedImage : GetComponentsInChildren<Image>()[^1]; // last one because background is before the image
		seedQuantity = seedQuantity != null ? seedQuantity : GetComponentInChildren<TextMeshProUGUI>();

		Initialize(_seed);
	}

	public virtual void Initialize(Seed seed)
	{
		Seed = seed;
	}
}
