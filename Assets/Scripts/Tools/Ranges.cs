using System;
using UnityEngine;

public enum Inclusivity
{
	MaxExclusive, MinExclusive, AllExclusive, AllInclusive
}

[Serializable]
public struct IntRange
{
	[SerializeField] private int _min, _max;

	public int Min
	{
		get => _min;
		set
		{
			if (value > _max)
			{
				(_min, _max) = (_max, value);
			}
		}
	}
	public int Max
	{
		get => _max;
		set
		{
            if (value < _min)
            {
				(_max, _min) = (_min, value);
            }
        }
	}

	public int Random(Inclusivity i = Inclusivity.AllInclusive)
	{
		switch (i)
		{
			case Inclusivity.MaxExclusive:
				return UnityEngine.Random.Range(Min, Max);
			case Inclusivity.MinExclusive:
				return UnityEngine.Random.Range(Max, Min);
			case Inclusivity.AllExclusive:
				if (Min == Max) return 0;
				return UnityEngine.Random.Range(Min + 1, Max);
			case Inclusivity.AllInclusive:
				return UnityEngine.Random.Range(Min, Max + 1);
			default:
				return 0; // should never occur
		}
	}
}

[Serializable]
public struct FloatRange
{
	[SerializeField] private float _min;
	[SerializeField] private float _max;
	public float Min
	{
		readonly get => _min;
		set
		{
			if (value > _max)
			{
				_min = _max;
				_max = value;
			}
		}
	}
	public float Max
	{
		readonly get => _max;
		set
		{
			if (value < _min)
			{
				_max = _min;
				_min = value;
			}
		}
	}
	public FloatRange(float min, float max)
	{
		(_min, _max) = max >= min ? (min, max) : (max, min);
	}
	public readonly float Random() => UnityEngine.Random.Range(_min, _max);
	public readonly bool Contains(float value) => _min <= value && value <= _max;
}