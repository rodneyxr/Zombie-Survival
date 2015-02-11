
var key = KeyCode.Mouse1;  
var grabPower = 10.0; 
var throwPower = 25.0;
var hit : RaycastHit;
var RayDistance : float = 3.0;
var layerMask : LayerMask;
private var Grab : boolean = false;
private var Throw : boolean = false;
var offset : float = 2.0;


function Update (){ 
	if (Input.GetKeyDown(key)){
		//var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		//var rayOffset = Vector3 (ray.origin.x, ray.origin.y, ray.origin.z - offset);
		Physics.Raycast(transform.position, transform.forward, hit, RayDistance, layerMask.value);
		Debug.DrawLine (transform.position, hit.point, Color.green);
		if(hit.rigidbody){
			Grab = true;
		}			
	}			
	
	if (Input.GetKeyUp(key)){ 
		if(Grab){
			Grab = false; 
			Throw = true;
		}	
    }

	if(Grab){
		if(hit.rigidbody){
			hit.rigidbody.velocity = (transform.position - (hit.transform.position + hit.rigidbody.centerOfMass))* grabPower; 			
		}
	}
	
	if(Throw){
		if(hit.rigidbody){
			hit.rigidbody.velocity = transform.forward * throwPower;
			Throw = false;
		}
	}	
} 
 