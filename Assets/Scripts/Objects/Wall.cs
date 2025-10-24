using UnityEngine;

public class Wall : Stareable {

    public static Wall instance;

    private void Awake() {
        
        instance = this;

    }

    [SerializeField] private float hp = 200.0f;
    [SerializeField] private float maxHP = 200.0f;
    public float damageRate = 1.0f;
    [SerializeField] private float healRate = 0.7f;

    private float hpPercantage = 1.0f;

    private Material material;

    private void Start() {

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
            material = renderer.material;

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

        if (hp > maxHP)
            hp = maxHP;
        else if (hp <= 0.0f)
            GameManager.instance.GameOver();

        hpPercantage = hp / maxHP;
        
    }
    private void HandleStage() {
        
        //

    }

}