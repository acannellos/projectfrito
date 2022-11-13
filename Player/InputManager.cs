using UnityEngine;
using i = UnityEngine.Input;

public class InputManager : MonoBehaviour {

	public Vector2 moveInput;
	public Vector2 lookInput;
	public float scrollInput;
	public bool jumpInput;
	public bool sprintInput;
	public bool sneakInput;
	public bool freezeInput;

	KeyCode sprintKey = KeyCode.LeftShift;
	KeyCode sneakKey = KeyCode.LeftControl;
	KeyCode freezeKey = KeyCode.X;

	public void HandleInputs() {
		BasicInputs();
	}

	private void BasicInputs() {
		moveInput = new Vector2(i.GetAxis("Horizontal"), i.GetAxis("Vertical"));
		moveInput = Vector2.ClampMagnitude(moveInput, 1f);

		lookInput = new Vector2(i.GetAxis("Mouse Y"), i.GetAxis("Mouse X"));
		scrollInput = i.GetAxis("Mouse ScrollWheel");

		jumpInput |= i.GetButtonDown("Jump");
		sprintInput = i.GetKey(sprintKey);
		sneakInput = i.GetKey(sneakKey);

		freezeInput = i.GetKey(freezeKey);
	}
}
