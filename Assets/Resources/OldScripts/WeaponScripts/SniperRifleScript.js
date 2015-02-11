
var bulletGUI : GUIText;
var clipsGUI : GUIText;
var soundFire : AudioClip;
var soundReload : AudioClip;
var soundEmpty : AudioClip;
var fireReloadDraw : GameObject;
var Concrete : GameObject[];
var Wood : GameObject[];
var Metal : GameObject[];
var Dirt : GameObject[];
var Blood : GameObject[];
var untagged : GameObject;
var rifleCrosshair : GameObject;
var layerMask : LayerMask;
var muzzleFlash : Renderer;
var muzzleLight : Light;
var aimMove : GameObject;
var aimMove2 : GameObject;
var waitTime = 0.1;
var kickbackTime = 1;
var damage = 50;
var bulletsPerClip = 50;
var clips = 5;
var reloadTime = 2;
var drawTime = 1.0;
static var bulletsLeft = 0;
static var forClips = 0;
var fireRate : float = 0.1;
var range : float = 1000.0;
var force : float = 1500.0;
var baseInaccuracy : float = 0.01;
var inaccuracyIncreaseOverTime : float = 2.5;
var maximumInaccuracy : float = 3.0;

//KickBack
var recoilMod : Transform;
var Kickback1 : GameObject;
var Kickback2 : GameObject;
var maxRecoil_x : float = -20;
var recoilSpeed : float = 10;
var kickback : float = 0.0;

//Aiming
var scopeTexture : Texture;
var aimSound : AudioClip;
var pw : GameObject;
var aimSpeed : float = 0.25;

var useScopeTexture : boolean = false; //Use scopeTexture or down sights

var hipPosition : Vector3;
var aimPosition : Vector3;

private var inScope : boolean = false;
private var scopeTime : float;
private var aiming : boolean;
private var curVect : Vector3;

private var triggerTime : float = 0.02;
private var m_LastFrameShot : int = -1;
private var nextFireTime = 0.0;
private var reload : boolean = true;
private var reloading : boolean;
private var draw : boolean;
private var holding : boolean;
private var weaponCamera : GameObject;
private var mainCamera : GameObject;
private var player : GameObject;


function Start(){
    weaponCamera = GameObject.FindWithTag("WeaponCamera");
	mainCamera = GameObject.FindWithTag("MainCamera");
    muzzleFlash.enabled = false;
	muzzleLight.enabled = false;
	bulletsLeft = bulletsPerClip;
	forClips = clips;
	player = GameObject.FindWithTag("Player");
	aiming = false;
	canaim = true;
	Gui();
}


function Gui () {
    bulletGUI.text ="Bullets: " + GetBulletsLeft().ToString();
    clipsGUI.text ="Clips: " + GetClipsLeft().ToString();
}
	
function Update() {

	if (Input.GetButtonUp("Drop") && !reloading && !aiming){
		BroadcastMessage("DropWeapon");
	}

    if (Input.GetButtonDown ("Reload") && !reloading){
	    Reload ();
	}
	
	if (Input.GetButtonDown("Fire1")) {
	   Fire();

		triggerTime += inaccuracyIncreaseOverTime;
		if (triggerTime >= maximumInaccuracy) {
			triggerTime = maximumInaccuracy;
		}
	} else {
		triggerTime -= 0.1;
		if (triggerTime <= baseInaccuracy) {
			triggerTime = baseInaccuracy;
		}
		
	}
	
	if (Input.GetButton("Fire2") && !Input.GetButton ("Run") && !Input.GetButton ("Jump") && !reloading){		
		if (!aiming){
			scopeTime = Time.time + aimSpeed;
			aiming = true;
			curVect= aimPosition-transform.localPosition;
			rifleCrosshair.guiTexture.enabled = false;

	    }
		
		if(Input.GetButton("Fire2") && Input.GetKeyDown("e") && !holding){
		HoldBreath();
		}

		if (transform.localPosition!=aimPosition && aiming){
			if(Mathf.Abs(Vector3.Distance(transform.localPosition , aimPosition)) < curVect.magnitude/aimSpeed*Time.deltaTime){
				transform.localPosition=aimPosition;
				aimStart();
			} else {
				transform.localPosition += curVect/aimSpeed*Time.deltaTime;
			}
		}
		
		if (useScopeTexture && Time.time >= scopeTime && !inScope){
			inScope = true;
			var gos = GetComponentsInChildren(Renderer);
				for( var go : Renderer in gos){
			        go.renderer.enabled = false;
			    }
				
		
		}

	} else {
		if (aiming){
			aiming = false;
			inScope = false;
			rifleCrosshair.guiTexture.enabled = true;
			curVect= hipPosition-transform.localPosition;
			aimStop();
			
			var go = GetComponentsInChildren(Renderer);
			for( var g : Renderer in go){
			if (g.name != "muzzle_flash")
                g.renderer.enabled = true;
			}
		}
			
		if(Mathf.Abs(Vector3.Distance(transform.localPosition , hipPosition)) < curVect.magnitude/aimSpeed*Time.deltaTime){
			transform.localPosition=hipPosition;
		} else {
			transform.localPosition += curVect/aimSpeed*Time.deltaTime;
		}
	}
	KickBack();
}

