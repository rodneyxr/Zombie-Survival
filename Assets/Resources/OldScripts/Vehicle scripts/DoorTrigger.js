/**
*  Script written by OMA [www.armedunity.com]
**/

var door : GameObject; 
private var opened : boolean = false;
var waitTime : float; 				//Time to Open door (animation time)
private var IRuzspiests : boolean = false;


function Close () {
	door.animation.CrossFade("Close");
}

function Open (){
	door.animation.CrossFade("Open");
}

function Action (){
    if (!opened && !IRuzspiests){
		Open();
		opened = true;
		IRuzspiests = true;
		yield WaitForSeconds(waitTime);
		IRuzspiests = false;
	}else if (opened && !IRuzspiests){
		Close();
		opened = false;
		IRuzspiests = true;
		yield WaitForSeconds(waitTime);
		IRuzspiests = false;
    }
}

function ApplyDamage(){
	Action ();
}