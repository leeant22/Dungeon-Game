using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.WalkActions walk;
    private PlayerMovement movement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerInput = new PlayerInput();
        walk = playerInput.Walk;
        movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        movement.MovePlayer(walk.Move.ReadValue<Vector2>());
    }

    private void OnEnable() {
        walk.Enable();
    }

    private void OnDisable() {
        walk.Disable();
    }
}
