var destination : Transform; 

function OnTriggerEnter (other : Collider) { 
    //if (other.CompareTag ("Player")) { 
       other.transform.position = destination.transform.position;
    //} 
}