function aimStart(){
	mainCamera.camera.fieldOfView = 10;
	weaponCamera.camera.fieldOfView = 60;
	aimMove.animation.CrossFade("aim");
	aimMove2.animation.CrossFade("aim");
	weaponCamera.GetComponent("MouseLook").sensitivityY = 1;
	player.GetComponent("MouseLook").sensitivityX = 1;
	camera.main.GetComponent("MouseLook").sensitivityY = 1;
}

function HoldBreath(){
if(holding)
return;
pw.audio.clip = aimSound;
pw.audio.Play();
holding = true;
yield WaitForSeconds(.5);
aimMove.animation.CrossFade("IdleBreath");
aimMove2.animation.CrossFade("IdleBreath"); 
aimMove.animation.PlayQueued("aim", QueueMode.CompleteOthers);
aimMove2.animation.PlayQueued("aim", QueueMode.CompleteOthers);
}


function aimStop(){
    holding = false;
	setPosition();
	weaponCamera.GetComponent("MouseLook").sensitivityY = 3;
	player.GetComponent("MouseLook").sensitivityX = 3;
	camera.main.GetComponent("MouseLook").sensitivityY = 3;
	mainCamera.camera.fieldOfView = 70;
	weaponCamera.camera.fieldOfView = 60;
	aimMove.animation.Stop("aim");
	aimMove2.animation.Stop("aim");
    pw.audio.clip = aimSound;
	pw.audio.Stop();
}

function setPosition(){
aimMove. transform.localPosition = Vector3.zero;
}

function LateUpdate (){
kickBack = kickback*180/mainCamera.camera.fieldOfView*Screen.height;
}

function KickBack() {
	if (kickback > 0.015)
	kickback = 0.015;
	
	if(kickback > 0.0){
		var maxRecoil = Quaternion.Euler (maxRecoil_x, 0, 0);
		recoilMod.rotation = Quaternion.Slerp(recoilMod.rotation, maxRecoil, Time.deltaTime * recoilSpeed);
		Kickback1.transform.localEulerAngles.x = recoilMod.localEulerAngles.x;
		Kickback2.transform.localEulerAngles.x = recoilMod.localEulerAngles.x;		
		kickback -= Time.deltaTime/5;
	}else{
		kickback = 0.0;
		var minRecoil = Quaternion.Euler (0, 0, 0);
		recoilMod.rotation = Quaternion.Slerp(recoilMod.rotation, minRecoil,Time.deltaTime * recoilSpeed / 0.2);
		Kickback1.transform.localEulerAngles.x = recoilMod.localEulerAngles.x;
		Kickback2.transform.localEulerAngles.x = recoilMod.localEulerAngles.x;

	}
	//kickBack = kickback*180/mainCamera.camera.fieldOfView*Screen.height;
}

function OnGUI () {
	if(scopeTexture != null && inScope)
		GUI.DrawTexture(Rect(0,0,Screen.width,Screen.height), scopeTexture, ScaleMode.StretchToFill);
}

function Fire() {
    if (reloading)
	return; 
	
	if(draw)
	return;

	if ((bulletsLeft == 0) && (reload == true))
		Empty();
		
	if (Time.time - fireRate > nextFireTime)
		nextFireTime = Time.time - Time.deltaTime;
	
	while( nextFireTime < Time.time && bulletsLeft != 0){
		FireOneShot();
		fireReloadDraw.animation.Rewind("sniperFire");
	    fireReloadDraw.animation.Play("sniperFire");
		nextFireTime += fireRate;
	}
}

