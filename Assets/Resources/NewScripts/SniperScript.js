/** 
*  Script written by OMA [www.armedunity.com]
**/
enum FireMode { semi, auto }
var Mode = FireMode.semi;

var soundFire : AudioClip;
var soundReload : AudioClip;
var soundEmpty : AudioClip;
var soundDraw : AudioClip;
var weaponAnim : GameObject;
	
var Concrete : GameObject;
var Wood : GameObject;
var Metal : GameObject;
var Dirt : GameObject;
var Blood : GameObject;
var untagged : GameObject;

var layerMask : LayerMask;
var muzzleFlash : Renderer;
var muzzleLight : Light;
	
var damage : int = 50;
var bulletsPerMag : int = 50;
var magazines : int = 5;
var reloadTime = 2;
var drawTime = 1.0;
private var bulletsLeft = 0;
var fireRate : float = 0.1;
var range : float = 1000;
var force : float = 500;

//Weapon accuracy
private var baseInaccuracy : float;
var inaccuracyIncreaseOverTime : float = 0.01;
var inaccuracyDecreaseOverTime : float = 0.5;
private var maximumInaccuracy : float;
private var triggerTime : float = 0.05;
var baseInaccuracyAIM : float = 0.005;
var baseInaccuracyHIP : float = 1.5;
var maxInaccuracyHIP : float = 5.0;
var maxInaccuracyAIM : float = 1.0;
	
//Aiming
var hipPosition : Vector3;
var aimPosition : Vector3;
private var aiming : boolean;
private var curVect : Vector3;
var aimSpeed : float = 0.25;
var scopeTime : float;
private var inScope : boolean = false;
var scopeTexture : Texture;
	
//Field Of View
var zoomSpeed : float = 0.5;
var FOV : int = 40;
	
//KickBack
var kickGO : Transform;
var kickUpside : float = 0.5;
var kickSideways : float = 0.5;
	
//GUI
var mySkin : GUISkin; 
 
private var m_LastFrameShot : int = -1;
private var nextFireTime = 0.0;
private var reloading : boolean;
private var draw : boolean;
var weaponCamera : GameObject;
var mainCamera : GameObject;
private var player : GameObject;
	@HideInInspector
var selected : boolean = false;
private var isFiring : boolean = false;
private var playing : boolean = false;
	
//Crosshair Textures
var crosshairFirstModeHorizontal : Texture2D;
var crosshairFirstModeVertical : Texture2D;
private var adjustMaxCroshairSize : float = 6.0;
	
function Start(){
    weaponCamera = GameObject.FindWithTag("WeaponCamera");
    mainCamera = GameObject.FindWithTag("MainCamera");
    player = GameObject.FindWithTag("Player");
    muzzleFlash.enabled = false;
    muzzleLight.enabled = false;
    bulletsLeft = bulletsPerMag;
    aiming = false;
}	
 
 
function Update() {
    if(selected){
	
        if(Input.GetButtonDown ("Fire1")){
            if(Mode == FireMode.semi){
                fireSniper();
            }
			
            if(bulletsLeft > 0)
                isFiring = true;
        }
		
        if (Input.GetButton ("Fire1")){
            if(Mode == FireMode.auto){
                fireSniper();
                if(bulletsLeft > 0)
                    isFiring = true;
            }	
        }
		
        if (Input.GetButtonDown ("Reload")){
            Reload();
        }
 
        if (Input.GetButton("Fire2") && !Input.GetButton("Run") && !reloading){		
            if (!aiming){
                aiming = true;
                curVect = aimPosition - transform.localPosition;
                scopeTime = Time.time + aimSpeed;
            }

            if (transform.localPosition != aimPosition && aiming){
                if(Mathf.Abs(Vector3.Distance(transform.localPosition , aimPosition)) < curVect.magnitude/aimSpeed * Time.deltaTime){
                    transform.localPosition = aimPosition;
                } else {
                    transform.localPosition += curVect/aimSpeed * Time.deltaTime;					
                }
            }
			
            if (Time.time >= scopeTime && !inScope){
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
            curVect= hipPosition-transform.localPosition;
            var go = GetComponentsInChildren(Renderer);
            for( var g : Renderer in go){
            if (g.name != "muzzle_flash")
                g.renderer.enabled = true;
            }
    }
			
    if(Mathf.Abs(Vector3.Distance(transform.localPosition , hipPosition)) < curVect.magnitude/aimSpeed*Time.deltaTime){
        transform.localPosition = hipPosition;
    }else{
        transform.localPosition += curVect/aimSpeed*Time.deltaTime;
    }
}
		
if(inScope){
    maximumInaccuracy = maxInaccuracyAIM;
    baseInaccuracy = baseInaccuracyAIM;
    mainCamera.camera.fieldOfView -= FOV * Time.deltaTime/zoomSpeed;
    if(mainCamera.camera.fieldOfView < FOV){
        mainCamera.camera.fieldOfView = FOV;
    }
    weaponCamera.camera.fieldOfView -= 40 * Time.deltaTime/zoomSpeed;
    if(weaponCamera.camera.fieldOfView < 40){
        weaponCamera.camera.fieldOfView = 40;
    }
}else{
    maximumInaccuracy = maxInaccuracyHIP;
    baseInaccuracy = baseInaccuracyHIP;
    mainCamera.camera.fieldOfView += 60 * Time.deltaTime/0.2;
    if(mainCamera.camera.fieldOfView > 60){
        mainCamera.camera.fieldOfView = 60;
    }
    weaponCamera.camera.fieldOfView += 50 * Time.deltaTime/0.2;
    if(weaponCamera.camera.fieldOfView > 50){
        weaponCamera.camera.fieldOfView = 50;
    }
}

if(player.rigidbody.velocity.magnitude > 3.0){
    triggerTime += inaccuracyDecreaseOverTime;
}

if(isFiring){
    triggerTime += inaccuracyIncreaseOverTime;
}else{
    if(player.rigidbody.velocity.magnitude < 3.0)
        triggerTime -= inaccuracyDecreaseOverTime;
}
	
if (triggerTime >= maximumInaccuracy) {
    triggerTime = maximumInaccuracy;
}
	
if (triggerTime <= baseInaccuracy) {
    triggerTime = baseInaccuracy;
}
		
if(nextFireTime > Time.time){
    isFiring = false;
}
}
}

