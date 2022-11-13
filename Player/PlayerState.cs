using UnityEngine;

public class PlayerState : MonoBehaviour {
    
    InputManager input;
    PlayerMovement movement;
    GrappleGun gg;

    public MovementState movementState;

    public enum MovementState {
        idle,
        running,
        midair,
        swinging
    }

    
    void Awake() {
        input = FindObjectOfType<InputManager>();
        movement = FindObjectOfType<PlayerMovement>();
        gg = FindObjectOfType<GrappleGun>();
    }

    public void HandleStates() {
        HandleMovementState();
    }

    private void HandleMovementState() {

        movementState = MovementState.idle;

        if (input.moveInput.magnitude != 0) {
            movementState = MovementState.running;
        }

        if (!movement.isGrounded){
            movementState = MovementState.midair;

            if (gg.isSwinging){
                movementState = MovementState.swinging;
            }
        }
    }
}