function FireOneShot () {
	var direction = transform.TransformDirection(Random.Range(-0.05, 0.05) * triggerTime, Random.Range(-0.05, 0.05) * triggerTime, 1);
	var hit : RaycastHit;
	var position = transform.parent.position;

	if (Physics.Raycast (position, direction, hit, range, layerMask.value)) {
	
	     var contact = hit.point;
		 var rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
			
		if (hit.rigidbody)
			hit.rigidbody.AddForceAtPosition(force * direction, hit.point);
			
		if (hit.transform.tag == "Untagged") {
				var default1 = Instantiate (untagged, contact, rotation) as GameObject;
				default1.transform.localPosition += .05*hit.normal;
				default1.transform.parent = hit.transform;
		    }
		if (hit.transform.tag == "Concrete") {
                var bulletHole = Instantiate (Concrete[Random.Range(0, Concrete.length)], contact, rotation) as GameObject;
				bulletHole.transform.localPosition += .05*hit.normal;
				bulletHole.transform.parent = hit.transform;
			}	
		if (hit.transform.tag == "Wood") {
                var woodHole = Instantiate (Wood[Random.Range(0, Wood.length)], contact, rotation) as GameObject;
				woodHole.transform.localPosition += .05*hit.normal;
				woodHole.transform.parent = hit.transform;
		    }
		if (hit.transform.tag == "Metal") {
                var metalHole = Instantiate (Metal[Random.Range(0, Metal.length)], contact, rotation) as GameObject;
				metalHole.transform.localPosition += .05*hit.normal;
				metalHole.transform.parent = hit.transform;
		    }
		if (hit.transform.tag == "Enemy") {
                var bloodHole = Instantiate (Blood[Random.Range(0, Blood.length)], contact, rotation) as GameObject;
				bloodHole.transform.localPosition += .05*hit.normal;
				bloodHole.transform.parent = hit.transform;
		    }
		if (hit.transform.tag == "Dirt") {
                var dirtHole = Instantiate (Dirt[Random.Range(0, Dirt.length)], contact, rotation) as GameObject;
				dirtHole.transform.localPosition += .05*hit.normal;
				dirtHole.transform.parent = hit.transform;
		    }
			
		hit.collider.SendMessageUpwards("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
		
		if (audio) {
		audio.Play();
		}
		
		m_LastFrameShot = Time.frameCount;
	}
	
	if(!reloading && draw == false){
	kickback+= 0.01;
	}
	
	bulletsLeft--;
	Gui();
	Shoot();
}


function Shoot () {
		audio.clip = soundFire;
		audio.Play();	
        yield WaitForSeconds(kickbackTime);

}

function Empty () {
        reload = false;
        audio.clip = soundEmpty;
        audio.Play();
		fireReloadDraw.animation["Fire"].speed = 3.0;
	    fireReloadDraw.animation.Play("Fire");
        yield WaitForSeconds(0.3);
		Reload ();
    } 

function Reload () {
	reloading = true;
	if ((forClips > 0) && (bulletsLeft < bulletsPerClip)) {
		yield WaitForSeconds(waitTime);
        fireReloadDraw.animation.Play("Reload", PlayMode.StopAll);
	    fireReloadDraw.animation.CrossFade("Reload");
        audio.PlayOneShot(soundReload);
        yield WaitForSeconds(reloadTime);
		forClips --;
		bulletsLeft = bulletsPerClip;
	}
	if ((forClips > 0) && (bulletsLeft < bulletsPerClip)) {
       forClips --;
       bulletsLeft = bulletsPerClip;
	}
	Gui();
	reload = true;
	reloading = false;
}

function DrawWeapon(){
 draw = true;
 yield WaitForSeconds(drawTime);
 draw = false;
  	if ((forClips > 0) && (bulletsLeft < 1)) {
	Reload();
	}else{
	reload = true;
	reloading = false;
	}
}

function GetBulletsLeft () {
	return bulletsLeft;
}

function GetClipsLeft () {
        return forClips;
}
