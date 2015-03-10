﻿using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    // Sound
    private AudioSource sound;
    public AudioClip soundReload;
    public AudioClip soundShoot;
    public AudioClip soundEmptyClip;

    // Settings
    public Transform bulletTransform;
    public int power = 10;
    public float range = 100f;
    public float fireDelay = 1f;
    public int clipSize = 10;
    public int maxAmmo = 100;
    public int defaultAmmo = 30;
    public bool isAutomatic = false;

    // local variables
    private int ammo = 0;
    private int clip = 0;
    private float timeToFire = 0f;

    // IK
    public Transform IK_LEFT_HAND;
    //public Transform IK_RIGHT_HAND;

    // Reload
    private bool reloading = false;
    private Coroutine reloadRoutine;

    void Start() {
        sound = GetComponent<AudioSource>();
        clip = clipSize;
        ammo = defaultAmmo;
        updateAmmo();
    }

    public void StartReload() {
        if (clip >= clipSize || ammo < 1 || reloading || reloadRoutine != null) { // clip is already full
            return;
        }

        PlayerController.AnimateReload();
        sound.PlayOneShot(soundReload);
        reloading = true;
        reloadRoutine = StartCoroutine(DelayedReload());
    }

    public void StopReload() {
        if (reloadRoutine == null) return;
        reloading = false;
        StopCoroutine(reloadRoutine);
        reloadRoutine = null;
        IK.ikLeftHandWeight = 1f;
        PlayerController.AnimateCancelReload();
        sound.Stop();
    }

    private void Reload() {
        reloading = false;
        reloadRoutine = null;
        if (ammo < 1) {
            StopReload();
            return;
        }

        IK.ikLeftHandWeight = 1f;
        int ammoReload = Mathf.Min(ammo, clipSize - clip);
        clip += ammoReload;
        ammo -= ammoReload;
        updateAmmo();
    }

    public void AttemptToFire() {
        if (!FireReady) return;
        if (EmptyClip) {
            SignalEmptyClip();
            return;
        }
        if (reloading) StopReload();
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Range)) {
            Fire(hit);
        } else { // hit nothing
            Vector3 point = ray.origin + (ray.direction * Range);
            Fire(point);
        }
    }

    private void Fire() {
        timeToFire = Time.time + fireDelay;
        PlayerController.AnimateShoot();
        sound.PlayOneShot(soundShoot, 1f);
        clip--;
        updateAmmo();
    }

    public void Fire(Vector3 point) {
        Fire();
        BulletPool.ActivateBullet(bulletTransform.position, Quaternion.LookRotation(point - bulletTransform.position, Vector3.up));
    }


    public void Fire(RaycastHit hit) {
        Fire();
        BulletPool.ActivateBullet(bulletTransform.position, Quaternion.LookRotation(hit.point - bulletTransform.position, Vector3.up));
        if (hit.transform.CompareTag("Enemy"))
            hit.collider.SendMessage("Damage", power, SendMessageOptions.DontRequireReceiver);
    }

    private void SignalEmptyClip() {
        sound.PlayOneShot(soundEmptyClip, 1f);
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

    public bool Reloading {
        get { return reloading; }
    }

    IEnumerator DelayedReload() {
        // wait for animation to finish
        yield return StartCoroutine(Wait(1.0f));
        IK.ikLeftHandWeight = .5f;
        yield return StartCoroutine(Wait(1.0f));
        IK.ikLeftHandWeight = 1f;
        yield return StartCoroutine(Wait(1.2f));
        if (reloading) Reload();
    }

    IEnumerator Wait(float duration) {
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
            yield return 0;
    }

}
//create hit particle here
//Particle particleClone = Instantiate(par, hit.point, Quaternion.LookRotation(hit.normal));
//Destroy(particleClone, 2);
