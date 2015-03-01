using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    public GameObject[] weapons;
    private Gun gun;

    void Start() {
        if (weapons.Length > 0) {
            weapons[0].SetActive(true);
            gun = weapons[0].GetComponent<Gun>();
            for (int i = 1; i < weapons.Length; i++) {
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
