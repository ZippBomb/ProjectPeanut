using UnityEngine;

public class Rift : Stareable {

    [SerializeField] private float health = 1;
    [SerializeField] private float maxHealth = 50;

    [SerializeField] private float damageRate = 0.8f;
    [SerializeField] private float healRate = 1.0f;

    [SerializeField] private Vector3 minScale;
    private Vector3 startScale;

    private void Start() {
        
        health = 1;
        startScale = transform.localScale;

    }
    private void Update() {

        // Update health
        
        if (isStaredAt)
            health -= damageRate * Time.deltaTime;
        else
            health += healRate * Time.deltaTime;

        health = Mathf.Clamp(health, 0, maxHealth);
        if (health == 0.0f)
            Die();

        // Grow/shrink based on hp

        transform.localScale = minScale + startScale * (health / maxHealth);

    }

    public override void SetStareAt(bool value) {

        base.SetStareAt(value);
        Wall.instance.SetStareAt(value);

    }

    private void Die() {

        Wall.instance.RemoveRift(this);
        Destroy(gameObject);

    }

    public void Damage(float damage) {

        health -= damage;
        health  = Mathf.Clamp(health, 0, maxHealth);

        if (health == 0.0f)
            Die();

    }

}
