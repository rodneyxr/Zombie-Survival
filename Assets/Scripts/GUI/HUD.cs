using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {

    public Text textAmmo;
    public Text textWave;
    public Text textZombiesLeft;

    private static Text ammo;
    private static Text wave;
    private static Text zombiesLeft;

    void Awake() {
        ammo = textAmmo;
        wave = textWave;
        zombiesLeft = textZombiesLeft;
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

}
