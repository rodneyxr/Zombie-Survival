using UnityEngine;
using System.Collections;

public class BulletPool : MonoBehaviour {

    public int poolSize = 0;
    private static GameObject[] bullets = null;

    void Start() {
        bullets = new GameObject[poolSize];
        InstantiateBullets();
    }

    private void InstantiateBullets() {
        for (int i = 0; i < bullets.Length; i++) {
            bullets[i] = Instantiate(Resources.Load("Prefabs/bullet")) as GameObject;
            bullets[i].SetActive(false);
        }
    }

    private void ActivateBullet() {
        for (int i = 0; i < bullets.Length; i++) {
            if (!bullets[i].activeSelf) {
                bullets[i].SetActive(true);
                bullets[i].GetComponent<Bullet>().Activate();
                return;
            }
        }
    }

    public static void ActivateBullet(Vector3 position, Quaternion rotation) {
        for (int i = 0; i < bullets.Length; i++) {
            if (!bullets[i].activeSelf) {
                bullets[i].SetActive(true);
                bullets[i].GetComponent<Bullet>().Activate(position, rotation);
                return;
            }
        }
    }
}
