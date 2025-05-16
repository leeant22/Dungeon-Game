using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera view;
    private float xRotation = 0f;
    public float xSens = 1000f;
    public float ySens = 1000f;
    public void Look(Vector2 input)
    {
        xRotation -= input.y * ySens * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -80f, 30f);
        view.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * (input.x * Time.deltaTime) * xSens);
    }
}
