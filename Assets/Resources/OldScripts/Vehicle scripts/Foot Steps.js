/**
* Script made by OMA [www.oma.netau.net]
**/

var concrete : AudioClip[];
var wood : AudioClip[];
var dirt : AudioClip[];
var metal : AudioClip[];
private var step : boolean = true;
var audioStepLengthWalk : float = 0.45;
var audioStepLengthRun : float = 0.25;


function OnControllerColliderHit (hit : ControllerColliderHit) {
var controller : CharacterController = GetComponent(CharacterController);

if (controller.isGrounded && controller.velocity.magnitude < 7 && controller.velocity.magnitude > 5 && hit.gameObject.tag == "Concrete"  && step == true || controller.isGrounded && controller.velocity.magnitude < 7 && controller.velocity.magnitude > 5 && hit.gameObject.tag == "Untagged" && step == true ) {
		WalkOnConcrete();
	} else if (controller.isGrounded && controller.velocity.magnitude > 8 && hit.gameObject.tag == "Concrete" && step == true || controller.isGrounded && controller.velocity.magnitude > 8 && hit.gameObject.tag == "Untagged" && step == true) {
		RunOnConcrete();
	} else if (controller.isGrounded && controller.velocity.magnitude < 7 && controller.velocity.magnitude > 5 && hit.gameObject.tag == "Wood" && step == true) {
		WalkOnWood();
	} else if (controller.isGrounded && controller.velocity.magnitude > 8 && hit.gameObject.tag == "Wood" && step == true) {
		RunOnWood();
	} else if (controller.isGrounded && controller.velocity.magnitude < 7 && controller.velocity.magnitude > 5 && hit.gameObject.tag == "Dirt" && step == true) {
		WalkOnDirt();
	} else if (controller.isGrounded && controller.velocity.magnitude > 8 && hit.gameObject.tag == "Dirt" && step == true) {
		RunOnDirt();
	} else if (controller.isGrounded && controller.velocity.magnitude < 7 && controller.velocity.magnitude > 5 && hit.gameObject.tag == "Metal" && step == true) {
		WalkOnMetal();
	} else if (controller.isGrounded && controller.velocity.magnitude > 8 && hit.gameObject.tag == "Metal" && step == true) {
		RunOnMetal();		
	}	
}

/////////////////////////////////// CONCRETE ////////////////////////////////////////
function WalkOnConcrete() {
	step = false;
	audio.clip = concrete[Random.Range(0, concrete.length)];
	audio.volume = .1;
	audio.Play();
	yield WaitForSeconds (audioStepLengthWalk);
	step = true;
}

function RunOnConcrete() {
	step = false;
	audio.clip = concrete[Random.Range(0, concrete.length)];
	audio.volume = .3;
	audio.Play();
	yield WaitForSeconds (audioStepLengthRun);
	step = true;
}	


////////////////////////////////// WOOD /////////////////////////////////////////////
function WalkOnWood() {
	step = false;
	audio.clip = wood[Random.Range(0, wood.length)];
	audio.volume = .1;
	audio.Play();
	yield WaitForSeconds (audioStepLengthWalk);
	step = true;
}

function RunOnWood() {
	step = false;
	audio.clip = wood[Random.Range(0, wood.length)];
	audio.volume = .3;
	audio.Play();
	yield WaitForSeconds (audioStepLengthRun);
	step = true;
}


/////////////////////////////////// DIRT //////////////////////////////////////////////
function WalkOnDirt() {
	step = false;
	audio.clip = dirt[Random.Range(0, dirt.length)];
	audio.volume = .1;
	audio.Play();
	yield WaitForSeconds (audioStepLengthWalk);
	step = true;
}

function RunOnDirt() {
	step = false;
	audio.clip = dirt[Random.Range(0, dirt.length)];
	audio.volume = .3;
	audio.Play();
	yield WaitForSeconds (audioStepLengthRun);
	step = true;
}


////////////////////////////////// METAL ///////////////////////////////////////////////
function WalkOnMetal() {	
	step = false;
	audio.clip = metal[Random.Range(0, metal.length)];
	audio.volume = .1;
	audio.Play();
	yield WaitForSeconds (audioStepLengthWalk);
	step = true;
}

function RunOnMetal() {
	step = false;
	audio.clip = metal[Random.Range(0, metal.length)];
	audio.volume = .3;
	audio.Play();
	yield WaitForSeconds (audioStepLengthRun);
	step = true;
}

@script RequireComponent(AudioSource)