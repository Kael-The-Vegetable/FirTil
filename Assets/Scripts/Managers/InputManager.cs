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
		if (IsPaused()) return;

		OnMove.Invoke(ctx.ReadValue<Vector2>());
	}

	public static UnityEvent SunShot = new();

	public void SunShotAttack(InputAction.CallbackContext ctx)
	{
		if(ctx.performed && !IsPaused())
		{
			SunShot.Invoke();
		}
	}

	public static UnityEvent Headbutt = new();

	public void HeadbuttAttack(InputAction.CallbackContext ctx)
	{
		if (ctx.performed && !IsPaused())
		{
			Headbutt.Invoke();
		}
	}

	public static UnityEvent TilPlant = new();

	public void TilOrPlant(InputAction.CallbackContext ctx) 
	{ 
		if(ctx.performed && !IsPaused())
		{
			TilPlant.Invoke();
		} 
	}

	public static UnityEvent WaterCan = new();

	public void WaterPlant(InputAction.CallbackContext ctx)
	{
		if (ctx.performed && !IsPaused())
		{
			WaterCan.Invoke();
		}
	}

	public static UnityEvent Next = new();

	public void NextPlant(InputAction.CallbackContext ctx)
	{
		if (ctx.performed && !IsPaused())
		{
			Next.Invoke();
		}
	}

	public static UnityEvent Previous = new();

	public void PreviousPlant(InputAction.CallbackContext ctx)
	{
		if (ctx.performed && !IsPaused())
		{
			Previous.Invoke();
		}
	}

	public static UnityEvent Escape = new();

	public void OnEscape(InputAction.CallbackContext ctx)
	{
		if (ctx.performed)
		{
			Escape.Invoke();
		}
	}

	private bool IsPaused() => PauseManager.HasInstance && PauseManager.Instance.IsPaused;
}
