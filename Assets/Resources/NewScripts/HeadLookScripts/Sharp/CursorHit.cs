using UnityEngine;
using System.Collections;

public class CursorHit : MonoBehaviour {
	
	public HeadLookController headLookVehicle;
	public HeadLookController headLookHelicopter;

	// Update is called once per frame
	void LateUpdate () {
		
		headLookVehicle.target = transform.position;
		headLookHelicopter.target = transform.position;
	}
}
