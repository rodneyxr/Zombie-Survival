
	//ANIMATIONS
	var weaponAnim : GameObject;         
	var idleAnim : String = "Idle";
	var DrawAnimation : String = "DrawAXE";
	var grenadeThrow : String = "GrenadeThrow";
	var grenadePull : String = "GrenadePull";
	var soundDraw : AudioClip;

	//GRENADE
	var grenadeCount : int = 3; 
	var throwForce : float = 15.0;
	var powerIncreaseSpeed : float = 80.0;
	private var powerBarFullWidth : float = 100.0;
	private var thePower : float = 0.0;
	private var increasing : boolean = false;
	private var throwingGrenade : boolean = false;
	
	var grenade : Rigidbody;
	var spawnPos : Transform;
	
	var grenadeRenderer : Renderer;
	
	//CROSSHAIR
	var crosshair : Texture2D;                 
	
	//GUI
	var mySkin : GUISkin;                         
	
	var selected : boolean = false;
	
function Start(){
	weaponAnim.animation[grenadePull].wrapMode = WrapMode.ClampForever;
}	
	
function Update () {
	if(selected){
		if(grenadeCount > 0 && !throwingGrenade){
			
		    if(Input.GetButtonDown("Fire")){
				if(thePower < 5){
					weaponAnim.animation.Stop();
					weaponAnim.animation[grenadePull].speed = 1.0;
					weaponAnim.animation.CrossFade(grenadePull);
					increasing = true;
				}	
			}
		
		    if (Input.GetButtonUp("Fire")){
				increasing = false;
				if(thePower > powerBarFullWidth/2){
					ThrowGrenade(thePower);	
				} else {
					weaponAnim.animation[grenadePull].speed = - 1.0;
					weaponAnim.animation.CrossFade(grenadePull);	
				}	
		    }
			
			if(thePower < 0){
				thePower = 0;
			}
		
		    if(increasing){    
				thePower += Time.deltaTime * powerIncreaseSpeed;
				thePower = Mathf.Clamp(thePower, 0, powerBarFullWidth);
			} else {
				if(thePower > 0)
				thePower -= Time.deltaTime * powerIncreaseSpeed * 2;
			}
		}
	}
}

function OnGUI(){

	GUI.skin = mySkin;
	var style1 = mySkin.customStyles[0];

	GUI.Label (Rect(Screen.width - 200,Screen.height-35,200,80),"Grenades : ");
	GUI.Label (Rect(Screen.width - 110,Screen.height-35,200,80),"" + grenadeCount, style1);
	
	GUI.Label (Rect(Screen.width - 200,Screen.height-65,200,80),"Throwing Power : ");	
	GUI.Label (Rect(Screen.width - 70,Screen.height-65,200,80),"" + thePower.ToString("F0") + "%", style1);

	if(crosshair != null){	
		var w = crosshair.width/2;
		var h = crosshair.height/2;
		position = Rect((Screen.width - w)/2,(Screen.height - h )/2, w, h);	
		GUI.DrawTexture(position, crosshair);
	}
}

function ThrowGrenade (power : float){
    if(throwingGrenade || grenadeCount <= 0)
	return;
   
	throwingGrenade  = true;
	weaponAnim.animation[grenadeThrow].speed = weaponAnim.animation[grenadeThrow].clip.length/0.4;
	weaponAnim.animation.Play(grenadeThrow);
	yield WaitForSeconds (0.2);
	grenadeRenderer.enabled = false;
	var instantGrenade : Rigidbody = Instantiate(grenade, spawnPos.position, spawnPos.rotation);
	
	var fwd : Vector3 = spawnPos.forward;
	instantGrenade.AddForce(fwd * power * throwForce);
	//Physics.IgnoreCollision(instantGrenade.collider, transform.root.collider);
	//Physics.IgnoreCollision(instantGrenade.collider, gameObject.FindWithTag("Player").transform.root.collider);
	grenadeCount--;
	
	yield WaitForSeconds(1.0);
	grenadeRenderer.enabled = true;
	throwingGrenade = false;
	DrawWeapon();	
}

function DrawWeapon(){
	thePower = 0;
	grenadeRenderer.enabled = true;
	increasing = false;
	draw = true;
	if(grenadeCount > 0){
		audio.clip = soundDraw;
		audio.Play();
		weaponAnim.animation[DrawAnimation].speed = weaponAnim.animation[DrawAnimation].clip.length/0.9;
		weaponAnim.animation.Play(DrawAnimation);
		yield WaitForSeconds(0.6);
	}
	selected = true;
}

function Deselect(){
	selected = false;
}