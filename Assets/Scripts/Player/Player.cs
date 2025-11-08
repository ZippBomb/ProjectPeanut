using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {

    public static Player instance;
    public MainInput.PlayerActions input { get; private set; }

    private void Awake() {

        instance = this;
        input = Game.input.Player;

    }

    public bool alive = true;

    [Header("Body parts")]
    [SerializeField] private GameObject cam;

    [Header("Stareables")]
    [SerializeField] private LayerMask stareableMask;
    [SerializeField] private Stareable currentStareable;

    [Header("Stats")]
    [SerializeField] private Stat[] stats = new Stat[] { new HealthStat(), new SatietyStat(), new ThirstStat(), new ExhaustionStat(), new ToiletStat() };
    [SerializeField] private Transform statIndicatorParent;
    [SerializeField] private GameObject statIndicatorPrefab;

    [Header("Inventory")]
    [SerializeField] private Item leftItem;
    [SerializeField] private Item rightItem;

    [Space]
    [SerializeField] private GameObject leftItemPreview;
    [SerializeField] private GameObject rightItemPreview;

    [Header("Spell system")]
    [SerializeField] private List<Spell> spells = new List<Spell>();

    private void Start() {

        Game.Assert(leftItemPreview != null, "No Left item preview assigned to Player.");
        Game.Assert(rightItemPreview != null, "No Right item preview assigned to Player.");

        Game.Assert(leftItemPreview.GetComponent<MeshRenderer>() != null && leftItemPreview.GetComponent<MeshFilter>() != null,
            "Left item previe assigned to player is missing Mesh Renderer and/or Mesh Filter components!");
        Game.Assert(rightItemPreview.GetComponent<MeshRenderer>() != null && rightItemPreview.GetComponent<MeshFilter>() != null,
            "Right item previe assigned to player is missing Mesh Renderer and/or Mesh Filter components!");

        if (leftItem != null)
            SetLeftItem(leftItem);
        if (rightItem != null)
            SetRightItem(rightItem);

        foreach (Stat stat in stats) {

            GameObject indicator = Instantiate(statIndicatorPrefab, statIndicatorParent);
            stat.HookUI(indicator);

        }

        input.UseLeftItem.performed += UseLeftItem;
        input.UseRightItem.performed += UseRightItem;

        input.Castspell.performed += CastSpell;
        
    }
    private void Update() {

        if (!alive) return;

        foreach (Stat stat in stats)
            stat.Update();

        HandleStareableRay();
        
    }

    private void OnEnable() {

        input.Enable();

    }
    private void OnDisable() {

        input.Disable();

    }

    public void Die() {
        
        alive = false;

        Game.input.Disable();

        GameManager.instance.GameOver();

    }

    // Stareables

    private void HandleStareableRay() {
        
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 1000.0f, stareableMask)) {
            
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

    // Item system
    
    private void UseLeftItem(InputAction.CallbackContext ctx) {

        if (!alive) return;
        if (leftItem == null) return;

        leftItem.Use(Hand.Left);

    }
    private void UseRightItem(InputAction.CallbackContext ctx) {

        if (!alive) return;
        if (rightItem == null) return;

        rightItem.Use(Hand.Right);

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
        Debug.Log("Cast spell triggered: " + value);
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
