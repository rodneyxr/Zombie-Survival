
var maximumHitPoints = 100.0;
var hitPoints = 100.0;
var deadReplacement : Rigidbody;
var GOPos : GameObject;
var scoreManager : ScoreManager;

function Start(){	
	scoreManager = gameObject.Find("ScoreManager").GetComponent("ScoreManager");
}

function ApplyDamage (damage : float) {
	if (hitPoints <= 0.0)
		return;

	// Apply damage
	hitPoints -= damage;
	scoreManager.DrawCrosshair();
	// Are we dead?
	if (hitPoints <= 0.0)
		Replace(); 
}

function Replace() {

	// If we have a dead barrel then replace ourselves with it!
	if (deadReplacement) {
		var dead : Rigidbody = Instantiate(deadReplacement, GOPos.transform.position, GOPos.transform.rotation);
		scoreManager.addScore(20);
		// For better effect we assign the same velocity to the exploded barrel
		dead.rigidbody.velocity = rigidbody.velocity;
		dead.angularVelocity = rigidbody.angularVelocity;
    }
	// Destroy ourselves
	Destroy(gameObject);
}



