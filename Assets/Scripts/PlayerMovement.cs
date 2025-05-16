using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController control;
    private Vector3 velocity;
    public float speed = 10f;
    private bool isGrounded;
    public float gravity = -9.8f;
    public float jumpHeight = 3.0f;
    
    void Start()
    {
        control = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = control.isGrounded;
    }

    public void MovePlayer(Vector2 input)
    {
        Vector3 dir = Vector3.zero;
        dir.x = input.x;
        dir.z = input.y;
        control.Move(transform.TransformDirection(dir) * speed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }
        control.Move(velocity * Time.deltaTime);
    }

    public void Jump() {
        if (isGrounded) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}
