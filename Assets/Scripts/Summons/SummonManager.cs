using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SummonManager : MonoBehaviour {

    public static SummonManager instance;

    private void Awake() {
        
        instance = this;

    }

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerCamera;

    [Header("Controls")]
    [SerializeField] private float rotateSensitivity = 1.0f;

    [Header("Preview")]
    [SerializeField] private GameObject previewPrefab;
    [SerializeField, ReadOnly] private GameObject summon;

    private GameObject preview;
    private Transform previewMesh;

    private void Start() {
        
        Game.input.Casting.Cast.performed += CastSummon;

        preview = Instantiate(previewPrefab, transform);
        preview.SetActive(false);

        previewMesh = preview.transform.GetChild(0);

    }
    private void Update() {
        
        if (!Player.instance.alive) return;
        if (summon == null) return;

        RaycastHit hit;
        if (!Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, 1000.0f)) {
            
            preview.SetActive(false);
            return;

        }

        preview.transform.position = hit.point;

        float rotDelta = Game.input.Casting.Rotate.ReadValue<float>() * rotateSensitivity;
        preview.transform.Rotate(Vector3.up, rotDelta);

    }

    public void StartSummon(GameObject summonPrefab) {

        if (summon != null) return;
        
        Game.Assert(summonPrefab.GetComponent<Summon>() != null, "Can't summon a prefab that has no Summon component. Prefab: " + summonPrefab.name);

        Summon summonComponent = summonPrefab.GetComponent<Summon>();
        Game.Assert(summonComponent.GetPreviewMesh() != null, "Can't summon a prefab with no preview mesh.");

        previewMesh.GetComponent<MeshFilter>().mesh = summonComponent.GetPreviewMesh();

        previewMesh.localPosition = summonComponent.GetPreviewOffset();
        previewMesh.localScale = summonComponent.GetPreviewSize();
        previewMesh.localRotation = Quaternion.Euler(summonComponent.GetPreviewRotation());

        preview.SetActive(true);

        summon = summonPrefab;

        Game.input.Casting.Enable();

        Game.DisableInteractionInput();
        Game.DisableInventoryInput();

    }

    private void CastSummon(InputAction.CallbackContext ctx) {
        
        if (summon == null) return;

        Instantiate(summon, preview.transform.position, preview.transform.rotation);
        preview.SetActive(false);

        summon.GetComponent<Summon>().OnSummoned();
        Wall.instance.UpdateUnitsInRange();

        summon = null;

        Game.input.Casting.Disable();

        Game.EnableInventoryInput();
        Game.EnableInteractionInput();

    }

}
