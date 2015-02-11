var hitPoints : float = 50.0;
var sound : AudioClip;

function OnTriggerEnter (other : Collider) { 
	if (other.CompareTag ("Player")){
		other.SendMessageUpwards("Medic", hitPoints, SendMessageOptions.DontRequireReceiver);
		AudioSource.PlayClipAtPoint(sound, transform.position);
		Destroy(gameObject);
	}
}