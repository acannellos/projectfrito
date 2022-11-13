using UnityEngine;

public class CameraManager : MonoBehaviour {

    InputManager input;

	[SerializeField] Transform focus;
	[SerializeField, Range(1f, 12f)] float sensX = 6f;
	[SerializeField, Range(1f, 12f)] float sensY = 6f;
	//private float rotationSpeed = 15f;
	private float rotationSpeed = 200f;

    private float distance = 8f;
	[SerializeField, Min(1f)]  private float distanceMax = 16f;
	[SerializeField, Min(1f)] private float distanceMin = 4f;
	[SerializeField, Range(1, 10f)]  private float scrollMultiplier = 4;

    [SerializeField, Min(0f)] float focusRadius = 1f;
    [SerializeField, Range(0f, 1f)] float focusCentering = 0.5f;
    Vector3 focusPoint;
    Vector2 orbitAngles = new Vector2(20f, 0f);

	[SerializeField, Range(-89f, 89f)] float minVerticalAngle = -10f, maxVerticalAngle = 60f;

	void Awake() {
		focusPoint = focus.position;
		transform.localRotation = Quaternion.Euler(orbitAngles);
        input = FindObjectOfType<InputManager>();
	}
	
    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
    }

    public void HandleCamera() {
		UpdateFocusPoint();
		UpdateDistance();

		Quaternion lookRotation;

		if (ManualRotation()) {
			ConstrainAngles();
			lookRotation = Quaternion.Euler(orbitAngles);
		}
		else {
			lookRotation = transform.localRotation;
		}

		Vector3 lookDirection = lookRotation * Vector3.forward;
        Vector3 lookPosition = focusPoint - lookDirection * distance;
		transform.SetPositionAndRotation(lookPosition, lookRotation);
	}

    private void UpdateFocusPoint() {
		Vector3 targetPoint = focus.position;

		if (focusRadius > 0f) {
			float focusCheck = Vector3.Distance(targetPoint, focusPoint);
            float t = 1f;
            if (focusCheck > 0.01f && focusCentering > 0f) {
				t = Mathf.Pow(1f - focusCentering, Time.unscaledDeltaTime);
			}
			if (focusCheck > focusRadius) {
                t = Mathf.Min(t, focusRadius / focusCheck);
			}
            focusPoint = Vector3.Lerp(targetPoint, focusPoint, t);
		}
		else {
			focusPoint = targetPoint;
		}
	}

    private void UpdateDistance() {
		distance -= input.scrollInput * scrollMultiplier;
		distance = Mathf.Clamp(distance, distanceMin, distanceMax);
	}

	bool ManualRotation() {
		
		if (input.lookInput.magnitude !=0) {
			input.lookInput.x *= sensX;
			input.lookInput.y *= sensY;
			orbitAngles += rotationSpeed * Time.unscaledDeltaTime * input.lookInput;
			return true;
		}
		return false;
	}

	void OnValidate() {
		if (maxVerticalAngle < minVerticalAngle) {
			maxVerticalAngle = minVerticalAngle;
		}
	}

	private void ConstrainAngles() {
		orbitAngles.x =	Mathf.Clamp(orbitAngles.x, minVerticalAngle, maxVerticalAngle);

		if (orbitAngles.y < 0f) {
			orbitAngles.y += 360f;
		}
		else if (orbitAngles.y >= 360f) {
			orbitAngles.y -= 360f;
		}
	}
}
