using UnityEngine;

public class Eyeball : Summon {

    [Header("References")]
    [SerializeField] private Transform eye;

    [SerializeField] private LayerMask wallMask;

    private void OnDrawGizmos() {
        
        Gizmos.color = Color.red;
        Gizmos.DrawRay(eye.position, -eye.up);

    }

    private void Update() {

        UpdateSummon();

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

    void OnDestroy() {

        if (Wall.instance == null) return;
        
        Wall.instance.SetStareAt(false);
        Wall.instance.UpdateUnitsInRange();

    }

}
