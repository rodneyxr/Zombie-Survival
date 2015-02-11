var target : GameObject;
var fadeDuration : float = 3.0;


function Update(){
	
    if (target.renderer.material.color.a > 0)
    target.renderer.material.color.a -= Time.deltaTime/fadeDuration;
}
