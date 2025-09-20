using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class Player : MonoBehaviour {

    public static Player instance;
    public MainInput.PlayerActions input { get; private set; }

    private void Awake() {

        instance = this;
        input = Game.input.Player;

    }

    [Header("Stats")]
    [SerializeField] private Stat[] stats = new Stat[] { new HealthStat(), new Stat(), new Stat(), };
    [SerializeField] private Transform statIndicatorParent;
    [SerializeField] private GameObject statIndicatorPrefab;

    [Header("Inventory")]
    [SerializeField] private Item leftItem;
    [SerializeField] private Item rightItem;

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

        foreach (Stat stat in stats)
            stat.Update();
        
    }

    private void OnEnable() {

        input.Enable();

    }
    private void OnDisable() {

        input.Disable();

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
    
    public void SetLeftItem(Item item) {

        leftItem = item;

        leftItemPreview.SetActive(true);
        leftItemPreview.GetComponent<MeshFilter>().mesh = item.mesh;
        leftItemPreview.GetComponent<MeshRenderer>().material = item.material;

    }
    public void SetRightItem(Item item) {

        rightItem = item;

        rightItemPreview.SetActive(true);
        rightItemPreview.GetComponent<MeshFilter>().mesh = item.mesh;
        rightItemPreview.GetComponent<MeshRenderer>().material = item.material;

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

    // Spell system

    private void CastSpell(InputAction.CallbackContext ctx) {

        int value = (int) ctx.ReadValue<float>();
        if (value >= spells.Count || spells[value] == null) return;

        Spell spell = spells[value];
        spell.Cast();

    }

    // Getters

    public static HealthStat GetHealthStat() { return (HealthStat) Player.instance.stats[0]; }
    public static Stat GetThirstStat() { return Player.instance.stats[1]; }
    public static Stat GetHungerStat() { return Player.instance.stats[2]; }

}