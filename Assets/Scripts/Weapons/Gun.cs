using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    // Sound
    private AudioSource sound;
    public AudioClip soundReload;
    public AudioClip soundShoot;
    public AudioClip soundEmptyClip;

    // Settings
    public string weaponName;
    public Transform bulletTransform;
    public int power = 10;
    public float range = 100f;
    public float fireDelay = .5f;
    public float burstDelay = 0f;
    public int clipSize = 10;
    public int maxAmmo = 100;
    public int defaultAmmo = 30;
    public bool isAutomatic = false;
    public int burst = 1;

    // local variables
    private int ammo = 0;
    private int clip = 0;
    private float timeToFire = 0f;
    private Collider hitCollider;

    // IK
    public Transform IK_LEFT_HAND;
    //public Transform IK_RIGHT_HAND;

    // Reload
    private bool reloading = false;
    private Coroutine reloadRoutine;

    // Blood Splat
    public GameObject bloodSplat;

    // Muzzle Flash
    public ParticleEmitter muzzleFlash;
    public GameObject redLight;
    public GameObject orangeLight;
    public GameObject yellowLight;
    private float muzzleTimer = 0f;
    private float muzzleCooler = .1f;

    void Awake() {
        sound = GetComponent<AudioSource>();
        clip = clipSize;
        ammo = defaultAmmo;
        //UpdateAmmo();
    }

    void Update() {
        if (muzzleTimer < 0 && redLight.activeSelf) {
            redLight.SetActive(false);
            orangeLight.SetActive(false);
            yellowLight.SetActive(false);
        }
        muzzleTimer -= Time.deltaTime;
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
        UpdateAmmo();
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

    public void Disable() {
        StopReload();
        gameObject.SetActive(false);
    }

    public void Enable() {
        gameObject.SetActive(true);
        IK.gun = this;
        IK.ikLeftHandWeight = 1f;
        UpdateAmmo();
        HUD.Weapon = weaponName;
    }

    private void Fire() {
        timeToFire = Time.time + burstDelay;
        StartCoroutine(Burst());
    }

    IEnumerator Burst() {
        for (int shotsFired = 0; shotsFired < burst; shotsFired++) {
            if (muzzleTimer < 0) {
                muzzleFlash.Emit();
                redLight.SetActive(true);
                orangeLight.SetActive(true);
                yellowLight.SetActive(true);
                muzzleTimer = muzzleCooler;
            }

            PlayerController.AnimateShoot();
            sound.PlayOneShot(soundShoot, 1f);
            clip--;
            UpdateAmmo();
            if (hitCollider != null)
                hitCollider.SendMessage("Damage", power, SendMessageOptions.DontRequireReceiver);
            if (fireDelay == 0) break;
            yield return new WaitForSeconds(fireDelay);
        }
        hitCollider = null;
        yield return null;
    }

    public void Fire(Vector3 point) {
        BulletPool.ActivateBullet(bulletTransform.position, Quaternion.LookRotation(point - bulletTransform.position, Vector3.up));
        Fire();
    }

    public void Fire(RaycastHit hit) {
        BulletPool.ActivateBullet(bulletTransform.position, Quaternion.LookRotation(hit.point - bulletTransform.position, Vector3.up));
        if (hit.transform.CompareTag("Enemy")) {
            Instantiate(bloodSplat, hit.point, Quaternion.LookRotation(hit.normal));
            hitCollider = hit.collider;
        }
        Fire();
    }

    public void AddAmmo(int amount) {
        ammo = Mathf.Min(ammo + amount, maxAmmo);
        //UpdateAmmo();
    }

    private void SignalEmptyClip() {
        sound.PlayOneShot(soundEmptyClip, 1f);
        timeToFire = Time.time + fireDelay;
    }

    public bool EmptyClip {
        get { return clip < burst; }
    }

    public bool FireReady {
        get { return timeToFire < Time.time; }
    }

    public void UpdateAmmo() {
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
