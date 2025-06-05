using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayAudio : MonoBehaviour
{
	private AudioSource _audioSource;
	private void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
	}
	private void OnEnable()
	{
        if (!_audioSource.loop)
		{
			if (_audioSource.clip != null)
			{
				StartCoroutine(Playing());
			}
		}
	}

	private IEnumerator Playing()
	{
		yield return new WaitForSeconds(_audioSource.clip.length / _audioSource.pitch);
		yield return new WaitUntil(() => !_audioSource.isPlaying);
		Dispose();
	}

	public void Dispose()
	{
		gameObject.SetActive(false);
		_audioSource.Stop();
	}
}
