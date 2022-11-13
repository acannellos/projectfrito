using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    InputManager input;

    [SerializeField] Transform camera;
    Rigidbody rb;

    [Header("Movement")]
    public float moveSpeed = 20f;
    [Range(1f, 50f)] public float maxSpeed = 20f;
    public float extraGravity = 10;
    public float counterMovement = 0.175f;
    private float threshold = 0.01f;

    [Header("Jump")]
    public bool isGrounded;
    private bool readyToJump = true;
    private bool readyToFreeze = true;
    private float jumpCooldown = 0.25f;
    [Range(1f, 10f)] public float jumpForce = 1f;
    private float jumpMultiplier = 5f;

    private float freezeCooldown = 1f;

    public Vector3 moveDirection;
    private Vector2 mag;
    private float lookAngle;
    private float moveAngle;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    [SerializeField] float rotationSpeed = 15f;

    private void Awake() {
        input = FindObjectOfType<InputManager>();
        rb = GetComponent<Rigidbody>();
    }

    public void HandleMovement() {

        moveDirection = camera.forward * input.moveInput.y;
        moveDirection = moveDirection + camera.right * input.moveInput.x;

        rb.AddForce(Vector3.down * extraGravity);

        mag = FindVelRelativeToLook();

        CounterMovement(input.moveInput.x, input.moveInput.y, mag);

        HandleRotation();
        
        if (readyToJump && input.jumpInput) Jump();
        if (readyToFreeze && input.freezeInput) Freeze();

        SpeedControl();

        float multiplier = 1f;
        if (!isGrounded) {
            multiplier = 0.5f;
        }

        rb.AddForce(moveDirection * moveSpeed * multiplier);

        isGrounded = false;
    }

    private Vector2 FindVelRelativeToLook() {
        
        lookAngle = camera.transform.eulerAngles.y;
        moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;
        
        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);
        
        return new Vector2(xMag, yMag);
    }
    
    private void SpeedControl() {
        if (input.moveInput.x > 0 && mag.x > maxSpeed) input.moveInput.x = 0;
        if (input.moveInput.x < 0 && mag.x < -maxSpeed) input.moveInput.x = 0;
        if (input.moveInput.y > 0 && mag.y > maxSpeed) input.moveInput.y = 0;
        if (input.moveInput.y < 0 && mag.y < -maxSpeed) input.moveInput.y = 0;
    }

    private void CounterMovement(float x, float y, Vector2 mag) {

        if (!isGrounded || input.jumpInput) return;

        if (Mathf.Abs(mag.x) > threshold && Mathf.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0)) {
            rb.AddForce(moveSpeed * camera.transform.right * -mag.x * counterMovement);
        }
        if (Mathf.Abs(mag.y) > threshold && Mathf.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0)) {
            rb.AddForce(moveSpeed * camera.transform.forward * -mag.y * counterMovement);
        }
        
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed) {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    private void HandleRotation() {
        if (input.moveInput != Vector2.zero) {
            Vector3 targetDirection = Vector3.zero;
            targetDirection = moveDirection;
            targetDirection.y = 0;

            if (targetDirection == Vector3.zero) {
                targetDirection = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            transform.rotation = playerRotation;
		}
    }

    private void Jump() {
        readyToJump = false;
        input.jumpInput = false;

        //rb.AddForce(Vector2.up * jumpForce * jumpMultiplier);
        rb.AddForce(Vector2.up * jumpForce * jumpMultiplier, ForceMode.Impulse);


        Vector3 vel = rb.velocity;
        if (rb.velocity.y < 0.5f)
            rb.velocity = new Vector3(vel.x, 0, vel.z);
        else if (rb.velocity.y > 0) 
            rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);
        
        Invoke(nameof(ResetJump), jumpCooldown);
    }

    
    private void Freeze() {
            readyToFreeze = false;
            input.freezeInput = false;

            rb.velocity = Vector3.zero;
            
            Invoke(nameof(ResetFreeze), freezeCooldown);
    }
    
    private void ResetJump() {
        readyToJump = true;
    }
        
    private void ResetFreeze() {
        readyToFreeze = true;
    }

    void OnCollisionEnter() {
		isGrounded = true;
	}

    void OnCollisionStay() {
		isGrounded = true;
	}
}
