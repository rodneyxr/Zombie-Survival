using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {

    public Text textAmmo;
    public Text textWave;
    public Text textZombiesLeft;
    public Text textMoney;
    public GameObject panelPaused;

    private static Text ammo;
    private static Text wave;
    private static Text zombiesLeft;
    private static Text money;

    private static GameObject paused;



    void Awake() {
        ammo = textAmmo;
        wave = textWave;
        zombiesLeft = textZombiesLeft;
        money = textMoney;
        paused = panelPaused;
    }

    public static string Ammo {
        set { ammo.text = value; }
    }

    public static string Wave {
        set { wave.text = value; }
    }

    public static string ZombiesLeft {
        set { zombiesLeft.text = value; }
    }

    public static string Money {
        set { money.text = value; }
    }

    public static void TogglePausedPanel() {
        paused.SetActive(!paused.activeSelf);
    }

    public static void SetPausedPanelActive(bool active) {
        paused.SetActive(active);
    }
}
