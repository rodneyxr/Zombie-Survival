var sound : AudioClip[];

function OnCollisionEnter (collision : Collision) {
plauDropSound();
}

function plauDropSound (){
audio.clip = sound[Random.Range(0, sound.length)];
audio.volume = .7;
audio.Play();
}