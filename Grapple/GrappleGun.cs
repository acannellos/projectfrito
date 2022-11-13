using UnityEngine;

public class GrappleGun : MonoBehaviour {

    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, camera, player;
    public float maxDistance = 200f;
    private SpringJoint joint;

    public bool isSwinging;

    public RaycastHit predictionHit;
    public float predictionSphereCastRadius;
    public Transform predictionPoint;

    void Awake() {
        lr = GetComponent<LineRenderer>();
    }

    void Update() {
        GetGrapplePoint();
        if (Input.GetMouseButtonDown(0)) {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0)) {
            StopGrapple();
        }
        if (joint != null) ShortenRope();
    }

    void LateUpdate() {
        DrawRope();
    }

    void StartGrapple() {
        
        if (predictionHit.point == Vector3.zero) return;
        
        //RaycastHit hit;
        //if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappleable)) {
        grapplePoint = predictionHit.point;
        joint = player.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = grapplePoint;

        float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

        //The distance grapple will try to keep from grapple point. 
        joint.maxDistance = distanceFromPoint * 0.8f;
        joint.minDistance = distanceFromPoint * 0.25f;

        //Adjust these values to fit your game.
        joint.spring = 4.5f;
        joint.damper = 7f;
        joint.massScale = 4.5f;

        lr.positionCount = 2;
        currentGrapplePosition = gunTip.position;

        isSwinging = true;
        //}
    }

    void StopGrapple() {
        lr.positionCount = 0;
        Destroy(joint);
        isSwinging = false;
    }

    private Vector3 currentGrapplePosition;
    
    void DrawRope() {
        //If not grappling, don't draw rope
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);
        
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    void ShortenRope() {
        if(Input.GetKey(KeyCode.Space)) {
            Vector3 directionToPoint = grapplePoint - player.position;
            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;
        }
    }

    public bool IsGrappling() {
        return joint != null;
    }

    public void GetGrapplePoint() {
        if (joint != null) return;

        RaycastHit sphereCastHit;
        Physics.SphereCast(camera.position, predictionSphereCastRadius, camera.forward, out sphereCastHit, maxDistance, whatIsGrappleable);

        RaycastHit raycastHit;
        Physics.Raycast(camera.position, camera.forward, out raycastHit, maxDistance, whatIsGrappleable);

        Vector3 realHitPoint;

        if (raycastHit.point != Vector3.zero) {
            realHitPoint = raycastHit.point;
        }
        else if (sphereCastHit.point != Vector3.zero) {
            realHitPoint = sphereCastHit.point;
        }
        else {
            realHitPoint = Vector3.zero;
        }

        if (realHitPoint != Vector3.zero) {
            predictionPoint.gameObject.SetActive(true);
            predictionPoint.position = realHitPoint;
        }
        else {
            predictionPoint.gameObject.SetActive(false);
        }
        
        predictionHit = raycastHit.point == Vector3.zero ? sphereCastHit : raycastHit;
    }
}
