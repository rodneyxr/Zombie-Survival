
var linkedLight : Light;

function Update () {
    if(Input.GetKeyDown("f")){
        linkedLight.enabled = !linkedLight.enabled;
   }
}

function LightOff (){
	linkedLight.enabled = false;
}	

