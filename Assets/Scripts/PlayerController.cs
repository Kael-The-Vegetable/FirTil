using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	private Vector2 _moveDir;
	private Rigidbody2D _rb;

	[SerializeField] private float _moveForce = 100;
	[SerializeField] private float _maxSpeed = 4;
	private void Start()
	{
		InputManager.OnMove.AddListener(Move);
		_rb = GetComponent<Rigidbody2D>();
	}
	private void OnDestroy()
	{
		InputManager.OnMove?.RemoveListener(Move);
	}

	private void FixedUpdate()
	{
		if (_moveDir != Vector2.zero)
		{
			Vector2 levelOfForce = Vector2.one;
			Vector2 movementScalar = GridRatio.Instance.MovementScalar;

			if ((_rb.linearVelocity + _moveDir).sqrMagnitude >= _rb.linearVelocity.sqrMagnitude)
			{ // force won't help stop target
				levelOfForce = new Vector2(
					Mathf.Clamp01((_maxSpeed - _rb.linearVelocity.magnitude) / _maxSpeed) * movementScalar.x,
					Mathf.Clamp01((_maxSpeed - _rb.linearVelocity.magnitude) / _maxSpeed) * movementScalar.y);
			}
			_rb.AddForce(_moveForce * _moveDir * movementScalar * levelOfForce);
		}
	}

	private void Move(Vector2 dir) => _moveDir = dir;
}
