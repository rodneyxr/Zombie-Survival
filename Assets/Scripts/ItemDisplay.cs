using UnityEngine;
using System.Collections;

// TODO: stop gun cloning when passed from weapon manager
// TODO: implement holdE and playerMessage
public class ItemDisplay : MonoBehaviour {

    private const float ROTATION_SPEED = -90f;

    public GameObject item;
    public float life = 30f;
    public bool holdE = false;

    private Transform featureTransform;
    private GameObject feature;

    private float timeToDestroy;
    private bool init = false;

    void Start() {
        featureTransform = transform.FindChild("Feature");
        gameObject.SetActive(false);
        if (item != null) {
            feature = Instantiate(item, featureTransform.position, featureTransform.rotation) as GameObject;
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
    }

    void Update() {
        // Check if time is up
        if (timeToDestroy <= Time.time) {
            Destroy(gameObject);
            return;
        }

        // Rotate
        feature.transform.Rotate(new Vector3(0f, ROTATION_SPEED * Time.deltaTime, 0f));
    }

    void OnTriggerEnter(Collider other) {
        print("ItemDisplay: hit " + other.name);
        if (other.tag.Equals("Player")) {
            if (holdE) return;
            Player player = other.GetComponent<Player>();

            switch (feature.tag) {
                case "Weapon":
                    player.PickUp(feature);
                    break;
            }

            // reset the feature
            feature.transform.localScale = item.transform.localScale;
            feature.transform.localPosition = item.transform.localPosition;
            feature.transform.localRotation = item.transform.localRotation;

            // destroy the ItemDisplay
            // Note: if the feature item's parent is not changed the feature item will be deleted
            Destroy(gameObject);
        }
    }
}
