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

    private void Start() {

        foreach (Stat stat in stats) {

            GameObject indicator = Instantiate(statIndicatorPrefab, statIndicatorParent);
            stat.HookUI(indicator);

        }

        input.UseLeftItem.performed += UseLeftItem;
        input.UseRightItem.performed += UseRightItem;
        
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

    public void SetItem(Hand hand, Item item) {

        switch (hand) {

            case Hand.Left: leftItem = item; break;
            case Hand.Right: rightItem = item; break;
            default: Debug.LogError("Invalid hand passed to SetItem."); break;

        }

    }
    public void RemoveItem(Hand hand) {

        switch (hand) {

            case Hand.Left: leftItem = null; break;
            case Hand.Right: rightItem = null; break;
            default: Debug.LogError("Invalid hand passed to RemoveItem."); break;

        }

    }

    // Getters

    public static HealthStat GetHealthStat() { return (HealthStat) Player.instance.stats[0]; }
    public static Stat GetThirstStat() { return Player.instance.stats[1]; }
    public static Stat GetHungerStat() { return Player.instance.stats[2]; }

}