using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Player : Character {

    public HealthBar healthBar;
    public float maxHealth = 100f;

    public float regenAmount = 10f;
    public float poisonAmount = 10f;
    public bool poisoned = false;
    private float regenDelay = 5f;
    private float timeToRegen = 0f;

    private PlayerController playerController;

    private Barricade barricade = null; // current barricade the player is at

    void Start() {
        playerController = GetComponent<PlayerController>();
        health = maxHealth;
        //Damage(100);
    }

    void Update() {
        if (timeToRegen < Time.time) {
            timeToRegen = Time.time + regenDelay;
            if (poisoned) {
                Damage(poisonAmount);
            } else {
                Regen(regenAmount);
            }
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
        switch (other.tag) {
            case "Barricade":
                if (barricade == null || PlayerMessage.Enabled == barricade.NeedsRepair) return;
                if (barricade.NeedsRepair)
                    PlayerMessage.DisplayMessage("Press 'E' to repair the barricade");
                else
                    PlayerMessage.HideMessage();
                break;

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

    public void PickUp(GameObject item) {
        switch (item.tag) {
            case "Weapon":
                playerController.weaponManager.PickUpWeapon(item);
                break;

            default:
                break;
        }
    }

    public void Regen(float health) {
        this.health = Mathf.Min(this.health + health, maxHealth);
        healthBar.UpdateHealthBar();
    }

    public override void Damage(float damage) {
        base.Damage(damage);
        playerController.AnimateHit();
        healthBar.UpdateHealthBar();
    }

    public override void OnDeath() {
        print("Player: Player OnDeath()");
        playerController.Die();
    }

    public Barricade CurrentBarricade {
        get { return barricade; }
    }


}
