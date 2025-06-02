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

	public void SunShotAttack(InputAction.CallbackContext ctx)
	{
		if(ctx.performed)
		{
			SunShot.Invoke();
		}
	}

	public static UnityEvent Headbutt = new();

	public void HeadbuttAttack(InputAction.CallbackContext ctx)
	{
		if (ctx.performed)
		{
			Headbutt.Invoke();
		}
	}

	public static UnityEvent TilPlant = new();

	public void TilOrPlant(InputAction.CallbackContext ctx) 
	{ 
		if(ctx.performed)
		{
			TilPlant.Invoke();
		} 
	}

	public static UnityEvent WaterCan = new();

	public void WaterPlant(InputAction.CallbackContext ctx)
	{
		if (ctx.performed)
		{
			WaterCan.Invoke();
		}
	}

	public static UnityEvent Next = new();

	public void NextPlant(InputAction.CallbackContext ctx)
	{
		if (ctx.performed)
		{
			Next.Invoke();
		}
	}

	public static UnityEvent Previous = new();

	public void PreviousPlant(InputAction.CallbackContext ctx)
	{
		if (ctx.performed)
		{
			Previous.Invoke();
		}
	}
}
