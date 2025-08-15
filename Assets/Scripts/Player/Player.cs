using UnityEngine;

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

    private void Start() {

        foreach (Stat stat in stats) {

            GameObject indicator = Instantiate(statIndicatorPrefab, statIndicatorParent);
            stat.HookUI(indicator);

        }
        
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

}