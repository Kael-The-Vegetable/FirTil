using AudioSystem;
using UnityEngine;

public class TryToPlayAudio : MonoBehaviour
{
    public SoundType soundType;
	public bool playUponAwake;
	public bool loop;
	private void Awake()
	{
		if (playUponAwake)
		{
			if (loop) TryToPlayLoopingClip(1);
			else TryToPlayClip();
		}
	}
	private void OnDestroy()
	{
		if (loop && AudioManager.HasInstance) AudioManager.Instance.StopLoopingSounds(soundType);
	}

	public void TryToPlayClip()
		=> AudioManager.Instance.PlaySound(soundType, transform.position);
	public void TryToPlayClip(float customVolume)
		=> AudioManager.Instance.PlaySound(soundType, transform.position, customVolume);
	public void TryToPlayClip(Vector3 pointInSpace)
		=> AudioManager.Instance.PlaySound(soundType, pointInSpace);
    public void TryToPlayLoopingClip(float customVolume)
    => AudioManager.Instance.PlayLoopingSound(soundType, transform.position, customVolume);
}
