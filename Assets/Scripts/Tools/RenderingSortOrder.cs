using UnityEngine;

public class RenderingSortOrder : MonoBehaviour
{
	[SerializeField] private SpriteRenderer _renderer;
	[SerializeField] private bool _isStatic = false;
	private Camera _camera;

	private void Awake()
	{
		_camera = Camera.main;
		if (_renderer == null) _renderer = GetComponent<SpriteRenderer>();
	}
	private void Start()
	{
		_renderer.sortingOrder = (int)(-10 * _camera.WorldToScreenPoint(transform.position).y);
	}

	private void Update()
	{
		if (!_isStatic)
		{ _renderer.sortingOrder = (int)(-10 * _camera.WorldToScreenPoint(transform.position).y); }
	}
}
