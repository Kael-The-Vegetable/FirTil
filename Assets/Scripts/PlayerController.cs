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

			if ((_rb.linearVelocity + _moveDir).sqrMagnitude >= _rb.linearVelocity.sqrMagnitude)
			{ // force won't help stop target
				float speedX = _maxSpeed * GridRatio.Instance.MovementScalar.x;
				float speedY = _maxSpeed * GridRatio.Instance.MovementScalar.y;
				levelOfForce = new Vector2(
					Mathf.Clamp01((speedX - Mathf.Abs(_rb.linearVelocityX)) / speedX),
					Mathf.Clamp01((speedY - Mathf.Abs(_rb.linearVelocityY)) / speedY));
			}

			_rb.AddForce(_moveForce *  _moveDir * levelOfForce * GridRatio.Instance.MovementScalar);
		}
	}

	private void Move(Vector2 dir) => _moveDir = dir;
}
