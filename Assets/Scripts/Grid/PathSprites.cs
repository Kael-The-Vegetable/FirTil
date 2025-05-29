using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PathSprites", menuName = "Scriptable Objects/PathSprites")]
public class PathSprites : ScriptableObject
{
	#region Additional Private Classes
	[Serializable]
	public class SetOfConnections
	{
		public int numberOfConnections = 0;
		[SerializeField] private List<PathSpriteState> _states = new List<PathSpriteState>();

		public PathSpriteState GetProperState(bool[] connections)
		{
			for (int i = 0; i < _states.Count; i++)
			{
				if (_states[i].Equal(connections)) return _states[i];
			}
			return null;
		}
	}
	#endregion

	[SerializeField] private SetOfConnections[] _setOfConnections = new SetOfConnections[7];

	[ContextMenu("Validate")]
	public void Validate()
	{
		try
		{
			ComboHelper(6, new bool[0]);
			Debug.Log("<color=green>All States Displayed In Connections");
		}
		catch (ArgumentNullException e)
		{
			Debug.LogError(e.Message);
		}
	}
	private void ComboHelper(int numOfSwitches, bool[] currentStates, int currentOn = 0)
	{
		if (numOfSwitches < 1)
		{
			SetOfConnections group = null;
			for (int i = 0; i < _setOfConnections.Length; i++)
			{
				if (_setOfConnections[i].numberOfConnections == currentOn)
				{
					group = _setOfConnections[i];
					break;
				}
			}
			if (group == null) throw new ArgumentNullException($"No set of connections found with {currentOn} connections!");

			_ = group.GetProperState(currentStates) 
				?? throw new ArgumentNullException($"No {currentOn} connection state found with this pattern => {currentStates.ToCommaSeparatedString()}");
			return;
		}

		bool[] newStates = new bool[currentStates.Length + 1];
		currentStates.CopyTo(newStates, 0);
		for (int i = 0; i < 2; i++)
		{
			newStates[^1] = Convert.ToBoolean(i);
				
			ComboHelper(numOfSwitches - 1, newStates, newStates[^1] ? currentOn + 1 : currentOn);
		}
	}

	public Sprite GetProperSprite(PathSection section, List<PathSection> neighbours)
	{
		HexCoord origin = section.GridCoordinates;
		bool[] connections = new bool[6];
		int numOfConnections = 0;

		for (int i = 0; i < neighbours.Count; i++)
		{

			HexCoord difference = neighbours[i].GridCoordinates - origin;
			if (difference.Magnitude() != 1) continue;
			if (difference.q == 0)
			{
				connections[difference.r == 1 ? 3 : 0] = true; // 0 if -1 or 3 if 1
			} else if (difference.r == 0)
			{
				connections[difference.q == 1 ? 5 : 2] = true; // 2 if -1 or 5 if 1
			} else if (difference.s == 0)
			{
				connections[difference.r == 1 ? 4 : 1] = true; // 1 if -1 or 4 if 1
			}
			numOfConnections++;
		} // gathering neighbours into a bool array identical to how it is usually stored.

		SetOfConnections group = null;
		for (int i = 0; i < _setOfConnections.Length; i++)
		{
			if (_setOfConnections[i].numberOfConnections == numOfConnections)
			{
				group = _setOfConnections[i];
				break;
			}
		}
		if (group == null) return null;

		return group.GetProperState(connections)?.Sprite;
	}
}
