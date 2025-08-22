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
    [SerializeField] private Stat[] stats = new Stat[] { new HealthStat(), };
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
    
    private void UseLeftItem(InputAction.CallbackContext ctx) {

        leftItem.Use();

    }
    private void UseRightItem(InputAction.CallbackContext ctx) {

        rightItem.Use();

    }

    private void OnEnable() {

        input.Enable();

    }
    private void OnDisable() {

        input.Disable();

    }

}