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
    private float yaw = 0f;

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
        yaw = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, yaw, 0);
        if (Mathf.Abs(yaw) > 1) {
            anim.SetBool("Turning", true);
        } else {
            anim.SetBool("Turning", false);
        }
        anim.SetFloat("deltaYaw", yaw);

        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -pitchRange, pitchRange);
        //cam.transform.localRotation = Quaternion.Euler(pitch, 0, 0);

        // Fire
        if (Input.GetButton("Fire1")) {
            anim.SetBool("Shooting", true);
        } else {
            anim.SetBool("Shooting", false);
        }

        // Movement
        float movementSpeed = walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) {
            movementSpeed = runSpeed;
            anim.SetBool("Running", true);
            anim.SetBool("Shooting", false);
        } else {
            anim.SetBool("Running", false);
        }

        input.Set(Input.GetAxis("Horizontal") * movementSpeed, Input.GetAxis("Vertical") * movementSpeed);
        verticalVelocity += 2 * Physics.gravity.y * Time.deltaTime;

        // Jump
        if (cc.isGrounded && Input.GetButtonDown("Jump")) {
            verticalVelocity = jumpSpeed;
            anim.SetTrigger("Jump");
        }



        Vector3 velocity = new Vector3(input.x, verticalVelocity, input.y);
        anim.SetFloat("VelocityX", velocity.x);
        anim.SetFloat("VelocityZ", velocity.z);
        if (input.x != 0 || input.y != 0)
            anim.SetFloat("Speed", 1);
        else
            anim.SetFloat("Speed", 0);
        velocity = transform.rotation * velocity;

        cc.Move(velocity * Time.deltaTime);
    }

}
