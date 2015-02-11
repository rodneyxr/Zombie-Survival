
    var speed : float  = 0;
    private var lastPosition : Vector3 = Vector3.zero;
	
function FixedUpdate(){
    speed = (transform.position - lastPosition).magnitude;
    lastPosition = transform.position;
}

function OnTriggerEnter (other : Collider) {
	if(speed > 0.2){
		if(other.CompareTag ("Enemy") || other.CompareTag ("Metal")){
			other.gameObject.BroadcastMessage("ApplyDamage", speed * 3000, SendMessageOptions.DontRequireReceiver);
		}	
	}	
}