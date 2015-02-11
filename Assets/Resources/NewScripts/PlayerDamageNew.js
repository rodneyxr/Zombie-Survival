/**
*  Script written by OMA [www.armedunity.com]
**/

@script ExecuteInEditMode()

var hitPoints : float;
var regenerationSkill : int;
var shieldSkill : int;
var painSound : AudioClip;
var die : AudioClip;
var deadReplacement : Transform;
var mySkin : GUISkin;
var explShake : GameObject;
private var radar : GameObject;
var maxHitPoints : int;
var damageTexture : Texture;
private var time : float = 0.0;
private var alpha : float;
private var callFunction : boolean = false;
private var scoreManager : ScoreManager;

function Start(){
	maxHitPoints = hitPoints;
	alpha = 0;
}

function Update(){
    if (time > 0){ 
        time -= Time.deltaTime;
    }
    alpha = time;
	if( hitPoints <= maxHitPoints){
		hitPoints += Time.deltaTime * regenerationSkill;
	}

	if( hitPoints > maxHitPoints){
		var convertToScore : float = hitPoints - maxHitPoints;
		scoreManager = gameObject.Find("ScoreManager").GetComponent("ScoreManager");
		scoreManager.addScore(convertToScore);
		hitPoints = maxHitPoints;
	}	
}

function PlayerDamage (damage : int) {
	if (hitPoints < 0.0)
		return;
		
	damage -= shieldSkill;
	
	if(damage > 0){
		// Apply damage
		hitPoints -= damage;
		audio.PlayOneShot(painSound, 1.0 / audio.volume);
		time = 2.0;		
	

		// Are we dead?
		if (hitPoints <= 0.0)
		Die();
	}else{
		damage = 0;
	}	
}

//Picking up MedicKit
function Medic (medic : int){
	
	hitPoints += medic;
	
	if(hitPoints > maxHitPoints)
	hitPoints = maxHitPoints;
}

function Die () {
	if(callFunction)
	return;
	callFunction = true;
	
	if (die && deadReplacement)
		AudioSource.PlayClipAtPoint(die, transform.position);

	// Disable all script behaviours (Essentially deactivating player control)
	var coms : Component[] = GetComponentsInChildren(MonoBehaviour);
	for (var b in coms) {
		var p : MonoBehaviour = b as MonoBehaviour;
		if (p)
			p.enabled = false;
	}
	// Disable all renderers
	var gos = GetComponentsInChildren(Renderer);
	for( var go : Renderer in gos){
		go.enabled = false;

    }
	if(radar != null){
		radar = gameObject.FindWithTag("Radar");
		radar.gameObject.SetActive(false);
	}
	Instantiate(deadReplacement, transform.position, transform.rotation);
    yield WaitForSeconds(4.5);
	//Destroy (transform.root.gameObject);
	LevelLoadFade.FadeAndLoadLevel(Application.loadedLevel, Color.black, 2.0);
}


function OnGUI () {
    //GUI.skin = mySkin;
	//var style1 = mySkin.customStyles[0];
	//GUI.Label (Rect(40, Screen.height - 50,60,60)," Health: ");
	//GUI.Label (Rect(100, Screen.height - 50,60,60),"" +hitPoints.ToString("F0"), style1);
	
	//GUI.color = Color(1.0, 1.0, 1.0, alpha); //Color (r,g,b,a)
	//GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height), damageTexture);
}

function Exploasion(){
	explShake.animation.Play("exploasion");
}