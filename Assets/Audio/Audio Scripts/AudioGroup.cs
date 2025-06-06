using System;
using UnityEngine;

namespace AudioSystem
{
	[CreateAssetMenu(fileName = "AudioGroup", menuName = "Scriptable Objects/Audio/AudioGroup")]
	[Serializable]
	public class AudioGroup : ScriptableObject
	{
		[SerializeField] private SoundCategory _category;
		public SoundCategory Category => _category;

		[SerializeField] private SoundType _type;
		public SoundType Type => _type;

		[SerializeField, Range(0, 1)] private float spatialBlend;
		public float SpatialBlend => spatialBlend;

		[Serializable]
		public class ClipData
		{
			[field: SerializeField] public AudioClip Clip { get; private set; }
			[field: SerializeField] public FloatRange PitchVariation { get; private set; }
			[field: SerializeField, Range(0, 1)] public float VolumeModifier { get; private set; }
		}
		[SerializeField] private ClipData[] _clips;

		public int Length => _clips.Length;
		public ClipData this[int index] => _clips[index];
		public ClipData RandomClip() => _clips[UnityEngine.Random.Range(0, _clips.Length)];
	}
}
