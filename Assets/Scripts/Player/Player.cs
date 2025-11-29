using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public static Player instance;
    public MainInput input { get; private set; }

    private void Awake() {

        instance = this;
        input = Game.input;

    }

    public bool alive = true;

    [Header("Body parts")]
    [SerializeField] private GameObject cam;

    [Header("Stareables")]
    [SerializeField] private LayerMask stareableMask;
    [SerializeField] private Stareable currentStareable;
    [SerializeField] private float stareableMaxDist = 50.0f;

    [Header("Stats")]
    [SerializeField] private Stat[] stats = new Stat[] { new HealthStat(), new SatietyStat(), new ThirstStat() };
    [SerializeField] private Transform statIndicatorParent;
    [SerializeField] private GameObject statIndicatorPrefab;

    [Header("Inventory")]
    [SerializeField] private Item leftItem;
    [SerializeField] private Item rightItem;

    [SerializeField] private GameObject thrownItemPrefab;
    [SerializeField] private float thrownItemForce = 10.0f;

    [Space]
    [SerializeField] private GameObject leftItemPreview;
    [SerializeField] private GameObject rightItemPreview;

    [Header("Spell system")]
    [SerializeField] private List<Spell> spells = new List<Spell>();

    [SerializeField] private GameObject cooldownPrefab;
    [SerializeField] private Transform cooldownParent;

    private void Start() {

        if (Game.inMainMenu) return;

        Game.Assert(leftItemPreview != null, "No Left item preview assigned to Player.");
        Game.Assert(rightItemPreview != null, "No Right item preview assigned to Player.");

        Game.Assert(leftItemPreview.GetComponent<MeshRenderer>() != null && leftItemPreview.GetComponent<MeshFilter>() != null,
            "Left item previe assigned to player is missing Mesh Renderer and/or Mesh Filter components!");
        Game.Assert(rightItemPreview.GetComponent<MeshRenderer>() != null && rightItemPreview.GetComponent<MeshFilter>() != null,
            "Right item previe assigned to player is missing Mesh Renderer and/or Mesh Filter components!");

        Game.Assert(thrownItemPrefab != null, "No thrown item prefab assigned.");
        Game.Assert(thrownItemPrefab.GetComponent<MeshFilter>() != null, "Thrown item prefab is missing MeshFilter component.");
        Game.Assert(thrownItemPrefab.GetComponent<Rigidbody>() != null, "Thrown item prefab is missing RigidBody component.");
        Game.Assert(thrownItemPrefab.GetComponent<ThrownItem>() != null, "Thrown item prefab is missing ThrownItem component");

        Game.Assert(cooldownPrefab != null, "Cooldown prefab not assinged.");
        Game.Assert(cooldownPrefab.GetComponent<Image>() != null, "Cooldown prefab is missing Image component.");
        
        Image tmp = cooldownPrefab.transform.GetChild(0).GetComponent<Image>();
        Game.Assert(tmp != null, "Cooldown prefab child not assinged.");
        Game.Assert(tmp.type == Image.Type.Filled, "Cooldown image child is not set to Filled type.");

        if (leftItem != null)
            SetLeftItem(leftItem);
        if (rightItem != null)
            SetRightItem(rightItem);

        foreach (Stat stat in stats) {

            GameObject indicator = Instantiate(statIndicatorPrefab, statIndicatorParent);
            stat.HookUI(indicator);

        }

        foreach (Spell spell in spells) {
            
            GameObject cooldownIcon = Instantiate(cooldownPrefab, cooldownParent);
            spell.HookUI(cooldownIcon.GetComponent<Image>());

        }
        
    }
    private void Update() {

        if (!alive) return;

        foreach (Stat stat in stats)
            stat.Update();

        foreach (Spell spell in spells)
            spell.Update();

        HandleStareableRay();
        
    }

    private void OnEnable() {

        if (Game.inMainMenu) return;

        input.Player.Enable();
        input.Items.Enable();

        input.Player.Interact.performed += Interact;
        input.Player.Castspell.performed += CastSpell;

        input.Player.Pause.performed += TogglePauseMenu;

        input.Items.UseLeftItem.performed += UseLeftItem;
        input.Items.UseRightItem.performed += UseRightItem;

        input.Items.DropLeftItem.performed += DropLeftItem;
        input.Items.DropRightItem.performed += DropRightItem;

    }
    private void OnDisable() {

        if (Game.inMainMenu) return;

        input.Player.Disable();
        input.Items.Disable();

        input.Player.Interact.performed -= Interact;
        input.Player.Castspell.performed -= CastSpell;

        input.Player.Pause.performed -= TogglePauseMenu;
        
        input.Items.UseLeftItem.performed -= UseLeftItem;
        input.Items.UseRightItem.performed -= UseRightItem;

        input.Items.DropLeftItem.performed -= DropLeftItem;
        input.Items.DropRightItem.performed -= DropRightItem;

    }

    public void Die() {
        
        alive = false;

        Game.input.Disable();

        GameManager.instance.GameOver();
        GameManager.instance.crosshair.SetActive(false);

    }

    private void TogglePauseMenu(InputAction.CallbackContext ctx) {

        if (!alive) return;

        if (SummonManager.instance.InProgress()) {
            
            SummonManager.instance.CancelSummon();
            return;

        }
        
        if (GameManager.instance.pauseMenuOpen)
            GameManager.instance.ClosePauseMenu();
        else
            GameManager.instance.OpenPauseMenu();

    }

    // Stareables

    private void HandleStareableRay() {
        
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 100.0f, stareableMask)) {

            Stareable stareable = hit.collider.GetComponent<Stareable>();
            if (stareable != null) {

                if (currentStareable != null)
                    currentStareable.SetStareAt(false);

                stareable.SetStareAt(true);
                currentStareable = stareable;

                return;

            }

        }

        if (currentStareable == null) return;

        currentStareable.SetStareAt(false);
        currentStareable = null;

    }

    private void Interact(InputAction.CallbackContext ctx) {
        
        if (currentStareable == null) return;
        currentStareable.Interact();

    }

    // Item system
    
    private void UseLeftItem(InputAction.CallbackContext ctx) {

        if (leftItem == null) return;
        leftItem.Use(Hand.Left);

    }
    private void UseRightItem(InputAction.CallbackContext ctx) {

        if (rightItem == null) return;
        rightItem.Use(Hand.Right);

    }

    private void DropLeftItem(InputAction.CallbackContext ctx) {

        if (leftItem == null) return;

        Vector3 position = cam.transform.position + cam.transform.forward * 0.5f;
        GameObject thrownItem = Instantiate(thrownItemPrefab, position, Quaternion.identity);

        thrownItem.GetComponent<MeshFilter>().mesh = leftItem.mesh;
        thrownItem.GetComponent<MeshRenderer>().material = leftItem.material;
        thrownItem.GetComponent<Rigidbody>().AddForce(cam.transform.forward * thrownItemForce);
        
        thrownItem.GetComponent<ThrownItem>().item = leftItem;

        RemoveLeftItem();

    }
    private void DropRightItem(InputAction.CallbackContext ctx) {

        if (rightItem == null) return;

        Vector3 position = cam.transform.position + cam.transform.forward * 0.5f;
        GameObject thrownItem = Instantiate(thrownItemPrefab, position, Quaternion.identity);

        thrownItem.GetComponent<MeshFilter>().mesh = rightItem.mesh;
        thrownItem.GetComponent<MeshRenderer>().material = rightItem.material;
        thrownItem.GetComponent<Rigidbody>().AddForce(cam.transform.forward * thrownItemForce);
        
        thrownItem.GetComponent<ThrownItem>().item = rightItem;

        RemoveRightItem();

    }
    
    public Item GetLeftItem() { return leftItem; }
    public Item GetRightItem() { return rightItem; }
    public Item GetItem(Hand hand) {

        switch (hand) {

            case Hand.Left: return leftItem;
            case Hand.Right: return rightItem;
            default: Debug.LogError("Invalid hand passed to GetItem."); return null;

        }

    }
    
    public void SetLeftItem(Item item) {

        leftItem = item;

        leftItemPreview.SetActive(true);

        leftItemPreview.GetComponent<MeshFilter>().mesh = item.mesh;
        leftItemPreview.GetComponent<MeshRenderer>().material = item.material;

        leftItemPreview.transform.localEulerAngles = item.rotation;

    }
    public void SetRightItem(Item item) {

        rightItem = item;

        rightItemPreview.SetActive(true);

        rightItemPreview.GetComponent<MeshFilter>().mesh = item.mesh;
        rightItemPreview.GetComponent<MeshRenderer>().material = item.material;

        rightItemPreview.transform.localEulerAngles = item.rotation;

    }
    public void SetItem(Hand hand, Item item) {

        switch (hand) {

            case Hand.Left: SetLeftItem(item); break;
            case Hand.Right: SetRightItem(item); break;
            default: Debug.LogError("Invalid hand passed to SetItem."); break;

        }

    }

    public void RemoveLeftItem() {

        leftItem = null;
        leftItemPreview.SetActive(false);

    }
    public void RemoveRightItem() {

        rightItem = null;
        rightItemPreview.SetActive(false);

    }
    public void RemoveItem(Hand hand) {

        switch (hand) {

            case Hand.Left: RemoveLeftItem(); break;
            case Hand.Right: RemoveRightItem(); break;
            default: Debug.LogError("Invalid hand passed to RemoveItem."); break;

        }

    }

    public bool HasItem(Item item) {
        
        if (leftItem == item || rightItem == item) return true;
        return false;

    }
    public bool HasItemInLeft(Item item) {
        
        if (leftItem == item) return true;
        return false;

    }
    public bool HasItemInRight(Item item) {
        
        if (rightItem == item) return true;
        return false;

    }

    // Spell system

    private void CastSpell(InputAction.CallbackContext ctx) {

        if (!alive) return;

        int value = (int) ctx.ReadValue<float>();
        if (value >= spells.Count || spells[value] == null) return;

        Spell spell = spells[value];
        spell.AttemptCast();

    }

    // Getters

    public static Stat GetStat(Stat.Type type) {

        int index = (int) type;

        Game.Assert(index < instance.stats.Length, "Invalid Stat type.");
        return instance.stats[index];

    }

    public static GameObject GetCamera() { return instance.cam; }

}
