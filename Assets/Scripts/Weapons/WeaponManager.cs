using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour {

    public GameObject[] weapons;
    public int initialWeaponIndex;
    private Gun gun;

    // Animation
    public Animator anim;
    private int hashShoot = Animator.StringToHash("Shoot");

    void Start() {
        try {
            if (initialWeaponIndex < 0 || initialWeaponIndex >= weapons.Length)
                throw new System.Exception("Weapon Index is out of Range.");
        } catch (System.Exception ex) {
            Debug.LogError(ex.Message);
            initialWeaponIndex = 0;
        }

        if (weapons.Length > 0) {
            weapons[initialWeaponIndex].SetActive(true);
            gun = weapons[initialWeaponIndex].GetComponent<Gun>();
            for (int i = 0; i < weapons.Length; i++) {
                if (i != initialWeaponIndex)
                    weapons[i].SetActive(false);
            }
        }
    }

    public bool Fire() {
        return gun.AttemptToFire();
    }

    public Gun Gun {
        get { return gun; }
    }
}
