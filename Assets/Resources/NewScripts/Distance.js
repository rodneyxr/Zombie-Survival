
var target : Transform; 
private var myTransform : Transform; 
var distanceTextMesh : TextMesh;
var currentDistance : String = "";


function Start () {
    myTransform = transform;
}

function Update () {
	if(target){
	   var distance = (target.position - myTransform.position).magnitude;
	   currentDistance = distance.ToString("F0");
	   distanceTextMesh.text = currentDistance;
	}   
}