/**
*  This script must be placed on vehicle model! 
* Simply create box (turn off renderer and make it as trigger), 
* after that you must make this box as a child of vehicle (parent)! 
* Place somewhere on the vehicle model, where you will activate it!
* Script made by OMA [www.armedunity.com]
**/
private var weaponCamera : GameObject;  			// drag and drop player camera from Hierarchy to Inspector window!
var vehicleCam : GameObject;			
var vehicleCameraTarget : Transform;
var vehicle : GameObject; 
private var Player;
var GetOutPosition : Transform;  					// Empty game object, where player will get out of the vehicle
var VehicleControllScript : String = "ScriptName"; 	// Just write script name, which controls vehicle movement (controller script).  
private var opened : boolean = false;
private var waitTime : float = 1; 					// leave it as 1 
private var temp : boolean = false;
private var mainCamera : GameObject;	

function Start () {
	vehicleCam.camera.enabled = false;
	vehicle.GetComponent(VehicleControllScript).controlsEnabled = false;
	vehicleCam.GetComponent(AudioListener).enabled = false;  
}

function Update() {
	
	Player = GameObject.FindWithTag("Player"); 		
	mainCamera = GameObject.FindWithTag("MainCamera");
	weaponCamera = GameObject.FindWithTag("WeaponCamera");
	
	
    if ((Input.GetKeyDown("e")) && opened && !temp){
		GetOut();
        opened = false;
	    temp = false;
    }
}

function Action (){
	if (!opened && !temp){
        GetIn();
	    opened = true;
	    temp = true;
	    yield WaitForSeconds(waitTime);
	    temp = false;
    }
}


function GetIn() {
	var changeTarget : VehicleCamera = vehicleCam.transform.GetComponent("VehicleCamera");
	changeTarget.target = vehicleCameraTarget;
	//Player.BroadcastMessage("LightOff");
	
	// Disable all script behaviours on Player (Essentially deactivating player control)
	var coms : Component[] = Player.GetComponentsInChildren(MonoBehaviour);
	for (var b in coms) {
		var p : MonoBehaviour = b as MonoBehaviour;
		if (p)
			p.enabled = false;
	}
	
	// Disable all renderers
	var gos = Player.GetComponentsInChildren(Renderer);
	for( var go : Renderer in gos){
		go.enabled = false;
    }
	
	// Disable all cameras
	var cam = Player.GetComponentsInChildren(Camera);
	for( var c : Camera in cam){
		c.enabled = false;
    }
	
	Player.transform.parent = vehicle.transform;
	Player.transform.position = vehicleCameraTarget.transform.position;
	Player.rigidbody.isKinematic = true;
	Player.collider.isTrigger = true;
	weaponCamera.camera.enabled = false;
	weaponCamera.GetComponent(AudioListener).enabled = false;
	mainCamera.camera.enabled = false;
	vehicleCam.camera.enabled = true;
	vehicle.GetComponent(VehicleControllScript).controlsEnabled = true;
	vehicleCam.GetComponent(AudioListener).enabled = true;
}


function GetOut() {
	
	// Enable all script behaviours on Player (Essentially deactivating player control)
	var coms : Component[] = Player.GetComponentsInChildren(MonoBehaviour);
	for (var b in coms) {
		var p : MonoBehaviour = b as MonoBehaviour;
		if (p)
			p.enabled = true;
	}
	
	// Enable all renderers
	var gos = Player.GetComponentsInChildren(Renderer);
	for( var go : Renderer in gos){
		go.enabled = true;
    }
	
	// Enable all cameras
	var cam = Player.GetComponentsInChildren(Camera);
	for( var c : Camera in cam){
		c.enabled = true;
    }

	Player.transform.parent = null;
	Player.rigidbody.isKinematic = false;
	Player.collider.isTrigger = false;
	Player.transform.position = GetOutPosition.transform.position;
	weaponCamera.camera.enabled = true;
	weaponCamera.GetComponent(AudioListener).enabled = true;
	mainCamera.camera.enabled = true;
	vehicleCam.camera.enabled = false;
	vehicleCam.GetComponent(AudioListener).enabled = false;
	vehicle.GetComponent(VehicleControllScript).controlsEnabled = false;
}
