using UnityEngine;
using System.Collections;

public class FallingWall : MonoBehaviour {

    private WaveManager waveManager;

    public GameObject forceField;
    public GameObject obsticle;
    public GameObject wall;
    public GameObject glass;
    public AudioSource gameChangerClip;
    private BoxCollider boxTrigger;

    private bool canPush = false;

    void Start() {
        waveManager = GameObject.Find("_Main").GetComponent<WaveManager>();
        boxTrigger = GetComponent<BoxCollider>();
    }

    void Update() {
        if (canPush && Input.GetKeyDown(KeyCode.E)) {
            Push();
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            canPush = true;
            PlayerMessage.DisplayMessage("Press 'E' to push the wall");
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            canPush = false;
            PlayerMessage.HideMessage();
        }
    }

    void Push() {
        canPush = false;
        wall.rigidbody.isKinematic = false;
        wall.rigidbody.WakeUp();
        PlayerMessage.HideMessage();
        StartCoroutine(DelayedPush());
    }

    IEnumerator DelayedPush() {
        yield return StartCoroutine(Wait(1.5f));
        while (wall.rigidbody.velocity.magnitude > .2f) {
            yield return StartCoroutine(Wait(.2f));
        }
        GameObject.Destroy(forceField);
        GameObject.Destroy(glass);
        GameObject.Destroy(obsticle);
        GameObject.Destroy(boxTrigger);
        gameChangerClip.Play();
        waveManager.GameChanger();
    }

    IEnumerator Wait(float duration) {
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
            yield return 0;
    }
}
