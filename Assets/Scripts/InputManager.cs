using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent (typeof(PlayerInput))]
public class InputManager : PersistentSingleton<InputManager>
{
	public static UnityEvent<Vector2> OnMove = new();
	protected override void Initialize() { }

	public void Move(InputAction.CallbackContext ctx)
	{
		OnMove.Invoke(ctx.ReadValue<Vector2>());
	}

	public static UnityEvent SunShot = new();

	public void Attack(InputAction.CallbackContext ctx)
	{
		if(ctx.performed)
		{
			SunShot.Invoke();
		}
	}

	public static UnityEvent TilGround = new();

	public static UnityEvent PlantSeed = new();
}
