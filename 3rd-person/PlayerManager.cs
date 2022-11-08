using UnityEngine;

public class PlayerManager : MonoBehaviour {

    CameraManager _camera;
    InputManager _input;
    PlayerMovement _movement;

    void Awake() {
        _camera = FindObjectOfType<CameraManager>();
        _input = FindObjectOfType<InputManager>();
        _movement = FindObjectOfType<PlayerMovement>();
    }

    void Start() {
        _camera.HideCursor();
    }

    void Update() {
        _input.HandleInputs();
        _movement.SetVelocity();
    }
    
    void FixedUpdate() {
        _movement.HandleMovement();
    }

    void LateUpdate() {
        _camera.HandleCamera();
    }
}
