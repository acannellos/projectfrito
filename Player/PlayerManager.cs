using UnityEngine;

public class PlayerManager : MonoBehaviour {

    InputManager input;
    PlayerMovement movement;
    CameraManager camera;
    PlayerState state;

    void Awake() {
        input = FindObjectOfType<InputManager>();
        movement = FindObjectOfType<PlayerMovement>();
        camera = FindObjectOfType<CameraManager>();
        state = FindObjectOfType<PlayerState>();
    }

    void Update() {
        input.HandleInputs();
        state.HandleStates();
    }
    
    void FixedUpdate() {
        movement.HandleMovement();
    }

    void LateUpdate() {
        camera.HandleCamera();
    }
}
