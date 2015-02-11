var crouchSpeed = 3.0;
var walkSpeed = 6.0;
var runSpeed = 10.0;
var jumpSpeed = 6.0;
var climbSpeed = 6.0;
var gravity = 20.0;
var fallingDamageThreshold = 10.0;
var fallDamageMultiplier = 5.0;
var slideSpeed = 12.0;
var antiBumpFactor = .75;
var antiBunnyHopFactor = 1;
var airControl = false;

var slideWhenOverSlopeLimit = false;
var slideOnTaggedObjects = false;
var jumpAnimation : GameObject;
var playerWeapons : GameObject;
var fallDamage : AudioClip;

private var moveDirection = Vector3.zero;
private var grounded = false;
private var controller : CharacterController;
private var myTransform : Transform;
private var speed : float;
private var hit : RaycastHit;
private var fallStartLevel : float;
private var falling = false;
private var slideLimit : float;
private var rayDistance : float;
private var contactPoint : Vector3;
private var playerControl = false;
private var limitDiagonalSpeed = true;
private var jumpTimer : int;
private var mainCamera : GameObject;
private var weaponCamera : GameObject;
private var standartCamHeight : float =0.7;
private var crouchingCamHeight : float = 0.2;
private var crouching : boolean;
private var canSprint : boolean = true;

function Start () {
    controller = GetComponent(CharacterController);
	mainCamera = gameObject.FindWithTag("MainCamera");
	weaponCamera = gameObject.FindWithTag("WeaponCamera");
    myTransform = transform;
    speed = walkSpeed;
	crouching = true;
    rayDistance = controller.height * .5 + controller.radius;
    slideLimit = controller.slopeLimit - .1;
    jumpTimer = antiBunnyHopFactor;
    oldPos = transform.position;
	playerWeapons.animation.wrapMode = WrapMode.Loop;
}

