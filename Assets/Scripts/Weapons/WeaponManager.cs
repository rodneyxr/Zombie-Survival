using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour {

    public int maxWeapons = 2;
    public int initialWeaponIndex = 0;
    private List<Gun> weapons;

    private Gun currentWeapon;
    private int cIndex; // current index 

    void Start() {
        weapons = new List<Gun>(maxWeapons);
        foreach (Gun o in GetComponentsInChildren(typeof(Gun), true)) {
            //print(o.name);
            PickUpWeapon(o.gameObject);
        }

        if (weapons.Count > 0) {
            for (int i = 0; i < weapons.Count; i++) {
                weapons[i].gameObject.SetActive(false);
            }
        }

        cIndex = initialWeaponIndex;
        currentWeapon = weapons[cIndex];
        SwitchWeapon(initialWeaponIndex);
    }

    public void SwitchWeapon() {
        SwitchWeapon(Next());
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
        //HUD.Weapon = currentWeapon.name;
    }

    public void DropWeapon(Gun weaponToDrop) {
        Vector3 dropPosition = transform.position;
        dropPosition.y = 0f;
        ItemDisplay itemDisplay = (Instantiate(Resources.Load("Prefabs/ItemDisplay"), dropPosition, Quaternion.identity) as GameObject).GetComponent<ItemDisplay>();
        itemDisplay.Init(weaponToDrop.gameObject, 30f, true);
    }

    private Gun SwapCurrentWeapon(Gun newWeapon) {
        Gun oldWeapon = currentWeapon;
        weapons[cIndex] = newWeapon;
        currentWeapon = newWeapon;
        currentWeapon.Enable();
        //HUD.Weapon = currentWeapon.name;
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
