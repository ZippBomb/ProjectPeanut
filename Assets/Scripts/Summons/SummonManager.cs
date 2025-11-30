using System.Collections.Generic;

using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
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

    [Header("Unit HP Indicator")]
    [SerializeField] private GameObject hpIndicatorPrefab;
    [SerializeField] private Transform hpIndicatorParent;

    private GameObject preview;
    private Transform previewMesh;

    private Dictionary<int, int> unitCount = new Dictionary<int, int>();

    private void Start() {
        
        Game.input.Casting.Cast.performed += CastSummon;

        preview = Instantiate(previewPrefab, transform);
        preview.SetActive(false);

        previewMesh = preview.transform.GetChild(0);

        Game.Assert(hpIndicatorPrefab.transform.GetChild(1).GetComponent<Image>() != null, "Unit HP Indicator prefab's first child (Background) is missing an Image component.");

        Game.Assert(hpIndicatorPrefab.transform.GetChild(0).GetComponent<Image>() != null, "Unit HP Indicator prefab's second child (Fill) is missing an Image component.");
        Game.Assert(hpIndicatorPrefab.transform.GetChild(0).GetComponent<Image>().type != Image.Type.Filled, "Unit HP Indicator prefab's second child's (Fill) Image component is not set to Filled type.");

    }
    private void Update() {
        
        if (!Player.instance.alive) return;
        if (summon == null) return;

        RaycastHit hit;
        if (!Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, 1000.0f)) {
            
            preview.SetActive(false);
            return;

        }
        if (!summon.GetComponent<Summon>().OnUpdatePreview(preview, hit)) {
            
            preview.SetActive(false);
            return;

        } else if (!preview.activeSelf)
            preview.SetActive(true);

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
        if (!preview.activeSelf) return;

        GameObject summonGO = Instantiate(summon, preview.transform.position, preview.transform.rotation);
        preview.SetActive(false);

        GameObject hpIndicator = Instantiate(hpIndicatorPrefab, hpIndicatorParent);

        summonGO.GetComponent<Summon>().OnSummoned(hpIndicator);
        Wall.instance.UpdateUnitsInRange();

        int summonID = summon.GetComponent<Summon>().GetID();
        if (unitCount.ContainsKey(summonID))
            unitCount[summonID]++;
        else
            unitCount.Add(summonID, 1);

        summon = null;

        Game.input.Casting.Disable();

        Game.EnableInventoryInput();
        Game.EnableInteractionInput();

    }

    public void CancelSummon() {
        
        preview.SetActive(false);
        summon = null;

        Game.input.Casting.Disable();

        Game.EnableInventoryInput();
        Game.EnableInteractionInput();

    }

    public int GetUnitCount(int summonID) {
        
        if (!unitCount.ContainsKey(summonID)) return -1;
        return unitCount[summonID];

    }
    public void DecrementUnitCount(int summonID) {
        
        Game.Assert(unitCount.ContainsKey(summonID), "Tried decrementing a summon ID that is not present in unitCount.");
        unitCount[summonID]--;

    }

    public bool InProgress() { return summon != null; }

}
