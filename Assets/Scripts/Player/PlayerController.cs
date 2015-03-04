using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    // the animator controller
    private Animator anim;

    // character Controller
    private CharacterController cc;
    private Player player;

    // the player's weapon
    public Weapon weapon;

    // Look
    public float mouseSensitivity = 7.0f;
    private float yaw = 0f;

    // Movement
    private Vector2 input = new Vector2();
    public float walkSpeed = 5.0f;
    public float runSpeed = 10.0f;
    public float jumpSpeed = 8f;
    private float verticalVelocity = 0f;

    // Use this for initialization
    void Start() {
        Screen.lockCursor = true;
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update() {
        // Pause
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (GameEngine.paused) {
                print("Player: Unpause");
                GameEngine.paused = false;
                Screen.lockCursor = true;
            } else {
                print("Player: Pause");
                GameEngine.paused = true;
                Screen.lockCursor = false;
            }
        }
        if (GameEngine.paused) return;

        // Rotation
        yaw = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, yaw, 0);
        if (Mathf.Abs(yaw) > 1) {
            anim.SetBool("Turning", true);
        } else {
            anim.SetBool("Turning", false);
        }
        anim.SetFloat("deltaYaw", yaw);

        // Movement
        float movementSpeed = walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) {
            movementSpeed = runSpeed;
            anim.SetBool("Running", true);
            anim.SetBool("Shooting", false);
        } else {
            anim.SetBool("Running", false);
        }

        // Repair Barricade
        if (player.CurrentBarricade != null && Input.GetKey(KeyCode.E)) {
            player.CurrentBarricade.Repair();
        } // Fire
        else if (movementSpeed <= walkSpeed && Input.GetButton("Fire1") && weapon.Fire()) {
            anim.SetBool("Shooting", true);
        } else {
            anim.SetBool("Shooting", false);
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
