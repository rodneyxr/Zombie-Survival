using UnityEngine;
using System.Collections;

public class FPSController : MonoBehaviour {

    // character Controller
    private CharacterController characterController;

    // Look
    public float mouseSensitivity = 7.0f;
    private Camera camera;
    private float pitch = 0f;
    private float pitchRange = 60.0f;

    // Movement
    private Vector2 input = new Vector2();
    public float walkSpeed = 5.0f;
    public float runSpeed = 10.0f;
    private float jumpSpeed = 8f;
    private float verticalVelocity = 0f;

    // Double-Click
    private float clickInterval = .150f; // in seconds (100ms)
    private float lastTime = -1f;


    // Use this for initialization
    void Start() {
        Screen.lockCursor = true;
        characterController = GetComponent<CharacterController>();
        camera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update() {
        // Rotation
        float yaw = Input.GetAxis("Mouse X") * mouseSensitivity;
        if (Input.GetKeyDown(KeyCode.E)) {
            float currentTime = Time.time;
            if (currentTime - lastTime <= clickInterval) {
                yaw = 90;
            }
            lastTime = currentTime;
        }
        //else if (Input.GetKeyDown (KeyCode.Q))
        //		yaw = -90;
        transform.Rotate(0, yaw, 0);

        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -pitchRange, pitchRange);
        camera.transform.localRotation = Quaternion.Euler(pitch, 0, 0);

        // Movement
        float movementSpeed = walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
            movementSpeed = runSpeed;

        input.Set(Input.GetAxis("Horizontal") * movementSpeed, Input.GetAxis("Vertical") * movementSpeed);
        //float forwardSpeed = Input.GetAxis("Vertical") * movementSpeed;
        //float sideSpeed = Input.GetAxis("Horizontal") * movementSpeed;
        verticalVelocity += 2 * Physics.gravity.y * Time.deltaTime;

        // Jump
        if (characterController.isGrounded && Input.GetButtonDown("Jump")) {
            verticalVelocity = jumpSpeed;
        }

        Vector3 velocity = new Vector3(input.x, verticalVelocity, input.y);
        velocity = transform.rotation * velocity;

        characterController.Move(velocity * Time.deltaTime);
    }

    void Animate() {
    }
}
