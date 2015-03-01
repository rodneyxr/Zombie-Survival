using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour {

    private Player player; // the player who's health should be displayed
    private Text healthText; // text displaying health

    public RectTransform healthTransform; // rect of the masked image
    private Canvas canvas;
    private float cachedY; // save the position.y of the health transform so we don't have to keep accessing it
    private float minXValue; // save min x value of the rect
    private float maxXValue; // save max x value of the rect
    private float maxHealth; // player's max health
    private float minHealth; // player's min health

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        canvas = GetComponentInParent<Canvas>();
        healthText = GetComponentInChildren<Text>();
        cachedY = healthTransform.position.y;
        maxXValue = healthTransform.position.x;
        minXValue = maxXValue - healthTransform.rect.width * canvas.scaleFactor;

        minHealth = 0f;
        maxHealth = player.maxHealth;
    }

    public void UpdateHealthBar() {
        int health = player.Health;
        healthText.text = "Health: " + health;
        float currentXValue = MapValues(health);
        healthTransform.position = new Vector3(currentXValue, cachedY);
    }

    private float MapValues(float health) {
        return (health - minHealth) * (maxXValue - minXValue) / (maxHealth - minHealth) + minXValue;
    }
}
