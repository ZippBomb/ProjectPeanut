using UnityEngine;

public class Eyeball : Summon {

    [Header("References")]
    [SerializeField] private Transform eye;

    [SerializeField] private LayerMask wallMask;

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

    public override bool OnUpdatePreview(GameObject preview, RaycastHit hit) {
        
        if (hit.normal != Vector3.down) return false;
        if (hit.point.y < 5.0f) return false;

        return true;

    }

    void OnDestroy() {

        if (Wall.instance == null) return;
        
        Wall.instance.SetStareAt(false);
        Wall.instance.UpdateUnitsInRange();

    }

    private void OnDrawGizmos() {
        
        Gizmos.color = Color.red;
        Gizmos.DrawRay(eye.position, -eye.up);

    }

}
