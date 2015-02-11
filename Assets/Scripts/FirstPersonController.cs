using UnityEngine;
using System.Collections;

public class FirstPersonController : MonoBehaviour {
    // character Controller
    CharacterController characterController;

    // Look
    public float mouseSensitivity = 7.0f;
    float pitch = 0f;
    float pitchRange = 60.0f;

    // Movement
    private const float walkSpeed = 5.0f;
    private const float runSpeed = 10.0f;
    float jumpSpeed = 8f;
    float verticalVelocity = 0f;

    // Double-Click
    float clickInterval = .150f; // in seconds (100ms)
    float lastTime = -1f;


    // Use this for initialization
    void Start() {
        Screen.lockCursor = true;
        characterController = GetComponent<CharacterController>();
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
        Camera.main.transform.localRotation = Quaternion.Euler(pitch, 0, 0);

        // Movement
        float movementSpeed = walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
            movementSpeed = runSpeed;

        float forwardSpeed = Input.GetAxis("Vertical") * movementSpeed;
        float sideSpeed = Input.GetAxis("Horizontal") * movementSpeed;
        verticalVelocity += 2 * Physics.gravity.y * Time.deltaTime;

        // Jump
        if (characterController.isGrounded && Input.GetButtonDown("Jump")) {
            verticalVelocity = jumpSpeed;
        }

        Vector3 velocity = new Vector3(sideSpeed, verticalVelocity, forwardSpeed);
        velocity = transform.rotation * velocity;


        characterController.Move(velocity * Time.deltaTime);
    }
}