function LateUpdate(){
    if (m_LastFrameShot == Time.frameCount && !inScope){
        muzzleFlash.transform.localRotation = Quaternion.AngleAxis(Random.value * 360, Vector3.forward);
        muzzleFlash.enabled = true;
        muzzleLight.enabled = true;		
    }else{
        muzzleFlash.enabled = false;
        muzzleLight.enabled = false;
    }	
}
	
function OnGUI (){
    //if(selected){
    //	GUI.skin = mySkin;
    //	var style1 = mySkin.customStyles[0];
		
    //	if(selected){
    //		GUI.Label (Rect(Screen.width - 200,Screen.height-35,200,80),"Bullets : ");
    //		GUI.Label (Rect(Screen.width - 110,Screen.height-35,200,80),"" + bulletsLeft, style1);
    //		GUI.Label (Rect(Screen.width - 80,Screen.height-35,200,80)," / " + magazines);
    //	}
		
    //	if(scopeTexture != null && inScope){
    //		GUI.DrawTexture(Rect(0,0,Screen.width,Screen.height), scopeTexture, ScaleMode.StretchToFill);
    //	}else{
    //		if(crosshairFirstModeHorizontal != null){	
    //			var w = crosshairFirstModeHorizontal.width;
    //			var h = crosshairFirstModeHorizontal.height;
    //			position1 = Rect((Screen.width + w)/2 + (triggerTime * adjustMaxCroshairSize),(Screen.height - h)/2, w, h);
    //			position2 = Rect((Screen.width - w)/2,(Screen.height + h)/2 + (triggerTime * adjustMaxCroshairSize), w, h);
    //			position3 = Rect((Screen.width - w)/2 - (triggerTime * adjustMaxCroshairSize) - w,(Screen.height - h )/2, w, h);
    //			position4 = Rect((Screen.width - w)/2,(Screen.height - h)/2 - (triggerTime * adjustMaxCroshairSize) - h, w, h);
    //			if (!aiming) { 
    //				GUI.DrawTexture(position1, crosshairFirstModeHorizontal); 	//Right
    //				GUI.DrawTexture(position2, crosshairFirstModeVertical); 	//Up
    //				GUI.DrawTexture(position3, crosshairFirstModeHorizontal); 	//Left
    //				GUI.DrawTexture(position4, crosshairFirstModeVertical);		//Down
    //			}
    //		}
    //	}
    //}		
}


function fireSniper (){
    if (reloading || bulletsLeft <= 0){
        if(bulletsLeft == 0){
            OutOfAmmo();
        }		
        return;
    }
	
    if (Time.time - fireRate > nextFireTime)
        nextFireTime = Time.time - Time.deltaTime;

    while(nextFireTime < Time.time){
        fireOneBullet();
        nextFireTime = Time.time + fireRate;
    }
}

