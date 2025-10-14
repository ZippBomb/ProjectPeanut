using UnityEngine;

public class Eyeball : Summon {

    [Header("References")]
    [SerializeField] private Transform eye;

    [Header("Health")]
    [SerializeField] private float health = 100.0f;
    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] private float damageRate = 1.0f;

    [SerializeField] private LayerMask wallMask;

    private void OnDrawGizmos() {
        
        Gizmos.color = Color.red;
        Gizmos.DrawRay(eye.position, -eye.up);

    }

    private void Update() {

        health -= damageRate * Time.deltaTime;
        if (health <= 0.0f) {
            
            Wall.instance.SetStareAt(false);
            Destroy(gameObject);

            return;

        }

        RaycastHit hit;
        if (!Physics.Raycast(eye.position, -eye.up, out hit, 1000.0f, wallMask)) {
            
            Wall.instance.SetStareAt(false);
            return;

        }
        if (hit.collider.GetComponent<Wall>() == null) {
            
            Wall.instance.SetStareAt(false);
            return;

        }

        Wall.instance.SetStareAt(true);
        
    }

}