using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public float life = 1.0f;
    public float speed = 10.0f;
    private float timeToDie = 0.0f;

    void Update() {
        CountDown();
        AutoMove();
    }

    public void Activate() {
        timeToDie = Time.time + life;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    public void Activate(Vector3 pos, Quaternion rot) {
        timeToDie = Time.time + life;
        transform.position = pos;
        transform.rotation = rot;
    }

    private void Deactivate() {
        this.gameObject.SetActive(false);
    }

    private void CountDown() {
        if (timeToDie < Time.time) {
            Deactivate();
        }
    }

    private void AutoMove() {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
