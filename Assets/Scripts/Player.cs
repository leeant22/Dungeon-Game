using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.WalkActions walk;
    private PlayerMovement movement;
    private PlayerLook look;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerInput = new PlayerInput();
        walk = playerInput.Walk;
        movement = GetComponent<PlayerMovement>();
        look = GetComponent<PlayerLook>();
        walk.Jump.performed += callback => movement.Jump();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        movement.MovePlayer(walk.Move.ReadValue<Vector2>());
    }

    void LateUpdate()
    {
        look.Look(walk.Look.ReadValue<Vector2>());
    }

    private void OnEnable() {
        walk.Enable();
    }

    private void OnDisable() {
        walk.Disable();
    }
}
