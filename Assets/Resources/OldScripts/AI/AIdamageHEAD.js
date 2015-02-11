var hitPoints = 100.0;


function ApplyDamage (damage : float) {
	// We already have less than 0 hitpoints, maybe we got killed already?
	if (hitPoints <= 0.0)
		return;

	hitPoints -= damage;
	if (hitPoints <= 0.0){
		yield WaitForSeconds (0.2);
		SendMessageUpwards("Replace");
	}
}

