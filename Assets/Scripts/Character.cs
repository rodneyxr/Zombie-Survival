using System.Collections;

public abstract class Character : ICharacter {

    protected int health;

    public void Damage(int damage) {
        health -= damage;
        if (health <= 0) {
            health = 0;
            OnDeath();
        }
    }

    public abstract void OnDeath();

}
