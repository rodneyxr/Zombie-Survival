var maximumHitPoints = 100.0;
var hitPoints = 100.0;
var deadReplacement : Rigidbody;
private var gotHitTimer = -1.0;
var detonationDelay = 0.0;
var explosion : Transform;
var particle : GameObject;
var controllScript : HelicopterController;
var mySkin : GUISkin;
private var callFunction : boolean = false;

function Start(){
	var controllScript : HelicopterController = GetComponent(HelicopterController);
}

function Update (){
	if(hitPoints <= maximumHitPoints/2){
		particle.particleEmitter.emit = true;
		particle.GetComponent(AudioSource).enabled = true;
		controllScript.damaged = true;
		hitPoints -= Time.deltaTime*20;
		if (hitPoints <= 0.0)
		Invoke("Detonate", detonationDelay);
	}
}

function ApplyDamage (damage : float) {
	if (hitPoints <= 0.0)
		return;

	// Apply damage
	hitPoints -= damage;
	
	// Are we dead?
	if (hitPoints <= 0.0)
		Invoke("Detonate", detonationDelay);
}

function Detonate () {
	if(callFunction)
	return;
	callFunction = true;
	
	Instantiate(explosion, transform.position, transform.rotation);
	gameObject.SetActive(false);
	
	// If we have a dead barrel then replace ourselves with it!
	if (deadReplacement) {
		var dead : Rigidbody = Instantiate(deadReplacement, transform.position, transform.rotation);
    }
	// Destroy ourselves
	Destroy(transform.parent.gameObject);
}

function OnGUI(){
	GUI.skin = mySkin;
	var style1 = mySkin.customStyles[0];
	
	if(controllScript.controlsEnabled){
		GUI.Label (Rect(40, Screen.height - 50, 150, 80)," Helicopter Health: ");
		GUI.Label (Rect(170, Screen.height - 50, 150, 80), "" + hitPoints, style1);
	}
}

