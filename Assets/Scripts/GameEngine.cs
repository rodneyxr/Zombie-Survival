using UnityEngine;
using System.Collections;

public class GameEngine : MonoBehaviour {

    public WaveManager waveManager;
    public static bool paused = false;

    private int wave = 0;

    void Start() {
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



}
