var hitPoints : float = 300.0;
var explosion : Transform;
private var callFunction : boolean = false;

function OnTriggerEnter (other : Collider) { 
    if (other.CompareTag ("Enemy")) { 
	   other.SendMessageUpwards("ApplyDamage", hitPoints, SendMessageOptions.DontRequireReceiver);
       Explosion();
	} 
	
	if (other.CompareTag ("Player")){
	other.SendMessageUpwards("PlayerDamage", hitPoints, SendMessageOptions.DontRequireReceiver);
    Explosion();
	}
}

function ApplyDamage(){
	yield WaitForSeconds(0.5);
	Explosion();
}

function Explosion(){
	if(callFunction)
	return;
	callFunction = true;
	yield WaitForSeconds(0.1);
	Instantiate (explosion, transform.position, transform.rotation);
	Destroy(gameObject);
}

