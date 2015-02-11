
var explosionPrefab : Transform;
var rotationSpeed : float = 100.0; 


function Update () {
	transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);	
}

// A grenade
// - instantiates a explosion prefab when hitting a surface
// - then destroys itself

function OnCollisionEnter(collision : Collision) {
    // Rotate the object so that the y-axis faces along the normal of the surface
    var contact : ContactPoint = collision.contacts[0];
    var rot : Quaternion = Quaternion.FromToRotation(Vector3.up, contact.normal);
    var pos : Vector3 = contact.point;
    Instantiate(explosionPrefab, pos, rot);
    // Destroy the projectile
    Destroy (gameObject);
}