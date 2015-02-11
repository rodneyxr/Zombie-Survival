
var maximumHitPoints = 200.0;
var hitPoints : float;
var regenerationSpeed : float = 5;
var painLittle : AudioClip;
var painBig : AudioClip;
var die : AudioClip;
var deadReplacement : Rigidbody;
private var gotHitTimer = -1.0;
var healthGUI : GUITexture;
private var healthGUIWidth = 100.0;
var damage : GameObject;
var explShake : GameObject;


function Awake () {
	healthGUIWidth = healthGUI.pixelInset.width;
}

function PlayerDamage (damage : int) {
	if (hitPoints < 0.0)
		return;

	// Apply damage
	hitPoints -= damage;
	
	// Play pain sound when getting hit - but don't play so often
	if (Time.time > gotHitTimer && painBig && painLittle) {
		// Play a big pain sound
		if (hitPoints < maximumHitPoints * 0.2 || damage > 100) {
			audio.PlayOneShot(painBig, 1.0 / audio.volume);
			gotHitTimer = Time.time + Random.Range(painBig.length * 2, painBig.length * 3);
		} else {
			// Play a small pain sound
			audio.PlayOneShot(painLittle, 1.0 / audio.volume);
			gotHitTimer = Time.time + Random.Range(painLittle.length * 2, painLittle.length * 3);
		}
	}

	// Are we dead?
	if (hitPoints < 0.0)
		Die();
}

function Medic (medic : int){
if (hitPoints > maximumHitPoints)
	return;
	
	hitPoints += medic;
}

function Die () {
	if (die && deadReplacement)
		AudioSource.PlayClipAtPoint(die, transform.position);
	var dead : Rigidbody = Instantiate(deadReplacement, transform.position, transform.rotation);
	
	// Disable all script behaviours (Essentially deactivating player control)
	var coms : Component[] = GetComponentsInChildren(MonoBehaviour);
	for (var b in coms) {
		var p : MonoBehaviour = b as MonoBehaviour;
		if (p)
			p.enabled = false;
	}
    yield WaitForSeconds(2.0);
	LevelLoadFade.FadeAndLoadLevel(Application.loadedLevel, Color.black, 2.0);
}

function LateUpdate () {
	UpdateGUI();
}

function Update (){
//REGENERATION and damage effect
	if (hitPoints < 200.0  && hitPoints > 0.0) 
	hitPoints += Time.deltaTime * regenerationSpeed;
    damage.guiTexture.enabled = true;
    
	if (hitPoints > 198.0)
  	damage.guiTexture.enabled = false;
}

function Exploasion(){
explShake.animation.Play("exploasion");
}

function UpdateGUI () {
	var healthFraction = Mathf.Clamp01(hitPoints / maximumHitPoints);
	healthGUI.pixelInset.xMax = healthGUI.pixelInset.xMin + healthGUIWidth * healthFraction;
 }
