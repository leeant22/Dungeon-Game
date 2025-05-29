using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Transactions;
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
    private GameObject[] collectibles;
    private int score;
    public TextMeshProUGUI scoreText;
    private Vector3 startingPosition;
    public GameObject endTextDisplay;
    public TextMeshProUGUI endText;
    public GameObject againButton;
    private float walkSpeed = 15f;
    private float sprintSpeed = 30f;
    public KeyCode sprintKey = KeyCode.LeftShift;
    private float currentSpeed;
    private float fallMultiplier = 2f;
    private float lowJumpMultiplier = 4f;
    private int health;
    public TextMeshProUGUI healthText;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;

        footstepAudio = gameObject.AddComponent<AudioSource>();
        footstepAudio.clip = footstepClip;
        footstepAudio.loop = false;
        footstepAudio.playOnAwake = false;

        collectibles = GameObject.FindGameObjectsWithTag("Collectible");
        score = 0;
        scoreText.text = "Score: " + score.ToString();

        health = 5;
        healthText.text = "Health: " + health.ToString();

        startingPosition = transform.position;
        endTextDisplay.SetActive(false);
        againButton.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        bool wasGrounded = isGrounded;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 2.1f, whatIsGround);

        MyInput();
        // SpeedControl();
        if (isGrounded)
        {
            rb.linearDamping = groundDrag;

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
            rb.linearDamping = 0;
            stepTimer = 0f;
        }

        wasGroundedLastFrame = isGrounded;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        BetterJumpGravity();
    }

    private void MyInput()
    {
        hInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");

        currentSpeed = Input.GetKey(sprintKey) ? sprintSpeed : walkSpeed;

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
        moveDirection.Normalize();
        Vector3 targetVelocity = moveDirection * currentSpeed;
        Vector3 currentVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        Vector3 velocityChange = targetVelocity - currentVelocity;

        velocityChange.y = 0f;

        float smoothFactor = isGrounded ? 10f : 2f;
        rb.AddForce(velocityChange * smoothFactor, ForceMode.Acceleration);
        //if (isGrounded)
        //{
        //    rb.AddForce(moveDirection.normalized * moveSpeed * 5f, ForceMode.Force);
        //}
        //else if (!isGrounded)
        //{
        //    rb.AddForce(moveDirection.normalized * moveSpeed * 5f * airMultiplier, ForceMode.Force);
        //}
    }

    //private void SpeedControl()
    //{
    //    Vector3 velocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
    //    if (velocity.magnitude > moveSpeed)
    //    {
    //        Vector3 limitVelocity = velocity.normalized * moveSpeed;
    //        rb.linearVelocity = new Vector3(limitVelocity.x, rb.linearVelocity.y, limitVelocity.z);
    //    }
    //}

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        footstepAudio.PlayOneShot(jumpStartClip);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            other.gameObject.SetActive(false);
            score += 1;
            scoreText.text = "Score: " + score.ToString();
        }
        else if (other.CompareTag("Lava"))
        {
            PlayerDead();
        } 
        else if (other.CompareTag("Club"))
        {
            health -= 1;
            healthText.text = "Health: " + health.ToString();

            if (health <= 0)
            {
                PlayerDead();
            }
        }
    }

    private void PlayerDead()
    {
        endTextDisplay.SetActive(true);
        endText.text = "You died!";
        againButton.SetActive(true);
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;
    }

    public void ResetPlayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        score = 0;
        scoreText.text = "Score: " + score.ToString();
        endTextDisplay.SetActive(false);
        againButton.SetActive(false);
    }

    private void BetterJumpGravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.AddForce(Vector3.up * (fallMultiplier - 1) * Physics.gravity.y, ForceMode.Acceleration);
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetKey(jumpKey))
        {
            rb.AddForce(Vector3.up * (lowJumpMultiplier - 1) * Physics.gravity.y, ForceMode.Acceleration);
        }
    }
}