function FixedUpdate() {
    var inputX = Input.GetAxis("Horizontal");
    var inputY = Input.GetAxis("Vertical");
    var inputModifyFactor = (inputX != 0.0 && inputY != 0.0 && limitDiagonalSpeed)? .7071 : 1.0;
   
    if (grounded) {
        var sliding = false;
        if (Physics.Raycast(myTransform.position, -Vector3.up, hit, rayDistance)) {
            if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit)
                sliding = true;
        }
        // However, just raycasting straight down from the center can fail when on steep slopes
        // So if the above raycast didn't catch anything, raycast down from the stored ControllerColliderHit point instead
        else {
            Physics.Raycast(contactPoint + Vector3.up, -Vector3.up, hit);
            if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit)
                sliding = true;
        }

        // If we were falling, and we fell a vertical distance greater than the threshold, run a falling damage routine
        if (falling) {
            falling = false;
            if (myTransform.position.y < fallStartLevel - fallingDamageThreshold)
                FallingDamageAlert (fallStartLevel - myTransform.position.y);
        }
        
		if(crouching){
            if (grounded && Input.GetButton("Run") && Input.GetKey("w") && canSprint)
				speed = runSpeed;
			    else 
			    speed = walkSpeed;
			}else{
			speed = crouchSpeed;
		}

        // If sliding (and it's allowed), or if we're on an object tagged "Slide", get a vector pointing down the slope we're on
        if ( (sliding && slideWhenOverSlopeLimit) || (slideOnTaggedObjects && hit.collider.tag == "Slide") ) {
            var hitNormal = hit.normal;
            moveDirection = Vector3(-hitNormal.x, hitNormal.y, hitNormal.z);
            Vector3.OrthoNormalize (hitNormal, moveDirection);
            moveDirection *= slideSpeed;
            playerControl = true;
        }
        // Otherwise recalculate moveDirection directly from axes, adding a bit of -y to avoid bumping down inclines
        else {
            moveDirection = Vector3(inputX * inputModifyFactor, -antiBumpFactor, inputY * inputModifyFactor);
            moveDirection = myTransform.TransformDirection(moveDirection) * speed;
            playerControl = true;
        }

        // Jump! But only if the jump button has been released and player has been grounded for a given number of frames
        if (!Input.GetButton("Jump"))
            jumpTimer++;
        else if (jumpTimer >= antiBunnyHopFactor) {
            moveDirection.y = jumpSpeed;
			jumpAnimation.animation.Play("Jump");
            jumpTimer = 0;
        }
    }
    else {
        // If we stepped over a cliff or something, set the height at which we started falling
        if (!falling) {
            falling = true;
            fallStartLevel = myTransform.position.y;
        }
       
        // If air control is allowed, check movement but don't touch the y component
        if (airControl && playerControl) {
            moveDirection.x = inputX * speed * inputModifyFactor;
            moveDirection.z = inputY * speed * inputModifyFactor;
            moveDirection = myTransform.TransformDirection(moveDirection);
        }
    }

    // Apply gravity
    moveDirection.y -= gravity * Time.deltaTime;

    // Move the controller, and set grounded true or false depending on whether we're standing on something
    grounded = (controller.Move(moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;
}

function Update () {
    
	if (Input.GetButtonDown("Crouch")) {
        if(crouching){
		    controller.height = 1;
	        controller.center = Vector3 (0, -0.5, 0);
	        mainCamera.transform.localPosition.y = crouchingCamHeight;
			weaponCamera.transform.localPosition.y = crouchingCamHeight;
	        crouching = false;
	        return;
	    }
	    if(!crouching)
			Crouch();
    }
	

	if (controller.isGrounded && controller.velocity.magnitude < 7 && controller.velocity.magnitude > 5 && !Input.GetButton("Fire2")){
		playerWeapons.animation.CrossFade("Walk");
	}else if (controller.isGrounded && controller.velocity.magnitude < 11 && controller.velocity.magnitude > 9){
		playerWeapons.animation.CrossFade("Run");
	}else if (controller.isGrounded && controller.velocity.magnitude < 4 && controller.velocity.magnitude > 2 && Input.GetButton("Fire2"))
	    playerWeapons.animation.CrossFade("CrouchAim");
	else
	    playerWeapons.animation.CrossFade("Idle");
	
}


function Crouch () {
	controller.height = 2.0;
	controller.center = Vector3 (0, 0, 0);
	mainCamera.transform.localPosition.y = standartCamHeight;
	weaponCamera.transform.localPosition.y = standartCamHeight;
    crouching = true;
    }

function OnControllerColliderHit (hit : ControllerColliderHit) {
    contactPoint = hit.point;
	if (hit.collider.gameObject.tag == "ladder"){
      
    OnLadder = true;
      
    if(Input.GetAxis("Vertical")){
         
      moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),0 );
      moveDirection *= climbSpeed;
      moveDirection = transform.TransformDirection(moveDirection);
      
      transform.position += moveDirection;
      
    } else {
         onLadder = false;
        }
    }
	
	if (hit.gameObject.tag == "Rifle"){
		Destroy(hit.gameObject);
		playerWeapons.SendMessage("PickupRifle");
	}
	
	if (hit.gameObject.tag == "Launcher"){
		Destroy(hit.gameObject);
		playerWeapons.SendMessage("PickupLauncher");
	}
	
	if (hit.gameObject.tag == "Pistol"){
		Destroy(hit.gameObject);
		playerWeapons.SendMessage("PickupPistol");
	}
	
	if (hit.gameObject.tag == "Sniper"){
		Destroy(hit.gameObject);
		playerWeapons.SendMessage("PickupSniper");
	}
}

function FallingDamageAlert (fallDistance : float) {
    Debug.Log ("Ouch! Fell " + fallDistance + " units!");
    audio.PlayOneShot(fallDamage, 1.0 / audio.volume);
    BroadcastMessage ("PlayerDamage",fallDistance * fallDamageMultiplier);	
}


function Sprinting (){
canSprint = true;
}

function normalSpeed (){
canSprint = false;
}

@script RequireComponent(CharacterController)