using UnityEngine;
using System.Collections;

public class SFX : MonoBehaviour {

    private static AudioSource sound;

    public AudioClip money;
    public AudioClip ammo;
    public AudioClip powerup;

    private static AudioClip moneySound;
    private static AudioClip ammoSound;
    private static AudioClip powerupSound;

    void Start() {
        sound = GetComponent<AudioSource>();
        moneySound = money;
        ammoSound = ammo;
        powerupSound = powerup;
    }

    public static void PlayMoneySound() {
        sound.PlayOneShot(moneySound, 1f);
    }

    public static void PlayAmmoSound() {
        sound.PlayOneShot(ammoSound, 1f);
    }

    public static void PlayPowerUpSound() {
        sound.PlayOneShot(powerupSound, 1f);
    }

}
