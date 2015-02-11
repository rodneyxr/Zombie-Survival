var GO : GameObject;

function ApplyDamage(){
	Action ();
}

function Action(){
//gameObject.BroadcastMessage("Action", SendMessageOptions.DontRequireReceiver);
GO.SendMessage("Action", SendMessageOptions.DontRequireReceiver);

}