/**
*  Script written by OMA [www.armedunity.com]
**/
	@script ExecuteInEditMode()
	
	var currentScore : int = 0;
	
	var time : float = 0.0;
	var hitCrosshairTexture : Texture;
	private var alphaHit : float;
	var hitSound : AudioClip;
	
	var mySkin : GUISkin;
	
	var pointsToNextRank : int = 50;
	var rank : int = 0;
	var rankSound : AudioClip;

function Update () {

	if (time > 0){ 
        time -= Time.deltaTime;
    }
    alphaHit = time;
}

function DrawCrosshair(){
	yield WaitForSeconds(0.1);
	time = 1.0;
	audio.PlayOneShot(hitSound, .5);
}

function addScore(value : int){
	yield WaitForSeconds(0.2);
	currentScore += value;
	
	if(currentScore >= pointsToNextRank){
		rank++;
		PlayAudioClip(rankSound, transform.position, 1.0);	
		pointsToNextRank += currentScore;
	}
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

function OnGUI(){
	if(!Screen.lockCursor) return;
	GUI.skin = mySkin;
	var style1 = mySkin.customStyles[0];
	
	GUI.Label (Rect(40, Screen.height - 80,100,60)," SCORE :");
	GUI.Label (Rect(100, Screen.height - 80,160,60),"" + currentScore, style1);
	
	GUI.Label (Rect(40, Screen.height - 110,100,60)," LVL :");
	GUI.Label (Rect(100, Screen.height - 110,160,60),"" + rank, style1);
	
	GUI.color = Color(1.0, 1.0, 1.0, alphaHit);
	GUI.DrawTexture (Rect ((Screen.width - hitCrosshairTexture.width)/2, (Screen.height - hitCrosshairTexture.height)/2, hitCrosshairTexture.width, hitCrosshairTexture.height), hitCrosshairTexture);
}	