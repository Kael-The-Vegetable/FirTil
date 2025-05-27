using UnityEngine;
using UnityEngine.InputSystem;

public class TempMove : MonoBehaviour
{
    [SerializeField] float speed = 0.5f;
    private Rigidbody2D rb;
    private Vector2 input;

    public InputActionAsset InputActions;
    private InputAction moveAction;
    private InputAction lookAction;
    private Vector2 moveValue;
    private Vector2 lookValue;


	private void Awake()
	{
        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");
	}
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

	private void OnEnable()
	{
        InputActions.FindActionMap("Player").Enable();
	}

	private void OnDisable()
	{
		InputActions.FindActionMap("Player").Disable();
	}

	// Update is called once per frame
	void Update()
    {
        moveValue = moveAction.ReadValue<Vector2>();
        lookValue = lookAction.ReadValue<Vector2>();

        moveValue.Normalize();
	}

	private void FixedUpdate()
	{
		rb.linearVelocity = moveValue * speed;
	}
}
