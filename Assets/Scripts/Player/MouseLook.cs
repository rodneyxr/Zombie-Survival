using UnityEngine;
using System.Collections;

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
    public Texture2D crosshairImage;
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

    void OnGUI() {
        if (GameEngine.paused) return;
        float scaleFactor = Mathf.Max(Screen.width, Screen.height) / 1080f;
        float shrinkFactor = cam.fieldOfView / normal;
        float imgWidth = (crosshairImage.width * scaleFactor) * shrinkFactor;
        float imgHeight = (crosshairImage.height * scaleFactor) * shrinkFactor;
        float xMin = (Screen.width / 2) - (imgWidth / 2);
        float yMin = (Screen.height / 2) - (imgHeight / 2);
        GUI.DrawTexture(new Rect(xMin, yMin, imgWidth, imgHeight), crosshairImage);
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