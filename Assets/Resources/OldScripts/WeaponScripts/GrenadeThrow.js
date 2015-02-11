
var grenadeGUI : GUIText;
var projectile : Rigidbody;
var initialSpeed = 15.0;
var reloadTime = 0.5;
var ammoCount = 4;
private var lastShot = -10.0;
var launchPosition : GameObject;
//var animGL : GameObject;
var soundFire : AudioClip;
var start : boolean = true;


function Awake (){
Gui();
}


function Update () {
	
	if (Input.GetKeyUp("q")){
		Throw();
	}
}

function Throw () {
	// Did the time exceed the reload time?
	if (Time.time > reloadTime + lastShot && ammoCount > 0) {
		// create a new projectile, use the same position and rotation as the Launcher.
		var instantiatedProjectile : Rigidbody = Instantiate (projectile, launchPosition.transform.position, launchPosition.transform.rotation);
//		animGL.animation.Play("FireGL");	
//		audio.clip = soundFire;
//		audio.Play();
		// Give it an initial forward velocity. The direction is along the z-axis of the missile launcher's transform.
		instantiatedProjectile.velocity = transform.TransformDirection(Vector3 (0, 0, initialSpeed));

		// Ignore collisions between the missile and the character controller
		//Physics.IgnoreCollision(instantiatedProjectile.collider, transform.root.collider);

		lastShot = Time.time;
		ammoCount--;
		Gui();
	}
}


function Gui () {
    grenadeGUI.text ="Grenade:  " + ammoCount.ToString();
}