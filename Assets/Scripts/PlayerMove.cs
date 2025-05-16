using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed;
    public Transform orientation;
    float hInput;
    float vInput;
    Vector3 moveDirection;
    Rigidbody rb;
    public float playerHeight;
    public LayerMask whatIsGround;
    bool isGrounded;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    public KeyCode jumpKey = KeyCode.Space;
    public AudioClip footstepClip;
    private AudioSource footstepAudio;
    public float stepInterval = 2f;
    private float stepTimer;
    public AudioClip jumpStartClip;
    public AudioClip jumpLandClip;
    private bool wasGroundedLastFrame;
    private bool hasLandedOnce = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;

        footstepAudio = gameObject.AddComponent<AudioSource>();
        footstepAudio.clip = footstepClip;
        footstepAudio.loop = false;
        footstepAudio.playOnAwake = false;
    }

    private void Update()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 2.1f, whatIsGround);

        MyInput();
        SpeedControl();
        if (isGrounded)
        {
            rb.drag = groundDrag;

            if (!wasGrounded && hasLandedOnce)
            {
                footstepAudio.PlayOneShot(jumpLandClip);
            }
            else if (!hasLandedOnce)
            {
                hasLandedOnce = true;
            }

            bool isMoving = hInput != 0 || vInput != 0;
            if (isMoving)
            {
                stepTimer -= Time.deltaTime;
                if (stepTimer <= 0f)
                {
                    footstepAudio.PlayOneShot(footstepClip);
                    stepTimer = stepInterval;
                }
            }
            else
            {
                stepTimer = 0f;
            }
        }
        else
        {
            rb.drag = 0;
            stepTimer = 0f;
        }

        wasGroundedLastFrame = isGrounded;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        hInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(jumpKey) && readyToJump && isGrounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * vInput + orientation.right * hInput;
        if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 5f, ForceMode.Force);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 5f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (velocity.magnitude > moveSpeed)
        {
            Vector3 limitVelocity = velocity.normalized * moveSpeed;
            rb.velocity = new Vector3(limitVelocity.x, rb.velocity.y, limitVelocity.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        footstepAudio.PlayOneShot(jumpStartClip);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}