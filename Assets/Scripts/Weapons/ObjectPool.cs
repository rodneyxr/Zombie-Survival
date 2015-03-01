using UnityEngine;
using System.Collections;

public class ObjectPool : MonoBehaviour {

    public int poolSize = 0;
    private static GameObject[] projectiles = null;
    private Weapon weapon;

    void Start() {
        weapon = GetComponent<Weapon>();
        projectiles = new GameObject[poolSize];
        InstantiateProjectiles();
    }

    private void InstantiateProjectiles() {
        for (int i = 0; i < projectiles.Length; i++) {
            projectiles[i] = Instantiate(Resources.Load("Prefabs/bullet")) as GameObject;
            projectiles[i].SetActive(false);
        }
    }

    private void ActivateProjectile() {
        for (int i = 0; i < projectiles.Length; i++) {
            if (!projectiles[i].activeSelf) {
                projectiles[i].SetActive(true);
                projectiles[i].GetComponent<Projectile>().Activate();
                return;
            }
        }
    }

    public static void ActivateProjectile(Vector3 position, Quaternion rotation) {
        for (int i = 0; i < projectiles.Length; i++) {
            if (!projectiles[i].activeSelf) {
                projectiles[i].SetActive(true);
                projectiles[i].GetComponent<Projectile>().Activate(position, rotation);
                return;
            }
        }
    }
}
