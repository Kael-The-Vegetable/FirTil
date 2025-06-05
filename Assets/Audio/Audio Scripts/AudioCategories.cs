using System;

namespace AudioSystem
{
	[Flags]
	public enum SoundType
	{
		None = 0,
		UI = 1 << 0,
		Human = 1 << 1,
		Friendly = 1 << 2,
		Hostile = 1 << 3,
		Music =  1 << 4,
		Magic = 1 << 5,
		Physical = 1 << 6,
		Ding = 1 << 7,
		Victory = 1 << 8,
		Defeat = 1 << 9,
		Water = 1 << 10,
		Fire = 1 << 11
	}
	public enum SoundCategory
	{
		Master,
		Music,
		SFX,
		Environmental
	}
}
