using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAimRotation : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 mousePos;

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //seems to lag a bit
        mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3 rotation = mousePos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }
}
