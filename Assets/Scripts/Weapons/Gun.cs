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
        ObjectPool.ActivateProjectile(bulletTransform.position, Quaternion.LookRotation(point - bulletTransform.position, Vector3.up));
    }

    public void Fire(RaycastHit hit) {
        Fire();
        ObjectPool.ActivateProjectile(bulletTransform.position, Quaternion.LookRotation(hit.point - bulletTransform.position, Vector3.up));
        hit.collider.SendMessage("Damage", power, SendMessageOptions.DontRequireReceiver);
    }

    public virtual bool canFire() {
        return timeToFire < Time.time;
    }

    public virtual float Range {
        get { return range; }
    }

}
