using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour {

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float defaultSensitivityX = 15F;
    public float defaultSensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    private float sensitivityX;
    private float sensitivityY;

    float rotationY = 0F;

    // Aiming
    public float zoomFOV = 30f;
    private Camera cam;
    private float normal;
    private bool aiming = false;

    void Start() {
        cam = GetComponentInChildren<Camera>();
        normal = cam.fieldOfView;
        sensitivityX = defaultSensitivityX;
        sensitivityY = defaultSensitivityY;

        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;
        Rigidbody modelRigidbody = GetComponentInChildren<Rigidbody>();
        if (modelRigidbody)
            modelRigidbody.freezeRotation = true;
    }

    void Update() {
        if (GameEngine.paused) return;

        if (axes == RotationAxes.MouseXAndY) {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        } else if (axes == RotationAxes.MouseX) {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        } else {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }
    }

    private void SetAiming(bool aiming) {
        this.aiming = aiming;
        if (aiming) {
            sensitivityX = defaultSensitivityX / 10f;
            sensitivityY = defaultSensitivityY / 10f;
        } else {
            sensitivityX = defaultSensitivityX;
            sensitivityY = defaultSensitivityY;
        }
    }

    public void Aim(bool aiming) {
        if (this.aiming != aiming) {
            SetAiming(aiming);
        }
        if (aiming) {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoomFOV, Time.deltaTime * 10f);
        } else {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, normal, Time.deltaTime * 10f);
        }
    }

}