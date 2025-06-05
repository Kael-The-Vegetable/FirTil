using UnityEngine;

public class PathHighlight : MonoBehaviour
{
	[SerializeField] private PlayerController _player;

	private void Awake()
	{
		_player = _player != null ? _player : FindAnyObjectByType<PlayerController>();
	}
}
