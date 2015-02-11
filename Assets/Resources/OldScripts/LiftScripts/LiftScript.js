
var platform : Transform;
var player : Transform;


function OnTriggerEnter () {
	var temp = GameObject.Find("Player");
	player = temp.transform;
	var GO = platform;
	var GO1 = player; 
	GO1.transform.parent = GO.transform;
}

function OnTriggerExit () {
	var GO = platform;
	var GO1 = player; 
	GO1.transform.parent = null;
}