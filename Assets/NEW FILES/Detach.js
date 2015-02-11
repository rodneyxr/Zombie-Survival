#pragma strict
var unparentWheels : GameObject[];
var hitPoints : float = 100;
var explosion : Transform;
var body : GameObject;
var trigger : GameObject;

function ApplyDamage (damage : float) {
	if (hitPoints <= 0.0)
		return;

	// Apply damage
	hitPoints -= damage;
	
	// Are we dead?
	if (hitPoints <= 0.0) Detonate();
}


function Detonate () {
	
	var coms : Component[] = GetComponentsInChildren(MonoBehaviour);
	for (var b in coms) {
		var p : MonoBehaviour = b as MonoBehaviour;
		if (p)
			p.enabled = false;
	}
	trigger.SetActive (false);
	for(var i : int = 0; i < unparentWheels.length; i++){
		unparentWheels[i].transform.parent = null;
		unparentWheels[i].AddComponent ("MeshCollider");
		unparentWheels[i].AddComponent ("Rigidbody");
		unparentWheels[i].transform.position.y += 1;
		
	}
	var aaa = Instantiate(explosion, body.transform.position, body.transform.rotation);
	transform.DetachChildren();
	
}
