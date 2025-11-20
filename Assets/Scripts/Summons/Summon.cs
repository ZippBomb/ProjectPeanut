using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public abstract class Summon : Stareable {

    [Header("Health")]
    [SerializeField] private float health = 100.0f;
    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] private float damageRate = 1.0f;

    [SerializeField, Space] private Vector3 hpIndicatorOffset = new Vector3(0.0f, 0.5f, 0.0f);

    [Header("Wall")]
    public bool inWallRange = false;

    [Header("Preview")]
    [SerializeField] private Mesh previewMesh;

    [Space]
    [SerializeField] private Vector3 previewOffset;
    [SerializeField] private Vector3 previewSize = Vector3.one;
    [SerializeField] private Vector3 previewRotation;

    private GameObject hpIndicator;

    private void Update() {

        if (GameManager.instance.pauseMenuOpen) return;

        health -= damageRate * Time.deltaTime;
        if (health <= 0.0f)
            Destroy(gameObject);

        UpdateSummon();

    }
    public virtual void OnDestroy() {
        
        if (Wall.instance != null)
            Wall.instance.UpdateUnitsInRange();

        Destroy(hpIndicator);

    }

    public virtual void UpdateSummon() {

        if (!hpIndicator.activeSelf) return;

        hpIndicator.transform.LookAt(Player.GetCamera().transform);
        hpIndicator.transform.GetChild(1).GetComponent<Image>().fillAmount = health / maxHealth;

    }

    public void Damage(float value) {
        
        health -= value;
        if (health <= 0.0f)
            Destroy(gameObject);

    }

    public override void SetStareAt(bool value) {

        base.SetStareAt(value);

        hpIndicator.SetActive(value);

    }

    public virtual void OnSummoned(GameObject indicator) {
        
        hpIndicator = indicator;
        hpIndicator.transform.position = transform.position + hpIndicatorOffset;

    }

    public virtual bool OnUpdatePreview(GameObject preview, RaycastHit hit) { return true; }

    public Mesh GetPreviewMesh() { return previewMesh; }

    public Vector3 GetPreviewOffset() { return previewOffset; }
    public Vector3 GetPreviewSize() { return previewSize; }
    public Vector3 GetPreviewRotation() { return previewRotation; }
    
}
