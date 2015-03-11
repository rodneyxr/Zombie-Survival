using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour {

    public int maxWeapons = 2;
    public int initialWeaponIndex = 0;
    //private List<GameObject> weaponsOLD;
    private List<Gun> weapons;

    //public GameObject[] weapons;
    //private Gun[] guns;
    private Gun currentWeapon;
    private int cIndex; // current index 

    void Start() {
        //weaponsOLD = new List<GameObject>(numberOfWeapons);
        weapons = new List<Gun>(maxWeapons);
        //guns = new Gun[weapons.Length];

        //try {
        //    if (initialWeaponIndex < 0 || initialWeaponIndex >= weapons.Count)
        //        throw new System.Exception("Weapon Index is out of Range.");
        //} catch (System.Exception ex) {
        //    Debug.LogError(ex.Message);
        //    initialWeaponIndex = 0;
        //}
        //transform.Cast<Transform>().Where(c => c.gameObject.tag == "Weapon").ToArray()
        foreach (Gun o in GetComponentsInChildren(typeof(Gun), true)) {
            print(o.name);
            PickUpWeapon(o.gameObject);
        }

        if (weapons.Count > 0) {
            for (int i = 0; i < weapons.Count; i++) {
                //weapons[i] = weapons[i].GetComponent<Gun>();
                weapons[i].gameObject.SetActive(false);
            }
        }

        cIndex = initialWeaponIndex;
        currentWeapon = weapons[cIndex];
        SwitchWeapon(initialWeaponIndex);
    }

    public void SwitchWeapon(int index) {
        try {
            if (index < 0 || index >= weapons.Count)
                throw new System.Exception("Weapon Index is out of Range.");
        } catch (System.Exception ex) {
            Debug.LogError(ex.Message);
            return;
        }

        // disable last weapon
        if (index != cIndex) {
            currentWeapon.Disable();
        }

        // enable new weapon
        cIndex = index;
        currentWeapon = weapons[cIndex];
        currentWeapon.Enable();
    }

    public void SwitchWeapon() {
        SwitchWeapon(Next());
    }

    public void DropWeapon(Gun weaponToDrop) {
        //Gun weaponToDrop = currentWeapon;
        //SwitchWeapon();
        //weapons.Remove(weaponToDrop);

        Vector3 dropPosition = transform.position;
        dropPosition.y = 0f;

        ItemDisplay iDisplay = (Instantiate(Resources.Load("Prefabs/ItemDisplay"), dropPosition, Quaternion.identity) as GameObject).GetComponent<ItemDisplay>();
        iDisplay.Init(weaponToDrop.gameObject, 30f, true);
    }

    private Gun SwapCurrentWeapon(Gun newWeapon) {
        Gun oldWeapon = currentWeapon;
        weapons[cIndex] = newWeapon;
        return oldWeapon;
    }

    public void PickUpWeapon(GameObject weapon) {
        Gun newWeapon = weapon.GetComponent<Gun>();

        // we can add weapons without swapping
        if (weapons.Count < maxWeapons) {
            weapons.Add(newWeapon);
            SwitchWeapon(weapons.Count - 1);
        } else { // we need to swap the weapons
            Gun weaponToDrop = SwapCurrentWeapon(newWeapon);
            DropWeapon(weaponToDrop);
        }

        weapon.transform.SetParent(transform);
    }

    private int Next() {
        int newIndex = cIndex + 1;
        if (newIndex >= weapons.Count) {
            newIndex = 0;
        }
        return newIndex;
    }

    public void Fire() {
        currentWeapon.AttemptToFire();
    }

    public void Reload() {
        currentWeapon.StartReload();
    }

    public void StopReload() {
        currentWeapon.StopReload();
    }

    public Gun Gun {
        get { return currentWeapon; }
    }
}
