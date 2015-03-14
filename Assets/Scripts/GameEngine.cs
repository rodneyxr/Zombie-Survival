using UnityEngine;
using System.Collections;

public class GameEngine : MonoBehaviour {

    public WaveManager waveManager;
    public static bool paused = false;
    public static bool gameOver = false;

    private int wave = 0;

    void Start() {
        SetPaused(false);
        AI.waveManager = waveManager;
        StartWave(9);
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

    public void TogglePaused() {
        SetPaused(!paused);
    }

    public static void SetPaused(bool paused) {
        GameEngine.paused = paused;
        HUD.SetPausedPanelActive(paused);
        HUD.LockCursor(!paused);
    }

    public static void GameOver() {
        gameOver = true;
        SetPaused(true);
        HUD.SetGameOverPanelActive();
    }

    public void MainMenu() {
        Application.LoadLevel("MainMenu");
    }

}
