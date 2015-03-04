using UnityEngine;
using System.Collections;

public abstract class Character : MonoBehaviour, ICharacter {

    protected float health;

    public virtual void Damage(float damage) {
        health -= damage;
        if (health <= 0) {
            health = 0;
            OnDeath();
        }
    }

    public abstract void OnDeath();

    public float Health {
        get { return health; }
    }
}