function fireOneBullet (){
    if (nextFireTime > Time.time || draw){
        if(bulletsLeft <= 0){
            OutOfAmmo();
        }	
        return; 
    }
	
    var direction = mainCamera.transform.TransformDirection(Vector3(Random.Range(-0.05, 0.05) * triggerTime/3, Random.Range(-0.05, 0.05) * triggerTime/3,1));
    var hit : RaycastHit;
    var position = transform.parent.position;

    if (Physics.Raycast (position, direction, hit, range, layerMask.value)) {
	
        var contact = hit.point;
        var rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
		
        if (hit.rigidbody){
            hit.rigidbody.AddForceAtPosition(force * direction, hit.point);
        }
		
        if(hit.transform.tag == "Fracture"){
            hit.transform.GetComponent("SimpleFracture").Fracture(contact,direction, true);
        }

        if (hit.transform.tag == "Untagged") {
            var default1 = Instantiate (untagged, contact, rotation) as GameObject;
            hit.collider.SendMessageUpwards("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
            default1.transform.parent = hit.transform;
        }
		
        if (hit.transform.tag == "Concrete") {
            var bulletHole = Instantiate (Concrete, contact, rotation) as GameObject;
            hit.collider.SendMessageUpwards("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
            bulletHole.transform.parent = hit.transform;
        }	
		
        if (hit.transform.tag == "Wood") {
            var woodHole = Instantiate (Wood, contact, rotation) as GameObject;
            hit.collider.SendMessageUpwards("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
            woodHole.transform.parent = hit.transform;
        }
		
        if (hit.transform.tag == "Metal") {
            var metalHole = Instantiate (Metal, contact, rotation) as GameObject;
            hit.collider.SendMessageUpwards("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
            metalHole.transform.parent = hit.transform;
        }
		
		
        if (hit.transform.tag == "Dirt") {
            var dirtHole = Instantiate (Dirt, contact, rotation) as GameObject;
            hit.collider.SendMessageUpwards("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
            dirtHole.transform.parent = hit.transform;
        }
		
        if (hit.transform.tag == "canBeUsed") {
            hit.collider.SendMessageUpwards("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
        }	
		
        if (hit.transform.tag == "Enemy") {
            hit.collider.SendMessageUpwards("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
			
            yield WaitForSeconds(0.02);
            var bloodHole = Instantiate (Blood, contact, rotation) as GameObject;
            if(Physics.Raycast (position, direction, hit, range, layerMask.value)){
                if(hit.rigidbody){
                    hit.rigidbody.AddForceAtPosition(force * direction, hit.point);
                }
            }	
        }
    }
	
    PlayAudioClip(soundFire, transform.position, 0.7);
    m_LastFrameShot = Time.frameCount;

    weaponAnim.animation.Rewind("sniperFire");
    weaponAnim.animation.Play("sniperFire");
    bulletsLeft--;
    KickBack();
}

function OutOfAmmo(){
    if(reloading || playing)
        return;
	
    playing = true;
    yield WaitForSeconds(0.2);
    PlayAudioClip(soundEmpty, transform.position, 0.7);	
    yield WaitForSeconds(0.2);
    playing = false;
}

function PlayAudioClip (clip : AudioClip, position : Vector3, volume : float) {
    var go = new GameObject ("One shot audio");
    go.transform.position = position;
    var source : AudioSource = go.AddComponent (AudioSource);
    source.clip = clip;
    source.volume = volume;
    source.pitch = Random.Range(0.95,1.05);
    source.Play ();
    Destroy (go, clip.length);
    return source;
}

    function Reload (){
        if(reloading)
            return;
	
        reloading = true;
        if (magazines > 0 && bulletsLeft < bulletsPerMag) {
            weaponAnim.animation["Reload"].speed = 1.0;
            weaponAnim.animation.Play("Reload", PlayMode.StopAll);
            weaponAnim.animation.CrossFade("Reload");
            audio.PlayOneShot(soundReload);
            yield WaitForSeconds(reloadTime);
            magazines --;
            bulletsLeft = bulletsPerMag;
        }
        reloading = false;
    }

    function DrawWeapon(){
        draw = true;
        audio.clip = soundDraw;
        audio.Play();
        weaponAnim.animation.Play("Draw", PlayMode.StopAll);
        weaponAnim.animation.CrossFade("Draw");
        yield WaitForSeconds(drawTime);
        draw = false;
        reloading = false;
        selected = true;
    }

    function KickBack() {
        kickGO.localRotation = Quaternion.Euler(kickGO.localRotation.eulerAngles - Vector3(kickUpside, Random.Range(-kickSideways, kickSideways), 0));   
    }

    function Deselect(){
        selected = false;
        mainCamera.camera.fieldOfView = 60;
        weaponCamera.camera.fieldOfView = 50;
        inScope = false;
        transform.localPosition = hipPosition;
        var go = GetComponentsInChildren(Renderer);
        for( var g : Renderer in go){
            if (g.name != "muzzle_flash")
                g.renderer.enabled = true;
        }
}

 
 