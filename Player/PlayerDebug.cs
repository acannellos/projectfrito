using TMPro;
using UnityEngine;

public class PlayerDebug : MonoBehaviour {

    [SerializeField] TextMeshProUGUI stateDisplay;
    [SerializeField] TextMeshProUGUI magDisplay;

    Rigidbody rb;
    PlayerState state;

    void Awake() {
        rb = FindObjectOfType<Rigidbody>();
        state = FindObjectOfType<PlayerState>();
    }

    void Update () {
        stateDisplay.text = state.movementState.ToString();
        magDisplay.SetText("{0:0}", rb.velocity.magnitude);
    }

}
