using UnityEngine;
using System.Collections;

public class Player : Character {

    public HealthBar healthBar;
    public int maxHealth = 100;

    public int regenAmount = 10;
    private float regenDelay = 5;
    private float timeToRegen = 0f;

    void Start() {
        health = maxHealth;
    }

    void Update() {
        if (timeToRegen < Time.time) {
            timeToRegen = Time.time + regenDelay;
            Regen();
        }
    }

    public void Regen(int health) {

    }

    private void Regen() {
        health = Mathf.Min(Health + regenAmount, maxHealth);
        healthBar.UpdateHealthBar();
    }

    public override void Damage(int damage) {
        base.Damage(damage);
        healthBar.UpdateHealthBar();
    }

    public override void OnDeath() {

    }
}
