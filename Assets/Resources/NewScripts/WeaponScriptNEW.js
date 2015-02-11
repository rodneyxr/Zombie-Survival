/**
*  Script written by OMA [www.armedunity.com]
**/

@script ExecuteInEditMode()

enum fireMode { none, semi, auto, burst, launcher}
@HideInInspector
var currentMode = fireMode.semi;
var firstMode = fireMode.semi;
var secondMode = fireMode.launcher;

enum Ammo { Magazines, Bullets }
var ammoMode : Ammo = Ammo.Magazines;

var canReloadLauncher : boolean = false; 
	
var soundDraw : AudioClip;
var soundFire : AudioClip;
var soundReload : AudioClip;
var soundEmpty : AudioClip;
var switchModeSound : AudioClip;

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
private var fireRate : float = 0.1;
var fireRateFirstMode : float = 0.1;
var fireRateSecondMode : float = 0.1;
var reloadTime : float = 3.0;
var reloadAnimSpeed : float = 1.2;
var drawTime : float = 1.5;
	
var range : float = 250.0;
var force : float = 200.0;
	
//Weapon accuracy
private var baseInaccuracy : float;
var baseInaccuracyAIM : float = 0.005;
var baseInaccuracyHIP : float = 1.5;
var inaccuracyIncreaseOverTime : float = 0.2;
var inaccuracyDecreaseOverTime : float = 0.5;
private var maximumInaccuracy : float;
var maxInaccuracyHIP : float = 5.0;
var maxInaccuracyAIM : float = 1.0;
	
private var triggerTime : float = 0.05;
	
//Aiming
var hipPosition : Vector3;
var aimPosition : Vector3;
private var aiming : boolean;
private var curVect : Vector3;
var aimSpeed : float = 0.25;
	
//Field Of View
var zoomSpeed : float = 0.5;
var FOV : int = 40;
	
private var bulletsLeft : int = 0;
private var m_LastFrameShot : int = -10;
private var nextFireTime : float = 0.0;
	@HideInInspector
var reloading : boolean;
private var draw : boolean;
private var weaponCamera : GameObject;
private var mainCamera : GameObject;
private var playing : boolean = false;
	@HideInInspector
var selected : boolean = false;
private var player : GameObject;
private var isFiring : boolean = false;
private var bursting : boolean = false;
	
//Burst
var shotsPerBurst : int = 3;
var burstTime : float = 0.07;	
	
//GUI
var mySkin : GUISkin; 

//Launcher
var projectilePrefab : Rigidbody;
var projectileSpeed = 30.0;
var projectiles = 20;
var launchPosition : GameObject;
var fireLauncherAnimGO : GameObject;
var soundReloadLauncher : AudioClip;
private var launcjerReload : boolean = false;
var launcherReloadTime : float = 2.0;
var reloadlauncher : String = "reloadlauncher";
private var rocket : GameObject;
	
//KickBack
var kickGO : Transform;
var kickUpside : float = 0.5;
var kickSideways : float = 0.5;
	
//Crosshair Textures
var crosshairFirstModeHorizontal : Texture2D;
var crosshairFirstModeVertical : Texture2D;
var crosshairSecondMode : Texture2D;
private var adjustMaxCroshairSize : float = 6.0;
	
//Switch Weapon Fire Modes
private var canSwicthMode : boolean = true;
	
function Start(){
    weaponCamera = GameObject.FindWithTag("WeaponCamera");
    mainCamera = GameObject.FindWithTag("MainCamera");
    player = GameObject.FindWithTag("Player");
    muzzleFlash.enabled = false;
    muzzleLight.enabled = false;
    bulletsLeft = bulletsPerMag;
    currentMode = firstMode;
    fireRate = fireRateFirstMode;
    aiming = false;
    if(firstMode == fireMode.launcher){
        rocket = GameObject.Find("RPGrocket");
    }
    if(ammoMode == Ammo.Bullets){
        magazines = magazines * bulletsPerMag;
    }
}	
	
