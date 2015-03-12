using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDisplay : MonoBehaviour {

    private static List<ItemDisplay> itemDisplays = new List<ItemDisplay>();
    private const float ROTATION_SPEED = -90f;

    public GameObject item;
    public float life = 30f; // -1 for infinite
    public bool holdE = false;
    public int price = 0;

    private Transform featureTransform;
    private GameObject feature;

    private float timeToDestroy;
    private bool init = false;

    private Player player = null;
    private bool isInteracting = false;
    private float holdDuration = 0f;
    private const float timeToHold = 1f;

    private Vector3 origPosition;
    private Vector3 origScale;
    private Quaternion origRotation;

    void Start() {
        featureTransform = transform.FindChild("Feature");
        gameObject.SetActive(false);
        if (item != null) {
            origPosition = item.transform.localPosition;
            origScale = item.transform.localScale;
            origRotation = item.transform.localRotation;
            feature = Instantiate(item, featureTransform.position, featureTransform.rotation) as GameObject;
            if (init) Destroy(item);
            feature.SetActive(true);
            feature.transform.SetParent(transform);
            timeToDestroy = Time.time + life;
            gameObject.SetActive(true);
        }
    }

    public void Init(GameObject item) {
        Init(item, this.life);
    }

    public void Init(GameObject item, float life) {
        Init(item, life, this.holdE);
    }

    public void Init(GameObject item, float life, bool holdE) {
        this.item = item;
        this.life = life;
        this.holdE = holdE;
        feature = item;
        init = true;
    }

    void Update() {
        if (GameEngine.paused && life != -1) {
            timeToDestroy += Time.deltaTime;
            return;
        }

        // Check if time is up
        if (life != -1 && timeToDestroy <= Time.time) {
            DestroyItemDisplay();
            return;
        }

        if (isInteracting && canBuy) {
            if (Input.GetKey(KeyCode.E)) {
                holdDuration += Time.deltaTime;
                if (holdDuration >= timeToHold) {
                    if (player.ChargeMoney(price))
                        Interact();
                }
            } else {
                holdDuration = 0f;
            }
        }

    }

    void LateUpdate() {
        // Rotate
        feature.transform.Rotate(new Vector3(0f, ROTATION_SPEED * Time.deltaTime, 0f));
    }

    void DestroyItemDisplay() {
        if (player != null) RemoveItemDisplay(this);
        Destroy(gameObject);
    }

    void Interact() {
        print("Interact: name: " + feature.name + " tag: " + feature.tag);
        switch (feature.tag) {
            case "Weapon":
                player.PickUp(feature);
                break;
        }

        // reset the feature
        feature.transform.localScale = origScale;
        feature.transform.localPosition = origPosition;
        feature.transform.localRotation = origRotation;

        // destroy the ItemDisplay
        // Note: if the feature item's parent is not changed the feature item will be deleted
        DestroyItemDisplay();
    }
    bool canBuy = false;
    void OnTriggerEnter(Collider other) {
        //print("ItemDisplay: hit " + other.name);
        if (other.tag.Equals("Player")) {
            player = other.GetComponent<Player>();
            if (holdE) {
                itemDisplays.Add(this);
                if (player.Money < price) {
                    PlayerMessage.DisplayMessage("You need $" + price + " for this item");
                    canBuy = false;
                } else {
                    PlayerMessage.DisplayMessage("Hold 'E' to buy for $" + price);
                    canBuy = true;
                }
                return;
            }
            Interact();
        }
    }

    void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")) {
            if (!holdE || itemDisplays.Count == 0) return;
            isInteracting = holdE && itemDisplays[0] == this;
            if (player.Money < price != !canBuy) {
                if (player.Money < price) {
                    PlayerMessage.DisplayMessage("You need $" + price + " for this item");
                    canBuy = false;
                } else {
                    PlayerMessage.DisplayMessage("Hold 'E' to buy for $" + price);
                    canBuy = true;
                }
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            RemoveItemDisplay(this);
            player = null;
        }
    }

    private void RemoveItemDisplay(ItemDisplay id) {
        if (itemDisplays.Remove(id)) PlayerMessage.HideMessage();
        isInteracting = false;
    }
}
