using UnityEngine;
using System.Collections;

public class FallingWall : MonoBehaviour {

    private WaveManager waveManager;
    private Player player;

    public GameObject forceField;
    public GameObject obsticle;
    public GameObject wall;
    public GameObject glass;
    public AudioSource gameChangerClip;
    private BoxCollider boxTrigger;

    // Economy
    public int price = 500;

    private bool canPush = false;

    void Start() {
        waveManager = GameObject.Find("_Main").GetComponent<WaveManager>();
        boxTrigger = GetComponent<BoxCollider>();
    }

    void Update() {
        if (canPush && Input.GetKeyDown(KeyCode.E)) {
            if (player.ChargeMoney(price)) {
                Push();
                PlayerMessage.HideMessage();
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            player = other.GetComponent<Player>();
            if (player.Money < price) {
                PlayerMessage.DisplayMessage("You need $" + price + " to open this wall");
                canPush = false;
            } else {
                PlayerMessage.DisplayMessage("Press 'E' to push the wall for $" + price);
                canPush = true;
            }
        }
    }

    void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")) {
            if (player.Money < price != !canPush) {
                if (player.Money < price) {
                    PlayerMessage.DisplayMessage("You need $" + price + " to open this wall");
                    canPush = false;
                } else {
                    PlayerMessage.DisplayMessage("Press 'E' to push the wall for $" + price);
                    canPush = true;
                }
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            player = null;
            canPush = false;
            PlayerMessage.HideMessage();
        }
    }

    void Push() {
        canPush = false;
        wall.GetComponent<Rigidbody>().isKinematic = false;
        wall.GetComponent<Rigidbody>().WakeUp();
        PlayerMessage.HideMessage();
        StartCoroutine(DelayedPush());
    }

    IEnumerator DelayedPush() {
        yield return StartCoroutine(Wait(1.5f));
        while (wall.GetComponent<Rigidbody>().velocity.magnitude > .2f) {
            yield return StartCoroutine(Wait(.2f));
        }
        GameObject.Destroy(forceField);
        GameObject.Destroy(glass);
        GameObject.Destroy(obsticle);
        GameObject.Destroy(boxTrigger);
        wall.GetComponent<Rigidbody>().isKinematic = true;
        gameChangerClip.Play();
        waveManager.GameChanger();
    }

    IEnumerator Wait(float duration) {
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
            yield return 0;
    }
}
