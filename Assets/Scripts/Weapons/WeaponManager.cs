using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour {

    public IK ik;
    public GameObject[] weapons;
    public int initialWeaponIndex;
    private Gun gun;

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
            ik.Gun = gun;
            IK.ikLeftHandWeight = 1f;

            for (int i = 0; i < weapons.Length; i++) {
                if (i != initialWeaponIndex)
                    weapons[i].SetActive(false);
            }
        }
    }

    public void Fire() {
        gun.AttemptToFire();
    }

    public void Reload() {
        gun.StartReload();
    }

    public void StopReload() {
        gun.StopReload();
    }

    public Gun Gun {
        get { return gun; }
    }
}
