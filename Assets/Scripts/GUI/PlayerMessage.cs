using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMessage : MonoBehaviour {

    private static Text msg;
    private float timeToDisplay;

    void Start() {
        msg = GetComponent<Text>();
        HideMessage();
    }

    void Update() {

    }

    public static void DisplayMessage(string message) {
        msg.enabled = true;
        msg.text = message;
    }

    public static void HideMessage() {
        msg.enabled = false;
    }

    public static bool Enabled {
        get { return msg.enabled; }
    } 
}
