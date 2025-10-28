using UnityEngine;

public class Rift : Stareable {

    [SerializeField] private float health = 1;
    [SerializeField] private float maxHealth = 50;

    [SerializeField] private float damageRate = 0.8f;
    [SerializeField] private float healRate = 1.0f;

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
            Destroy(gameObject);

        // Grow/shrink based on hp

        transform.localScale = startScale * (health / maxHealth);

    }

}