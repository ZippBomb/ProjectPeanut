using System.Collections.Generic;
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

    private float hpPercantage = 1.0f;

    [SerializeField] private int stage = 0;

    [Header("Stage 1")]
    [SerializeField] private GameObject riftPrefab;
    [SerializeField] private float spawnTimer = 5.0f;
    [SerializeField, Range(0, 100)] private float spawnChance = 50;

    [Space]
    [SerializeField] private float riftDamage = 1.0f;
    [SerializeField] private List<Rift> rifts = new List<Rift>();

    [Space]
    [SerializeField] private float riftRange = 2.0f;
    [SerializeField] private LayerMask riftLayerMask;

    private List<Summon> unitsInRange = new List<Summon>();

    private float lastSpawnAttempt = 0.0f;

    [Header("Stage 3")]
    [SerializeField] private float gameOverDamage = 1.0f;
    [SerializeField] private float gameOverDamageMultiplier = 2.0f;

    private HealthStat playerHealth;

    private Material material;

    // General

    private void Start() {

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
            material = renderer.material;

        playerHealth = (HealthStat) Player.GetStat(Stat.Type.Health);
        Game.Assert(playerHealth != null, "Could not get and cast Health Stat.");

    }

    private void Update() {

        if (isStaredAt)
            lastSpawnAttempt = Time.time;

        UpdateHP();
        HandleStage();

        // Temporary, just to visualise the hp.
        material.color = Color.Lerp(Color.red, Color.white, hp / maxHP);

    }

    private void UpdateHP() {
        
        if (!isStaredAt)
            hp -= damageRate * Time.deltaTime;

        hp = Mathf.Clamp(hp, 0.0f, maxHP);
        hpPercantage = hp / maxHP;
        
    }

    // Stages

    private void HandleStage() {
        
        if (hpPercantage >= 0.95f)
            stage = 0;
        else if (hpPercantage > 0.0f) {

            // Introduces rifts. Rifts will spawn over the wall randomly and damage nearby 
            // units. They will slowly disappear when stared at, but will disappear faster when
            // stared directly at them.

            stage = 1; // Rifts

            HandleRifts();

        } else {

            stage = 2; // Game over

            if (Game.inMainMenu || !Player.instance.alive) return;

            playerHealth.Damage(gameOverDamage * Time.deltaTime);
            gameOverDamage += gameOverDamageMultiplier * Time.deltaTime;

        }

    }

    // Stage 1: Rifts
    //          Rifts will spawn on the wall randomly every few seconds, once spawned, they will
    //          slowly damage units in range (unitsInRange list), and also grow in size. Staring
    //          at them will decrease their size, or a rifter unit can be spawned to do that 
    //          automatically.
    private void HandleRifts() {

        // Damage all units in range.

        foreach (Summon summon in unitsInRange) {
            
            summon.Damage(riftDamage * rifts.Count * Time.deltaTime);

        }

        if (isStaredAt) return;

        // Rift spawning.
        // Every spawnTimer seconds, a rift will attempt to spawn, which will succeed spawnChance percent of time.
        // The rifts spawn at random places on the wall.

        if (Time.time < lastSpawnAttempt + spawnTimer) return;
        
        lastSpawnAttempt = Time.time;
        if (Random.Range(0, 100) > spawnChance) return;

        float x = Random.Range(-0.48f, 0.48f);
        float y = Random.Range(-0.48f, 0.48f);

        InstantiateParameters parameters = new InstantiateParameters();
        parameters.worldSpace = false;
        parameters.parent = transform;

        GameObject riftGO = Instantiate(riftPrefab, new Vector3(x, y, 0.5f), Quaternion.identity, parameters);
        Rift rift = riftGO.GetComponent<Rift>();
        Game.Assert(rift != null, "Instantiated riftPrefab does not have a Rift component.");

        rifts.Add(rift);

    }

    public void DamageRifts(float damage) {

        for (int i = rifts.Count - 1; i >= 0; i--) {

            rifts[i].Damage(damage);

        }

    }

    public void RemoveRift(Rift rift) {

        Game.Assert(rift != null, "Can't remove invalid rift (null).");
        Game.Assert(rifts.Contains(rift), "Can't remove invalid rift (not contained).");

        rifts.Remove(rift);

    }

    // Wall range

    public void UpdateUnitsInRange() {

        unitsInRange.Clear();

        Vector3 center = transform.position + transform.forward * (riftRange / 2.0f);
        Vector3 extents = new Vector3(transform.localScale.x / 2.0f, transform.localScale.y / 2.0f, riftRange / 2.0f);
        
        Collider[] colliders = Physics.OverlapBox(center, extents, transform.rotation, riftLayerMask);
        foreach (Collider collider in colliders) {
            
            Summon summon = collider.GetComponent<Summon>();
            if (summon == null) {
                
                Debug.LogError("An object with summon layer was in range of wall rifts, but did not have a Summon component. Name: " + collider.gameObject.name);
                continue;

            }

            unitsInRange.Add(summon);
            summon.inWallRange = true;

        }
        
    }

    // Debug

    void OnDrawGizmos() {

        Vector3 center = transform.position + transform.forward * (riftRange / 2.0f);
        Vector3 extents = new Vector3(transform.localScale.x / 2.0f, transform.localScale.y / 2.0f, riftRange / 2.0f);

        Gizmos.color = Color.green;
        Gizmos.matrix = Matrix4x4.TRS(center, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, extents * 2f);
        
    }

}
