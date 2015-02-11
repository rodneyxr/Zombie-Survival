
@script ExecuteInEditMode()



private var thePower : float;
private var increasing : boolean = false;
private var shooting : boolean = false;

//speed to increment the bar
var barSpeed : float = 25;
var ball : Rigidbody;
var spawnPos : Transform;
var shotForce : float = 5;
var throwSound : AudioClip;


function Update () {
	
		//if we are not currently shooting and Jump key is pressed down
		if(!shooting && Input.GetButtonDown("Fire")){
			//set the increasing part of Update() below to start adding power
			increasing = true;
		}
		
		// detect if Jump key is released and then call the Shoot function, passing current 
		// value of 'thePower' variable into its 'power' argument
		if(!shooting && Input.GetButtonUp("Fire")){
			//reset increasing to stop charge of the power bar
			increasing = false;	
			//call the custom function below with current value of thePower fed to its argument
			Shoot(thePower);	
		}
		
		if(increasing){
			//add to thePower variable using Time.deltaTime multiplied by barSpeed
			thePower += Time.deltaTime * barSpeed;

		
			
			//set the pitch of the audio tone to the power var but step it down with division
			audio.pitch = thePower/30;
		}
}

// start the 'Shoot' custom function, and establish a 
// float argument to recieve 'thePower' when function is called
function Shoot (power : float){
	shooting  = true;
	
	//play the sound of the cannon blast in a new object to avoid interfering
	//with the current sound assignment and loop setup
	AudioSource.PlayClipAtPoint(throwSound, transform.position);
	
	//create a ball, assign the newly created ball to a var called pFab
	var pFab : Rigidbody = Instantiate(ball, spawnPos.position, spawnPos.rotation);
	
	//find the forward direction of the object assigned to the spawnPos variable
	var fwd : Vector3 = spawnPos.forward;
	pFab.AddForce(fwd * power * shotForce);
	
	thePower = 0;
	
	//allow shooting to occur again
	shooting = false;
}

function OnGUI () {
	GUI.Label (Rect(40, Screen.height - 100,60,60)," Power: ");
	GUI.Label (Rect(100, Screen.height - 100,60,60),"" + thePower);
}

function DrawWeapon(){
	print ("drawing");
}