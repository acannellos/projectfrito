using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    InputManager _input;
    Rigidbody body;

    Vector3 velocity, desiredVelocity;

    [SerializeField] Transform playerSpace;

    //float walkSpeed = 1.5f;
    float runSpeed = 5f;
    float sprintSpeed = 7f;
    float maxAcceleration = 20f;
    [SerializeField] float jumpHeight = 1f;
    
    public bool isGrounded;

    void Awake() {
        _input = FindObjectOfType<InputManager>();
        body = GetComponent<Rigidbody>();
    }

	public void SetVelocity() {

        // add walkSpeed
        float targetSpeed = _input.isSprinting ? sprintSpeed : runSpeed;

        desiredVelocity = playerSpace.TransformDirection(_input.moveInput.x, 0f, _input.moveInput.y) * targetSpeed;
    }

	public void HandleMovement() {
        velocity = body.velocity;
        float maxSpeedChange = maxAcceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        if (_input.desiredJump) {
			_input.desiredJump = false;
			Jump();
		}

        body.velocity = velocity;
        isGrounded = false;
    }
    
	private void Jump() {
        if (isGrounded){
            velocity.y += Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
        }
    }

    void OnCollisionEnter() {
		isGrounded = true;
	}

    void OnCollisionStay() {
		isGrounded = true;
	}
}
