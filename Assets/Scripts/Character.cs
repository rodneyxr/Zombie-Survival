using UnityEngine;
using System.Collections;

public abstract class Character : MonoBehaviour, ICharacter {

    protected int health;

    public virtual void Damage(int damage) {
        health -= damage;
        if (health <= 0) {
            health = 0;
            OnDeath();
        }
    }

    public abstract void OnDeath();

    public int Health {
        get { return health; }
    }
}
