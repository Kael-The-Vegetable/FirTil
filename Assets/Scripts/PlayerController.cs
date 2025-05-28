using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	private Vector2 _moveDir;
	private Rigidbody2D _rb;

	[SerializeField] private float _moveForce = 100;
	[SerializeField] private float _maxSpeed = 4;

	[SerializeField] GameObject sunShot;
	[SerializeField] Transform sunShotTransform;

	bool canFire = true;
	float timer;
	[SerializeField] float timeBetweenFiring;
	private void Start()
	{
		InputManager.OnMove.AddListener(Move);
		InputManager.SunShot.AddListener(ShootSunShot);
		InputManager.TilPlant.AddListener(TilPlant);
		InputManager.Next.AddListener(NextPlant);
		InputManager.Previous.AddListener(PreviousPlant);
		_rb = GetComponent<Rigidbody2D>();
	}
	private void OnDestroy()
	{
		InputManager.OnMove?.RemoveListener(Move);
		InputManager.SunShot?.RemoveListener(ShootSunShot);
		InputManager.TilPlant?.RemoveListener(TilPlant);
		InputManager.Next?.RemoveListener(NextPlant);
		InputManager.Previous?.RemoveListener(PreviousPlant);
	}

	private void FixedUpdate()
	{
		if (_moveDir != Vector2.zero)
		{
			// TODO: make movement diagonal feel good by changing vector from diagonal to scaled diagonal (more towards down / up less left / right)
			Vector2 levelOfForce = Vector2.one;
			Vector2 movementScalar = GridRatio.Instance.MovementScalar;

			if ((_rb.linearVelocity + _moveDir).sqrMagnitude >= _rb.linearVelocity.sqrMagnitude)
			{ // force won't help stop target
				levelOfForce = new Vector2(
					Mathf.Clamp01((_maxSpeed - _rb.linearVelocity.magnitude) / _maxSpeed) * movementScalar.x,
					Mathf.Clamp01((_maxSpeed - _rb.linearVelocity.magnitude) / _maxSpeed) * movementScalar.y);
			}
			Debug.Log(levelOfForce + " | " + movementScalar);
			_rb.AddForce(_moveForce * _moveDir * movementScalar * levelOfForce);
		}

		if (!canFire)
		{
			timer += Time.deltaTime;
			if (timer > timeBetweenFiring)
			{
				canFire = true;
				timer = 0;
			}
		}
	}

	private void Move(Vector2 dir) => _moveDir = dir;

	private void ShootSunShot()
	{
		if(canFire)
		{
			canFire = false;
			Instantiate(sunShot, sunShotTransform.position, Quaternion.identity);
		}
		
	}

	private void TilPlant()
	{
		//Tils or plants based off the state of the tile
	}

	private void NextPlant()
	{
		//Goes forward in the list
	}

	private void PreviousPlant()
	{
		//Goes back in the list
	}
}
