/**
*  Script written by OMA [www.armedunity.com]
**/

var lift : GameObject;
var liftSound : AudioClip;  
private var canUse : boolean = false;
var waitTime : float; //Time to Open door (animation lenght)
private var IRuzspiests : boolean = false;


function Down () {
	lift.animation.CrossFade("LiftDown");
	audio.PlayOneShot(liftSound, 1.0 / audio.volume);
}

function Up (){
	lift.animation.CrossFade("LiftUp");
	audio.PlayOneShot(liftSound, 1.0 / audio.volume);
}

function ApplyDamage(){
	Action ();
}

function Action (){
    if (!canUse && !IRuzspiests){
		Up();
		canUse = true;
		IRuzspiests = true;
		yield WaitForSeconds(waitTime);
		IRuzspiests = false;
	
	}else{
		if (canUse && !IRuzspiests){
		Down();
		canUse = false;
		IRuzspiests = true;
		yield WaitForSeconds(waitTime);
		IRuzspiests = false;
		}
	}
}