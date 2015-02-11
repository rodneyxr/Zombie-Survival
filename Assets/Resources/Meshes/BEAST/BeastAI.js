var target : Transform; 
var moveSpeed = 3; 
var rotationSpeed = 3; 
var attackRange : float = 1.5; 
var chaseRange = 10; // distance within which to start chasing
var giveUpRange = 20; // distance beyond which AI gives up
var attackRepeatTime = 1; 
var anim : GameObject;
var dontComeCloserRange : float = 3;
var maxDamage = 5.0;
var minDamage : float = 5.0;
var attack : AudioClip;
var idleAnim : String = "idle";
var walkAnim : String = "walk";
var attackAnim : String = "attack";
var attackAnim2 : String = "crouchLook";
var hitAnim : String = "hit";
private var chasing = false;
private var attackTime : float;
private var checking : boolean = false;
var attackdelay : float = 0.8;
var delayBeforeJump : float = 1.0;
var walkSound : AudioClip[];
var audioStepLength : float = 0.25;
private var isPlaying : boolean = false;
private var attemptToJump : boolean = false;

var maximumHitPoints = 100.0;
var hitPoints = 100.0;
private var gotHitTimer = -1.0;
var detonationDelay = 0.0;
var deadReplacement : Rigidbody;
var grounded : boolean = false;
var gravity : float = 10;
@HideInInspector
var scoreManager : ScoreManager;
private var myTransform : Transform; 

function Awake(){
    myTransform = transform; //cache transform data for easy access/preformance
	rigidbody.freezeRotation = true;
}

function Start(){
	target = GameObject.FindWithTag("Player").transform;
	anim.animation.wrapMode = WrapMode.Loop;
	anim.animation[attackAnim].wrapMode = WrapMode.Once;
	anim.animation[hitAnim].wrapMode = WrapMode.Once;
	anim.animation[attackAnim].layer = 2;
	anim.animation[hitAnim].layer = 1;
	anim.animation.Stop();
	var GO = gameObject.FindWithTag("ScoreManager");
	scoreManager = GO.GetComponent("ScoreManager");
}

function FixedUpdate () {
	if(target){
		// check distance to target (every frame)
		var distance = (target.position - myTransform.position).magnitude;
	
		if (distance < dontComeCloserRange){
				moveSpeed = 0;
			}else{
				moveSpeed = Random.Range(3, 6);
		}

		if (chasing){
		
			//rotate to look at the player
			myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed*Time.deltaTime);
			transform.eulerAngles = Vector3(0, transform.eulerAngles.y, 0); 
       
			if(distance > attackRange && !attemptToJump){
				myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
				if(grounded){
					anim.animation.CrossFade(walkAnim);
					if(!isPlaying){
						playWalkSounds();
					}
				}	
			}
		
			// give up
			if (distance > giveUpRange) {
				chasing = false;
				audio.Stop();
			}

			// attack
			if (distance < attackRange) {
				anim.animation.CrossFade(attackAnim2);
				if(Time.time > attackTime){
					checkInDelay();
					attackTime = Time.time + attackRepeatTime;	
				}
			}
		} else {
			anim.animation.CrossFade(idleAnim);
			audio.Stop();
			// start chasing if target comes close enough
			if (distance < chaseRange) {
				chasing = true;
			}
		}
	
		// Gravity 
		rigidbody.AddForce(Vector3 (0, -gravity * rigidbody.mass, 0));
		grounded = false;
	}
}

function OnCollisionStay (){
    grounded = true;    
}

function checkInDelay(){
	if (checking)
	return;
	
	checking = true;
	attemptToJump = true;
	yield WaitForSeconds(delayBeforeJump);
	rigidbody.AddRelativeForce (0, 30000, 40000);
	anim.animation.CrossFade(attackAnim);
	yield WaitForSeconds (attackdelay);
    attemptToJump = false;
	if((target.position - myTransform.position).magnitude < 1.5){
		target.SendMessage( "PlayerDamage", Random.Range(minDamage, maxDamage));
		audio.PlayOneShot(attack, 1.0 / audio.volume);
		rigidbody.AddRelativeForce (0, 2000, -10000);
		} else {
			checking = false;
			return;
		}
	checking = false;		
}

function ApplyDamage (damage : float) {
	if (hitPoints <= 0.0)
		return;
	
	hitPoints -= damage;
	scoreManager.DrawCrosshair();
	if (hitPoints <= 0.0)
		Invoke("Detonate", detonationDelay);
}

function Detonate () {
	scoreManager.addScore(10);
	// Destroy ourselves
	Destroy(gameObject);
	gameObject.SetActive(false);
	
	// If we have a dead barrel then replace ourselves with it!
	if (deadReplacement) {
		var dead : Rigidbody = Instantiate(deadReplacement, transform.position, transform.rotation);

		// For better effect we assign the same velocity to the exploded barrel
		dead.rigidbody.velocity = rigidbody.velocity;
		dead.angularVelocity = rigidbody.angularVelocity;
    }
}	

function playWalkSounds(){
	isPlaying = true;
	audio.clip = walkSound[Random.Range(0, walkSound.length)];
	audio.Play();
	yield WaitForSeconds (audioStepLength);
	isPlaying = false;
}

function OnDrawGizmosSelected (){
	Gizmos.color = Color.yellow;
	Gizmos.DrawWireSphere (transform.position, attackRange);
	Gizmos.color = Color.red;
	Gizmos.DrawWireSphere (transform.position, chaseRange);
}
