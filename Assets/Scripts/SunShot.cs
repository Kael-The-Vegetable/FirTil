using UnityEngine;
using UnityEngine.InputSystem;

public class SunShot : MonoBehaviour
{
    Vector3 mousePos;
    Camera mainCam;
    private Rigidbody2D rb;
    [SerializeField] float force;
    void Start()
    {
		mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        mousePos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3 direc = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;
        rb.linearVelocity  = new Vector2(direc.x, direc.y).normalized * force;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
