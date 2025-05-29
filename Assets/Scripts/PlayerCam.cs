using UnityEngine;
public class PlayerCam : MonoBehaviour
{
    public float xSens = 50f;
    public float ySens = 50f;
    public Transform orientation;
    float xRotation;
    float yRotation;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * xSens * 0.1f;
        float mouseY = Input.GetAxisRaw("Mouse Y") * ySens * 0.1f;
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 50f);
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}