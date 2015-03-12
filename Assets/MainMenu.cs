using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public void StartGame() {
        Application.LoadLevel("Level01");
    }

    public void Quit() {
        Application.Quit();
    }

}
