

	
var	hitPrefab : GameObject;	


function OnTriggerEnter () {
	
	
	//hit effect
	Instantiate( hitPrefab, transform.position, transform.rotation );
}

function OnCollisionEnter(collision : Collision){
    var contact : ContactPoint = collision.contacts[0];
    Instantiate( hitPrefab, contact.point.position, contact.point.rotation );   //contact.point; //this is the Vector3 position of the point of contact
}