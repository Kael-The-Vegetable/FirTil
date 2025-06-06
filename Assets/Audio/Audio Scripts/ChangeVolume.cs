using AudioSystem;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ChangeVolume : MonoBehaviour
{
	private Slider _slider;
	[SerializeField] private SoundCategory _category;

	private void Awake()
	{
		_slider = GetComponent<Slider>();
	}
	private void OnEnable()
	{
		_slider.value = Mathf.Pow(10f, AudioManager.Instance.GetVolume(_category) / 20); // invert log10 operation
	}
	public void AlterVolume(float volume)
		=> AudioManager.Instance.SetVolume(_category, Mathf.Log10(volume) * 20);
	public void AlterVolume(float volume, SoundCategory category)
		=> AudioManager.Instance.SetVolume(category, Mathf.Log10(volume) * 20);
}
