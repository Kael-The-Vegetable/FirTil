using System;
using UnityEngine;

[Serializable]
public class PathSpriteState
{
	[SerializeField] Sprite _sprite;
	[SerializeField] bool _connectedNW;
	[SerializeField] bool _connectedNE;
	[SerializeField] bool _connectedE;
	[SerializeField] bool _connectedSE;
	[SerializeField] bool _connectedSW;
	[SerializeField] bool _connectedW;

	public Sprite Sprite => _sprite;

	public bool Equal(bool[] dirs)
	{
		if (dirs.Length != 6) return false;

		return _connectedNW == dirs[0]
			&& _connectedNE == dirs[1]
			&& _connectedE  == dirs[2] 
			&& _connectedSE == dirs[3]
			&& _connectedSW == dirs[4]
			&& _connectedW  == dirs[5];
	}
}
