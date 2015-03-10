using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour {

    public IK ik;
    public int initialWeaponIndex;
    public GameObject[] weapons;
    private Gun[] guns;
    private Gun currentGun;
    private int cIndex; // current index 

    void Start() {
        try {
            if (initialWeaponIndex < 0 || initialWeaponIndex >= weapons.Length)
                throw new System.Exception("Weapon Index is out of Range.");
        } catch (System.Exception ex) {
            Debug.LogError(ex.Message);
            initialWeaponIndex = 0;
        }

        guns = new Gun[weapons.Length];

        //if (weapons.Length > 0) {
        //    weapons[initialWeaponIndex].SetActive(true);
        //    cGun = weapons[initialWeaponIndex].GetComponent<Gun>();
        //    ik.Gun = cGun;
        //    IK.ikLeftHandWeight = 1f;

        //    for (int i = 0; i < weapons.Length; i++) {
        //        if (i != initialWeaponIndex)
        //            weapons[i].SetActive(false);
        //    }
        //}

        if (weapons.Length > 0) {
            for (int i = 0; i < weapons.Length; i++) {
                guns[i] = weapons[i].GetComponent<Gun>();
                weapons[i].SetActive(false);
            }
        }
        cIndex = initialWeaponIndex;
        currentGun = guns[cIndex];
        SwitchWeapon(initialWeaponIndex);
    }

    public void SwitchWeapon(int index) {
        try {
            if (index < 0 || index >= weapons.Length)
                throw new System.Exception("Weapon Index is out of Range.");
        } catch (System.Exception ex) {
            Debug.LogError(ex.Message);
            return;
        }

        // disable last weapon
        if (index != cIndex) {
            //weapons[cIndex].SetActive(false);
            currentGun.Disable();
        }

        // enable new weapon
        cIndex = index;

        //weapons[cIndex].SetActive(true);
        currentGun = guns[cIndex];
        currentGun.Enable();
        //IK.Gun = currentGun;
        //IK.ikLeftHandWeight = 1f;
    }

    private int Next() {
        int newIndex = cIndex + 1;
        if (newIndex >= weapons.Length) {
            newIndex = 0;
        }
        return newIndex;
    }



    public void Fire() {
        currentGun.AttemptToFire();
    }

    public void Reload() {
        currentGun.StartReload();
    }

    public void StopReload() {
        currentGun.StopReload();
    }

    public Gun Gun {
        get { return currentGun; }
    }
}
