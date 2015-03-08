using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    // the animator controller
    private Animator anim;
    private int hashSpeed = Animator.StringToHash("Speed");
    private int hashVelocityX = Animator.StringToHash("VelocityX");
    private int hashVelocityZ = Animator.StringToHash("VelocityZ");
    private int hashRunning = Animator.StringToHash("Running");
    private int hashTurning = Animator.StringToHash("Turning");
    private int hashDeltaYaw = Animator.StringToHash("deltaYaw");
    private int hashFire = Animator.StringToHash("Fire");
    private int hashJump = Animator.StringToHash("Jump");
    private int hashShoot = Animator.StringToHash("Shoot");

    // character Controller
    private CharacterController cc;
    private Player player;

    // the player's weapon
    public WeaponManager weapon;

    // Look
    public MouseLook mouseLook;
    public float defaultMouseSensitivity = 3.0f;
    private float mouseSensitivity;
    private float yaw = 0f;
    private bool aiming = false;


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
        mouseSensitivity = defaultMouseSensitivity;
    }

    // Update is called once per frame
    void Update() {
        // Pause
        if (Input.GetButtonDown("Cancel")) {
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

        // Aim
        if (Input.GetButton("Aim")) {
            aiming = true;
            mouseSensitivity = defaultMouseSensitivity / 10f;
        } else {
            aiming = false;
            mouseSensitivity = defaultMouseSensitivity;
        }
        mouseLook.Aim(aiming);

        // Rotation
        yaw = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, yaw, 0);
        if (Mathf.Abs(yaw) > 1) {
            anim.SetBool(hashTurning, true);
        } else {
            anim.SetBool(hashTurning, false);
        }
        anim.SetFloat(hashDeltaYaw, yaw);

        // Movement
        float movementSpeed;
        if (aiming)
            movementSpeed = walkSpeed / 2f;
        else
            movementSpeed = walkSpeed;
        if (!aiming && Input.GetKey(KeyCode.LeftShift)) {
            movementSpeed = runSpeed;
            anim.SetBool(hashRunning, true);
        } else {
            anim.SetBool(hashRunning, false);
        }

        // Repair Barricade
        if (player.CurrentBarricade != null && Input.GetKey(KeyCode.E)) {
            player.CurrentBarricade.Repair();
        } // Fire
        else if (movementSpeed <= walkSpeed) {
            bool fired = false;
            if (weapon.Gun.isAutomatic) {
                if (Input.GetButton("Fire1")) {
                    fired = weapon.Fire();
                }
            } else {
                if (Input.GetButtonDown("Fire1")) {
                    fired = weapon.Fire();
                }
            }
            if (fired) anim.SetTrigger(hashShoot);
        }

        input.Set(Input.GetAxis("Horizontal") * movementSpeed, Input.GetAxis("Vertical") * movementSpeed);
        verticalVelocity += 2 * Physics.gravity.y * Time.deltaTime;

        // Jump
        if (cc.isGrounded && Input.GetButtonDown("Jump")) {
            verticalVelocity = jumpSpeed;
            anim.SetTrigger(hashJump);
        }

        Vector3 velocity = new Vector3(input.x, verticalVelocity, input.y);
        anim.SetFloat(hashVelocityX, velocity.x);
        anim.SetFloat(hashVelocityZ, velocity.z);
        if (input.x != 0 || input.y != 0)
            anim.SetFloat(hashSpeed, 1);
        else
            anim.SetFloat(hashSpeed, 0);
        velocity = transform.rotation * velocity;

        cc.Move(velocity * Time.deltaTime);
    }

}
