using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

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
            for (int i = 0; i < weapons.Length; i++) {
                if (i != initialWeaponIndex)
                    weapons[i].SetActive(false);
            }
        }
    }

    public bool Fire() {
        if (!gun.canFire()) return false;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, gun.Range)) {
            gun.Fire(hit);
        } else { // hit nothing
            Vector3 point = ray.origin + (ray.direction * gun.Range);
            gun.Fire(point);
        }
        return true;
    }

    void Update() {

    }

    public Gun Gun {
        get { return gun; }
    }
}
