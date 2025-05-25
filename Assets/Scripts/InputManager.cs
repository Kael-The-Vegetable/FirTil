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
}
