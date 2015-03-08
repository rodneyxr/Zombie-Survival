using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    public Transform bulletTransform;
    public int power = 10;
    public float range = 100f;
    public float fireDelay = 1f;
    public int clipSize = 10;
    public int maxAmmo = 100;
    public int defaultAmmo = 30;
    public bool isAutomatic = false;

    private GunSFX gunSFX;
    private int ammo = 0;
    private int clip = 0;
    private float timeToFire = 0f;

    void Start() {
        gunSFX = GetComponentInChildren<GunSFX>();
        clip = clipSize;
        ammo = defaultAmmo;
        updateAmmo();
    }

    void Update() {
    }

    public void reload() {

    }

    public bool AttemptToFire() {
        if (!FireReady) return false;
        if (EmptyClip) {
            SignalEmptyClip();
            return false;
        }
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Range)) {
            Fire(hit);
        } else { // hit nothing
            Vector3 point = ray.origin + (ray.direction * Range);
            Fire(point);
        }
        return true;
    }

    private void Fire() {
        timeToFire = Time.time + fireDelay;
        gunSFX.shootSound.Play();
        clip--;
        updateAmmo();
    }

    public void Fire(Vector3 point) {
        Fire();
        BulletPool.ActivateBullet(bulletTransform.position, Quaternion.LookRotation(point - bulletTransform.position, Vector3.up));
    }

    //create hit particle here
    //Particle particleClone = Instantiate(par, hit.point, Quaternion.LookRotation(hit.normal));
    //Destroy(particleClone, 2);

    public void Fire(RaycastHit hit) {
        Fire();
        BulletPool.ActivateBullet(bulletTransform.position, Quaternion.LookRotation(hit.point - bulletTransform.position, Vector3.up));
        if (hit.transform.CompareTag("Enemy"))
            hit.collider.SendMessage("Damage", power, SendMessageOptions.DontRequireReceiver);
    }

    private void SignalEmptyClip() {
        gunSFX.emptyClipSound.Play();
        timeToFire = Time.time + fireDelay;
    }

    public bool EmptyClip {
        get { return clip < 1; }
    }

    public bool FireReady {
        get { return timeToFire < Time.time; }
    }

    public void updateAmmo() {
        HUD.Ammo = "Ammo: " + clip + " / " + ammo;
    }

    public float Range {
        get { return range; }
    }

}