function Update(){
    if(selected){
	
        if (Input.GetButtonDown ("Fire1")){
            if(currentMode == fireMode.semi){
                fireSemi();
            }
		
            if(currentMode == fireMode.launcher){
                fireLauncher();
            }
			
            if(currentMode == fireMode.burst){
                fireBurst();
            }
			
            if(bulletsLeft > 0)
                isFiring = true;	
        }
		
        if (Input.GetButton ("Fire1")){
            if(currentMode == fireMode.auto){
                fireSemi();
                if(bulletsLeft > 0)
                    isFiring = true;
            }	
        }
		
        if (Input.GetButtonDown ("Reload")){
            Reload();
        }
    }

    if (Input.GetButton("Fire2") && !Input.GetButton ("Run") && !reloading){		
        if (!aiming){
            aiming = true;
            curVect = aimPosition - transform.localPosition;
        }
        if (transform.localPosition != aimPosition && aiming){
            if(Mathf.Abs(Vector3.Distance(transform.localPosition , aimPosition)) < curVect.magnitude/aimSpeed * Time.deltaTime){
                transform.localPosition = aimPosition;
            } else {
                transform.localPosition += curVect/aimSpeed * Time.deltaTime;					
            }
        }
    } else {
        if (aiming){
            aiming = false;
            curVect = hipPosition - transform.localPosition;
            /*
			var go = GetComponentsInChildren(Renderer);
			for( var g : Renderer in go){
			if (g.name != "muzzle_flash")
				g.renderer.enabled = true;
			}
			*/
        }
		
        if(Mathf.Abs(Vector3.Distance(transform.localPosition , hipPosition)) < curVect.magnitude/aimSpeed * Time.deltaTime){
            transform.localPosition = hipPosition;
        }else{
            transform.localPosition += curVect/aimSpeed * Time.deltaTime;
        }
    }
		
    if(aiming){
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
        mainCamera.camera.fieldOfView += 60 * Time.deltaTime/0.5;
        if(mainCamera.camera.fieldOfView > 60){
            mainCamera.camera.fieldOfView = 60;
        }
        weaponCamera.camera.fieldOfView += 50 * Time.deltaTime/0.5;
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
	
    if(Input.GetButtonDown("switchFireMode") && secondMode != fireMode.none && canSwicthMode){
        if(currentMode != firstMode){
            FirstFireMode();
        }else{
            SecondFireMode();
        }
    }			
}


function LateUpdate(){
    if (m_LastFrameShot == Time.frameCount){
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
    //	if(ammoMode == Ammo.Magazines){
    //		if(firstMode != fireMode.none && firstMode != fireMode.launcher || secondMode != fireMode.none && secondMode != fireMode.launcher){
    //			GUI.Label (Rect(Screen.width - 200,Screen.height-35,200,80),"Ammo : ");
    //			GUI.Label (Rect(Screen.width - 110,Screen.height-35,200,80),"" + bulletsLeft, style1);
    //			GUI.Label (Rect(Screen.width - 80,Screen.height-35,200,80)," / " + magazines);
    //		}
    //	}	
		
    //	if(ammoMode == Ammo.Bullets){
    //		if(firstMode != fireMode.none && firstMode != fireMode.launcher || secondMode != fireMode.none && secondMode != fireMode.launcher){
    //			GUI.Label (Rect(Screen.width - 200,Screen.height-35,200,80),"Bullets : ");
    //			GUI.Label (Rect(Screen.width - 110,Screen.height-35,200,80),"" + bulletsLeft, style1);
    //			GUI.Label (Rect(Screen.width - 80,Screen.height-35,200,80)," |  " + magazines);
    //		}	
    //	}
		
    //	if(firstMode != fireMode.none || secondMode != fireMode.none){
    //		GUI.Label (Rect(Screen.width - 200,Screen.height-65,200,80),"Firing Mode :");
    //		GUI.Label (Rect(Screen.width - 110,Screen.height-65,200,80),"" + currentMode, style1);
    //	}
		
    //	if(secondMode == fireMode.launcher || firstMode == fireMode.launcher){
    //		GUI.Label (Rect(Screen.width - 200,Screen.height-95,200,80),"Projectiles : ");
    //		GUI.Label (Rect(Screen.width - 110,Screen.height-95,200,80),"" + projectiles, style1);
    //	}	
		
    //	if(crosshairFirstModeHorizontal != null){	
    //		if(currentMode != fireMode.launcher){
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
		
    //	if(crosshairSecondMode != null){
    //		if(currentMode == fireMode.launcher){
    //			var width = crosshairSecondMode.width/2;
    //			var height = crosshairSecondMode.height/2;
    //			position = Rect((Screen.width - width)/2,(Screen.height - height )/2, width, height);
    //			if (!aiming) { 
    //				GUI.DrawTexture(position, crosshairSecondMode);
    //			}
    //		}
    //	}	
    //}	
}

function FirstFireMode(){
	
    canSwicthMode = false;
    selected = false;
    weaponAnim.animation.Rewind("SwitchAnim");
    weaponAnim.animation.Play("SwitchAnim");
    audio.clip = switchModeSound;
    audio.Play();
    yield WaitForSeconds(0.6);
    currentMode = firstMode;
    fireRate = fireRateFirstMode;
    selected = true;
    canSwicthMode = true;
}

function SecondFireMode(){
	
    canSwicthMode = false;
    selected = false;
    audio.clip = switchModeSound;
    audio.Play();
    weaponAnim.animation.Play("SwitchAnim");
    yield WaitForSeconds(0.6);
    currentMode = secondMode;
    fireRate = fireRateSecondMode;
    selected = true;
    canSwicthMode = true;
	
}

function fireSemi (){
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

function fireLauncher (){
    if (reloading || projectiles <= 0){
        if(projectiles == 0){
            OutOfAmmo();
        }		
        return;
    }
	
    if (Time.time - fireRate > nextFireTime)
        nextFireTime = Time.time - Time.deltaTime;

    while(nextFireTime < Time.time){
        fireProjectile();
        nextFireTime = Time.time + fireRate;
    }
}

function fireBurst (){
    var shotCounter : int = 0;
	
    if (reloading || bursting || bulletsLeft <= 0){
        if(bulletsLeft <= 0){
            OutOfAmmo();
        }		
        return;
    }
	
    if (Time.time - fireRate > nextFireTime)
        nextFireTime = Time.time - Time.deltaTime;

    if(Time.time > nextFireTime){
        while (shotCounter < shotsPerBurst){
            bursting = true;
            shotCounter++;
            if (bulletsLeft > 0){
                fireOneBullet();
            }			
            yield WaitForSeconds(burstTime); 
        }           
        nextFireTime = Time.time + fireRate;
    }
    bursting = false;
}


function fireOneBullet (){
    if (nextFireTime > Time.time || draw){
        if(bulletsLeft <= 0){
            OutOfAmmo();
        }	
        return; 
    }
	
    var direction = gameObject.transform.TransformDirection(Vector3(Random.Range(-0.01, 0.01) * triggerTime, Random.Range(-0.01, 0.01) * triggerTime,1));
    var hit : RaycastHit;
    var position = transform.parent.position;

    if (Physics.Raycast(position, direction, hit, range, layerMask.value)) {
	
        var contact = hit.point;
        var rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
		
        if (hit.rigidbody){
            hit.rigidbody.AddForceAtPosition(force * direction, hit.point);
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
			
            yield WaitForSeconds(0.01);
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

    weaponAnim.animation.Rewind("Fire");
    weaponAnim.animation.Play("Fire");
    KickBack();
    bulletsLeft--;
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

    function fireProjectile (){
        if (projectiles < 1 || draw || launcjerReload){	
            return; 
        }
	
        var instantiatedProjectile : Rigidbody = Instantiate (projectilePrefab, launchPosition.transform.position, launchPosition.transform.rotation);
        fireLauncherAnimGO.animation.Play("FireGL");
        instantiatedProjectile.velocity = transform.TransformDirection(Vector3 (0, 0, projectileSpeed));
        Physics.IgnoreCollision(instantiatedProjectile.collider, gameObject.FindWithTag("Player").transform.root.collider);
        projectiles--;
	
        if(canReloadLauncher)
            ReloadLauncher();
    }

    function OutOfAmmo(){
        if(reloading || playing)
            return;
	
        playing = true;
        PlayAudioClip(soundEmpty, transform.position, 0.7);
	
        weaponAnim.animation["Fire"].speed = 2.0;
        weaponAnim.animation.Rewind("Fire");
        weaponAnim.animation.Play("Fire");
        yield WaitForSeconds(0.2);
        playing = false;
    }
    //For RPG
    function ReloadLauncher(){
        if(projectiles > 0){
            launcjerReload = true;
            canSwicthMode = false;
            DisableProjectileRenderer();		
            yield WaitForSeconds(0.5);
            audio.PlayOneShot(soundReloadLauncher);
            weaponAnim.animation[reloadlauncher].speed = 1.2;
            weaponAnim.animation.Play(reloadlauncher);
            yield WaitForSeconds(launcherReloadTime);	
            canSwicthMode = true;
            launcjerReload = false;
        }else{
            if(rocket != null && projectiles == 0){
                rocket.renderer.enabled = false;
            }
        }	
	
    }

    function DisableProjectileRenderer(){
        if(rocket != null){
            rocket.renderer.enabled = false;
        }
        yield WaitForSeconds(launcherReloadTime/1.5);
        if(rocket != null){
            rocket.renderer.enabled = true;
        }
    }

    function EnableProjectileRenderer(){
        if(rocket != null){
            rocket.renderer.enabled = true;
        }
    }

    function Reload (){
        if(reloading)
            return;
	
        if(ammoMode == Ammo.Magazines){
            reloading = true;
            canSwicthMode = false;
            if (magazines > 0 && bulletsLeft != bulletsPerMag) {
                weaponAnim.animation["Reload"].speed = reloadAnimSpeed;
                weaponAnim.animation.Play("Reload", PlayMode.StopAll);
                weaponAnim.animation.CrossFade("Reload");
                audio.PlayOneShot(soundReload);
                yield WaitForSeconds(reloadTime);
                magazines --;
                bulletsLeft = bulletsPerMag;
            }
            reloading = false;
            canSwicthMode = true;
            isFiring = false;
        }	
	
        if(ammoMode == Ammo.Bullets){
            if(magazines > 0 && bulletsLeft != bulletsPerMag){
                if(magazines > bulletsPerMag){
                    canSwicthMode = false;
                    reloading = true;
                    weaponAnim.animation["Reload"].speed = reloadAnimSpeed;
                    weaponAnim.animation.Play("Reload", PlayMode.StopAll);
                    weaponAnim.animation.CrossFade("Reload");
                    audio.PlayOneShot(soundReload);
                    yield WaitForSeconds(reloadTime);
                    magazines -= bulletsPerMag - bulletsLeft;
                    bulletsLeft = bulletsPerMag;
                    canSwicthMode = true;
                    reloading = false;
                    return;
                }else{
                    canSwicthMode = false;
                    reloading = true;
                    weaponAnim.animation["Reload"].speed = reloadAnimSpeed;
                    weaponAnim.animation.Play("Reload", PlayMode.StopAll);
                    weaponAnim.animation.CrossFade("Reload");
                    audio.PlayOneShot(soundReload);
                    yield WaitForSeconds(reloadTime);
                    var bullet = Mathf.Clamp(bulletsPerMag, magazines, bulletsLeft + magazines);
                    magazines -= (bullet - bulletsLeft);
                    bulletsLeft = bullet;
                    canSwicthMode = true;
                    reloading = false;
                    return;
                }	
            }	
        }
    }

    function KickBack() {
        kickGO.localRotation = Quaternion.Euler(kickGO.localRotation.eulerAngles - Vector3(kickUpside, Random.Range(-kickSideways, kickSideways), 0));   
    }

    function DrawWeapon(){
        draw = true;
        canSwicthMode = false;
        audio.clip = soundDraw;
        audio.Play();
        weaponAnim.animation.Play("Draw", PlayMode.StopAll);
        weaponAnim.animation.CrossFade("Draw");
        yield WaitForSeconds(drawTime);
        draw = false;
        reloading = false;
        canSwicthMode = true;
        selected = true;
	
    }

    function Deselect(){
        selected = false;
        mainCamera.camera.fieldOfView = 60;
        weaponCamera.camera.fieldOfView = 50;
        transform.localPosition = hipPosition;
    }
	
	