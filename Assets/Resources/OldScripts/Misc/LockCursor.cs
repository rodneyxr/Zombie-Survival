using UnityEngine;
using System.Collections;

public class LockCursor : MonoBehaviour {

    void Start () {
	    // Start out in paused mode in web player
	    if (Application.platform == RuntimePlatform.OSXWebPlayer || Application.platform == RuntimePlatform.WindowsWebPlayer) {
		    SetPause(true);
	    } else {
		    SetPause(false);
		    Screen.lockCursor = true;
	    }
    }

    void OnApplicationQuit () {
	    Time.timeScale = 1;
    }

    void SetPause (bool pause) {
	    Input.ResetInputAxes();
	    DidPause(pause);
    	
	    transform.position = Vector3.zero;
    	
	    if (pause) {
		    Time.timeScale = 0;
		    transform.position = new Vector3 (.5f, .5f, 0);
		    guiText.anchor = TextAnchor.MiddleCenter;
	    } else {
		    guiText.anchor = TextAnchor.UpperLeft;
		    transform.position = new Vector3(0, 1, 0);
		    Time.timeScale = 1;
	    }
    }

    void DidPause (bool pause) {
	    if (pause) {
	        // Show the button again
	        guiText.enabled = true;
		    guiText.text = "Click to start playing";
	    } else {
	        // Disable the button
	        guiText.enabled = true;
	        guiText.text = "Escape to show the cursor";
	    }
    }

    void OnMouseDown () {
        // Lock the cursor
        Screen.lockCursor = true;
    }

    private bool wasLocked = false;

    void Update () {
	    if (Input.GetMouseButton(0))
		    Screen.lockCursor = true;
    	
        // Did we lose cursor locking?
        // eg. because the user pressed escape
        // or because he switched to another application
        // or because some script set Screen.lockCursor = false;
        if (!Screen.lockCursor && wasLocked) {
            wasLocked = false;
            SetPause(true);
	    // Did we gain cursor locking?
        } else if (Screen.lockCursor && !wasLocked) {
            wasLocked = true;
            SetPause(false);
        }
    }
}
