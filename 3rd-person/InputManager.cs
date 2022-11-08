using UnityEngine;

public class InputManager : MonoBehaviour {

	public Vector2 moveInput;
	public Vector2 lookInput;
	public float scrollInput;
	public bool desiredJump;
	public bool isSprinting;
	//public bool isWalking;

	//public bool alwaysRun;
	//public bool autoRun;

	public void HandleInputs() {
		moveInput.x = Input.GetAxis("Horizontal");
		moveInput.y = Input.GetAxis("Vertical");
		moveInput = Vector2.ClampMagnitude(moveInput, 1f);

		lookInput.x = Input.GetAxis("Mouse Y");
		lookInput.y = Input.GetAxis("Mouse X");

		desiredJump |= Input.GetButtonDown("Jump");

		scrollInput = Input.GetAxis("Mouse Scrollwheel");

		ToggleSprint();

		//crouching = Input.GetKey(KeyCode.LeftShift);
	}

	private void ToggleSprint() {
		if (Input.GetButtonDown("Sprint") && isSprinting) {
			isSprinting = false;
		}
		else {
			isSprinting |= Input.GetButtonDown("Sprint");
		}
	}
}
