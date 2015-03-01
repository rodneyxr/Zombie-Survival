using UnityEngine;
using System.Collections;

public class Crosshairs : MonoBehaviour {

    public Texture2D crosshairImage;

    void OnGUI() {
        float scaleFactor = Mathf.Max(Screen.width, Screen.height) / 1080f;
        float imgWidth = crosshairImage.width * scaleFactor;
        float imgHeight = crosshairImage.height * scaleFactor;
        float xMin = (Screen.width / 2) - (imgWidth / 2);
        float yMin = (Screen.height / 2) - (imgHeight / 2);
        GUI.DrawTexture(new Rect(xMin, yMin, imgWidth, imgHeight), crosshairImage);
    }
}
