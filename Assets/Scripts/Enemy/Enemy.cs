using UnityEngine;
using System.Collections;

public class Enemy : Character {

    public int initialHealth = 100;
    //private Animator anim;

    void Start() {
        //anim = GetComponentInChildren<Animator>();
        health = initialHealth;
    }

    void Update() {
        //print(string.Format("Enemy Health: {0}", health));
    }

    public override void OnDeath() {
        print(name + "OnDeath.");
        Destroy(this.gameObject);
    }
}
