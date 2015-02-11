
var destination : Transform[];
var Player : GameObject; 

function SpawnPlayer(){
	var player : GameObject = Instantiate (Player, Vector3.zero, Quaternion.identity);
	player.name = "Player";
    player.transform.position = destination[Random.Range(0, destination.Length)].transform.position;
	gameObject.GetComponent(MeshRenderer).enabled = false;	
}
