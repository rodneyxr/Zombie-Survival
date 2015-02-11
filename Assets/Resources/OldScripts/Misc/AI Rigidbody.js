// These variables are for adjusting in the inspector how the object behaves
var maxSpeed = 7.000;
var force = 8.000;

// Don't let the Physics Engine rotate this physics object so it doesn't fall over when running
function Awake (){
    rigidbody.freezeRotation = true;
}

// This is called every physics frame
function FixedUpdate (){
    if(rigidbody.velocity.magnitude < maxSpeed){
        rigidbody.AddForce (transform.rotation * Vector3.forward);
        rigidbody.AddForce (transform.rotation * Vector3.right);
    }
 } 