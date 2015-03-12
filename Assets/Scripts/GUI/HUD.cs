using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {

    public Text textAmmo;
    public Text textWave;
    public Text textZombiesLeft;
    public Text textMoney;
    public GameObject panelPaused;
    public GameObject panelControls;

    private static Text ammo;
    private static Text wave;
    private static Text zombiesLeft;
    private static Text money;
    private static GameObject paused;
    private static GameObject controls;

    void Awake() {
        ammo = textAmmo;
        wave = textWave;
        zombiesLeft = textZombiesLeft;
        money = textMoney;
        paused = panelPaused;
        controls = panelControls;
    }

    void Update() {
        // Pause
        if (Input.GetButtonDown("Cancel")) {
            if (GameEngine.paused) {
                Back();
            } else {
                GameEngine.SetPaused(true);
            }
        }
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

    public void SetControlsPanelActiveWrapper(bool active) {
        SetControlsPanelActive(active);
    }

    public static void SetControlsPanelActive(bool active) {
        controls.SetActive(active);
        SetPausedPanelActive(!active);
    }

    public static void LockCursor(bool locked) {
        if (locked) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public static void Back() {
        if (controls.activeSelf) {
            SetControlsPanelActive(false);
        } else if (paused.activeSelf) {
            GameEngine.SetPaused(false);
        }
    }
}
