using UnityEngine;

namespace AudioSystem
{
	[CreateAssetMenu(fileName = "AudioLibrary", menuName = "Scriptable Objects/Audio/AudioLibrary")]
	public class AudioLibrary : ScriptableObject
	{
		[SerializeField] private AudioGroup[] _audioGroups;
		public AudioGroup this[SoundType index]
		{
			get
			{
				for (int i = 0; i < _audioGroups.Length; i++)
				{
					if (_audioGroups[i].Type == index)
					{
						return _audioGroups[i];
					}
				}
				Debug.LogError($"Could not find an <color=#24c778>{nameof(AudioGroup)}</color> using the <color=#89ed53>{nameof(SoundType)}</color>.{index} Inside {name}");
				return null;
			}
		}
	}
}