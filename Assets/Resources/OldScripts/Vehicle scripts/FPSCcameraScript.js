/**
*  Script written by OMA [www.armedunity.com]
**/

var maxRayDistance = 2.0; 
var layerMask : LayerMask;
var mySkin : GUISkin;
var showGui : boolean = false;

function Update() {
	var direction = gameObject.transform.TransformDirection(Vector3.forward);
	var hit : RaycastHit;
	var position = transform.position;
	if (Physics.Raycast(position, direction, hit, maxRayDistance, layerMask.value)) {
		showGui = true;
		if(Input.GetButtonDown("Use")) {
			var target = hit.collider.gameObject;
			target.BroadcastMessage("Action");
		}		
	}else{
		showGui = false;
	}
}

function OnGUI(){
	GUI.skin = mySkin;
	if(showGui){
		GUI.Label(Rect(Screen.width - (Screen.width/1.7),Screen.height - (Screen.height/1.4),800,100),"Press key >>E<< to Use");	
	}
}