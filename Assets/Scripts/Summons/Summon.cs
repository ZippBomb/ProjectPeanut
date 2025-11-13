using UnityEngine;

public abstract class Summon : MonoBehaviour {

    [Header("Health")]
    [SerializeField] private float health = 100.0f;
    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] private float damageRate = 1.0f;

    [Header("Wall")]
    public bool inWallRange = false;

    [Header("Preview")]
    [SerializeField] private Mesh previewMesh;

    [Space]
    [SerializeField] private Vector3 previewOffset;
    [SerializeField] private Vector3 previewSize = Vector3.one;
    [SerializeField] private Vector3 previewRotation;

    private void Update() {

        UpdateSummon();

    }
    private void OnDestroy() {
        
        if (Wall.instance != null)
            Wall.instance.UpdateUnitsInRange();

    }

    protected void UpdateSummon() {
        
        health -= damageRate * Time.deltaTime;
        if (health <= 0.0f)
            Destroy(gameObject);

    }

    public virtual void OnSummoned() {}

    public virtual bool OnUpdatePreview(GameObject preview, RaycastHit hit) { return true; }

    public void Damage(float value) {
        
        health -= value;
        if (health <= 0.0f)
            Destroy(gameObject);

    }

    public Mesh GetPreviewMesh() { return previewMesh; }

    public Vector3 GetPreviewOffset() { return previewOffset; }
    public Vector3 GetPreviewSize() { return previewSize; }
    public Vector3 GetPreviewRotation() { return previewRotation; }
    
}
