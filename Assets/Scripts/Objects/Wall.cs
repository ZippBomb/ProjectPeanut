using UnityEngine;

public class Wall : Stareable {

    public static Wall instance;

    private void Awake() {
        
        instance = this;

    }

    [Header("Health")]
    [SerializeField] private float hp = 200.0f;
    [SerializeField] private float maxHP = 200.0f;
    public float damageRate = 1.0f;
    public float healRate = 0.7f;

    private float hpPercantage = 1.0f;

    [SerializeField] private int stage = 0;

    [Header("Stage 1")]
    [SerializeField] private float baseDamage = 1.0f;

    [Header("Stage 3")]
    [SerializeField] private float gameOverDamage = 1.0f;
    [SerializeField] private float gameOverDamageMultiplier = 2.0f;

    private HealthStat playerHealth;

    private Material material;

    private void Start() {

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
            material = renderer.material;

        playerHealth = (HealthStat) Player.GetStat(Stat.Type.Health);
        Game.Assert(playerHealth != null, "Could not get and cast Health Stat.");

    }

    private void Update() {

        if (!Player.instance.alive) return;

        UpdateHP();
        HandleStage();

        // Temporary, just to visualise the hp.
        material.color = Color.Lerp(Color.red, Color.white, hp / maxHP);

    }

    private void UpdateHP() {
        
        if (!isStaredAt)
            hp -= damageRate * Time.deltaTime;
        else
            hp += healRate * Time.deltaTime;

        hp = Mathf.Clamp(hp, 0.0f, maxHP);
        hpPercantage = hp / maxHP;
        
    }

    private void HandleStage() {
        
        if (hpPercantage >= 0.95f)
            stage = 0;
        else if (hpPercantage >= 0.7f) {

            // Damage the player slowly over time;

            stage = 1;

            ConstantPlayerDamage();

        } else if (hpPercantage > 0.0f) {

            // Introduces rifts. Rifts will spawn over the wall randomly and damage nearby 
            // units. They will slowly disappear when stared at, but will disappear faster when
            // stared directly at them.

            stage = 2; // Damage + Rifts

            ConstantPlayerDamage();

        } else {

            stage = 3; // Game over

            playerHealth.Damage(gameOverDamage * Time.deltaTime);
            gameOverDamage += gameOverDamageMultiplier * Time.deltaTime;

        }

    }

    private void ConstantPlayerDamage() {
        
        playerHealth.Damage(baseDamage * Time.deltaTime);

    }
    
}