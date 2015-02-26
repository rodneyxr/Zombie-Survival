using UnityEngine;
using System.Collections;

public class FPSController : MonoBehaviour {

    // the animator controller
    private Animator anim;

    // character Controller
    private CharacterController cc;

    // Look
    public float mouseSensitivity = 7.0f;
    private Camera cam;
    private float pitch = 0f;
    private float pitchRange = 60.0f;

    // Movement
    private Vector2 input = new Vector2();
    public float walkSpeed = 5.0f;
    public float runSpeed = 10.0f;
    private float jumpSpeed = 8f;
    private float verticalVelocity = 0f;

    // Use this for initialization
    void Start() {
        Screen.lockCursor = true;
        cc = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update() {
        // Rotation
        float yaw = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, yaw, 0);

        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -pitchRange, pitchRange);
        //cam.transform.localRotation = Quaternion.Euler(pitch, 0, 0);

        // Movement
        float movementSpeed = walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
            movementSpeed = runSpeed;

        input.Set(Input.GetAxis("Horizontal") * movementSpeed, Input.GetAxis("Vertical") * movementSpeed);
        //float forwardSpeed = Input.GetAxis("Vertical") * movementSpeed;
        //float sideSpeed = Input.GetAxis("Horizontal") * movementSpeed;
        verticalVelocity += 2 * Physics.gravity.y * Time.deltaTime;

        // Jump
        if (cc.isGrounded && Input.GetButtonDown("Jump")) {
            verticalVelocity = jumpSpeed;
        }

        Vector3 velocity = new Vector3(input.x, verticalVelocity, input.y);
        velocity = transform.rotation * velocity;

        cc.Move(velocity * Time.deltaTime);
        Animate();
    }

    void Animate() {
        if (input.y > 0) {
            anim.Play("standing_walk_forward_1");
        } else if (input.y < 0) {
            anim.Play("standing_walk_back_1");
        }
        if (input.x > 0) {
            anim.Play("standing_walk_right_1");
        } else if (input.x < 0) {
            anim.Play("standing_walk_left_1");
        }
    }
}
