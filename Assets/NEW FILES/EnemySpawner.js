
var spawnPoints : Transform[];  	// Array of spawn points to be used.
var enemyPrefabs : GameObject[]; 	// Array of different Enemies that are used.
var amountEnemies = 20;  			// Total number of enemies to spawn.
var yieldTimeMin = 2;  				// Minimum amount of time before spawning enemies randomly.
var yieldTimeMax = 5;  				// Don't exceed this amount of time between spawning enemies randomly.
var enemiesToSpawnAtTheSameTime : int = 1;
private var wait : boolean = false;
var disableRenderers : MeshRenderer[];

function Start(){
	for(var i : int = 0; i < disableRenderers.length; i++){
		disableRenderers[i].enabled = false;
	}
}

function OnTriggerStay (other : Collider) {
    if (other.CompareTag ("Player")) {
		Spawn();
	}
}



function Spawn(){ 
	if(wait) return;
	
	if(amountEnemies > 0){
		wait = true;
		yield WaitForSeconds(Random.Range(yieldTimeMin, yieldTimeMax));  // How long to wait before another enemy is instantiated.
		for(var s : int = 0; s < enemiesToSpawnAtTheSameTime; s++){
			var obj : GameObject = enemyPrefabs[Random.Range(0, enemyPrefabs.length)]; // Randomize the different enemies to instantiate.
			var pos: Transform = spawnPoints[Random.Range(0, spawnPoints.length)];  // Randomize the spawnPoints to instantiate enemy at next.
		
			Instantiate(obj, pos.position, pos.rotation);
		}
		amountEnemies --;	
		wait = false;
	}else{
		Destroy(gameObject);
	}	
}  