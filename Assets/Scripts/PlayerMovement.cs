using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController control;
    private Vector3 velocity;
    public float speed = 5f;
    
    void Start()
    {
        control = GetComponent<CharacterController>();
    }

    void Update()
    {

    }

    public void MovePlayer(Vector2 input)
    {
        Vector3 dir = Vector3.zero;
        dir.x = input.x;
        dir.z = input.y;
        control.Move(transform.TransformDirection(dir) * speed * Time.deltaTime);
    }
}
