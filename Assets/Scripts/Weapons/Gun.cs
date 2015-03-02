using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    public int power = 10;
    public float range = 100f;
    public float fireDelay = 1f;
    public Transform bulletTransform;

    private float timeToFire = 0f;

    void Start() {

    }

    void Update() {

    }

    private void Fire() {
        timeToFire = Time.time + fireDelay;
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

    public virtual bool canFire() {
        return timeToFire < Time.time;
    }

    public virtual float Range {
        get { return range; }
    }

}
