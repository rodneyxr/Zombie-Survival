using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : Character {

    public HealthBar healthBar;
    public float maxHealth = 100f;

    public float regenAmount = 10f;
    private float regenDelay = 5f;
    private float timeToRegen = 0f;

    private Barricade barricade = null; // current barricade the player is at

    void Start() {
        health = maxHealth;
    }

    void Update() {
        if (timeToRegen < Time.time) {
            timeToRegen = Time.time + regenDelay;
            Regen(regenAmount);
        }
    }

    void OnTriggerEnter(Collider other) {
        //print("Player: Enter " + other.name);
        switch (other.tag) {
            case "Barricade":
                barricade = other.GetComponent<Barricade>();
                if (barricade.NeedsRepair) {
                    PlayerMessage.DisplayMessage("Press 'E' to repair the barricade");
                }
                break;
        }
    }

    void OnTriggerStay(Collider other) {
        if (other.CompareTag("Barricade")) {
            if (barricade == null || PlayerMessage.Enabled == barricade.NeedsRepair) return;
            if (barricade.NeedsRepair)
                PlayerMessage.DisplayMessage("Press 'E' to repair the barricade");
            else
                PlayerMessage.HideMessage();
        }
    }

    void OnTriggerExit(Collider other) {
        //print("Player: Exit " + other.name);
        switch (other.tag) {
            case "Barricade":
                barricade = null;
                PlayerMessage.HideMessage();
                break;
        }
    }

    public void Regen(float health) {
        this.health = Mathf.Min(this.health + health, maxHealth);
        healthBar.UpdateHealthBar();
    }

    public override void Damage(float damage) {
        base.Damage(damage);
        healthBar.UpdateHealthBar();
    }

    public override void OnDeath() {
        print("Player: Player OnDeath()");
    }

    public Barricade CurrentBarricade {
        get { return barricade; }
    }

    

}
