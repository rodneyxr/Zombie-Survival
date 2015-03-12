using UnityEngine;
using System.Collections;

public class GameEngine : MonoBehaviour {

    public WaveManager waveManager;
    public static bool paused = false;

    private int wave = 0;

    void Start() {
       //SetPaused(false);
        AI.waveManager = waveManager;
        StartWave(1);
    }

    void Update() {
    }

    void StartWave(int wave) {
        this.wave = wave;
        waveManager.StartWave(wave);
    }

    public void EndWave() {
        wave++;
        StartWave(wave);
    }

    //public void SetPaused(bool paused) {

    //}

    public static void SetPaused(bool paused) {
        GameEngine.paused = paused;
        HUD.SetPausedPanelActive(paused);
        if (paused) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        } else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = false;
        }
    }

    public void MainMenu() {
        Application.LoadLevel("MainMenu");
    }

}
