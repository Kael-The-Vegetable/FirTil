using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace AudioSystem
{
	public class AudioManager : PersistentSingleton<AudioManager>
	{
		[SerializeField] private AudioLibrary _library;
		[Space]
		[SerializeField] private ObjectPool<AudioSource> _audioPool;
		[SerializeField] private AudioMixer _mixer;
		protected override void Initialize()
		{
			_audioPool.CreatePoolInstantly(transform);
		}
		public void PlaySound(SoundType type, Vector3 position = default, float volume = 1)
		{
			AudioSource source = _audioPool.GetInactiveObject;
			if (source == null) return;

			AudioGroup group = _library[type];
			if (group == null) return;
			string mix = group.Category.ToString();
			var mixes = _mixer.FindMatchingGroups(mix);
			if (mixes.Length == 0)
			{
				Debug.LogError($"Could not find any mixers of name {group.Category}");
				return;
			}
			source.outputAudioMixerGroup = mixes[0];
			var data = group.RandomClip();
			source.clip = data.Clip;
			source.pitch = Random.Range(data.PitchVariation.Min, data.PitchVariation.Max);
			source.volume = volume * data.VolumeModifier;
			source.spatialBlend = group.SpatialBlend;
			if (source.spatialBlend != 0) source.transform.position = position;
			source.loop = false;
			source.gameObject.SetActive(true);
			source.Play();
		}
		public void PlayLoopingSound(SoundType type, Vector3 position = default, float volume = 1)
		{
			AudioGroup group = _library[type];
			if (group == null) return;
			AudioSource source = _audioPool.GetObject;
			if (source == null) return;
            string mix = group.Category.ToString();
            var mixes = _mixer.FindMatchingGroups(mix);
			if (mixes.Length == 0)
			{
				Debug.LogError($"Could not find any mixers of name {group.Category}");
				return;
			}
			source.outputAudioMixerGroup = mixes[0];
			source.spatialBlend = group.SpatialBlend;
			if (source.spatialBlend != 0) source.transform.localPosition = position;
			var data = group.RandomClip();
			source.clip = data.Clip;
			source.pitch = Random.Range(data.PitchVariation.Min, data.PitchVariation.Max);
			source.volume = volume * data.VolumeModifier;
			source.loop = true;
			source.Play();
		}
		public void StopLoopingSounds(SoundType type)
		{
			AudioGroup group = _library[type];
			foreach (var obj in _audioPool.Pool)
			{
				if (obj.gameObject.activeInHierarchy)
				{
					for (int i = 0; i < group.Length; i++)
					{
						if (obj.clip == group[i].Clip && obj.loop)
						{
							obj.gameObject.SetActive(false);
							return;
						}
					}
				}
			}
		}
		public void SetVolume(SoundCategory category, float volume)
		{
			if (!VolumeListContains(category))
			{
				_volumes.Add(new Volumes_DATA(category, volume));
			}
			else
			{
				GetVolumeList(category).volume = volume;
			}
			string name = category.ToString();
			if (!_mixer.SetFloat(name, volume))
			{
				Debug.LogError($"Mixer could not find an exposed parameter of name <color=#c7d13d>{name}</color>");
			}
		}
		public float GetVolume(SoundCategory category)
		{
			string name = category.ToString();
			if (_mixer.GetFloat(name, out float volume)) return volume;

			Debug.LogError($"Mixer could not find an exposed parameter of name <color=#c7d13d>{name}</color>");
			return 0f;
		}

		[Serializable]
		public class Volumes_DATA
		{
			public SoundCategory type;
			public float volume;
			public Volumes_DATA()
			{
				type = SoundCategory.Master;
				volume = 1f;
			}
			public Volumes_DATA(SoundCategory type, float volume)
			{
				this.type = type;
				this.volume = volume;
			}
		}
		private List<Volumes_DATA> _volumes = new List<Volumes_DATA>();
		private const string _VOLUMES_FILE = "Volumes.xml";
		private bool VolumeListContains(SoundCategory category)
		{
			for (int i = 0; i < _volumes.Count; i++)
			{
				if (_volumes[i].type == category) return true;
			}
			return false;
		}
		private Volumes_DATA GetVolumeList(SoundCategory category)
		{
			for (int i = 0; i < _volumes.Count; i++)
			{
				if (_volumes[i].type == category) return _volumes[i];
			}
			return null;
		}
	}
}

