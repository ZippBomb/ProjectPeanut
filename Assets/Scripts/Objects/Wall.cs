using UnityEngine;

public class Wall : Stareable {

    public static Wall instance;

    private void Awake() {
        
        instance = this;

    }

    [SerializeField] private float hp = 200.0f;
    [SerializeField] private float maxHP = 200.0f;
    [SerializeField] private float damageRate = 1.0f;
    [SerializeField] private float healRate = 0.7f;

    private Material material;

    private void Start() {

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
            material = renderer.material;

    }

    private void Update() {

        if (!isStaredAt)
            hp -= damageRate * Time.deltaTime;
        else
            hp += healRate * Time.deltaTime;

        if (hp > maxHP)
            hp = maxHP;

        material.color = Color.Lerp(Color.red, Color.white, hp / maxHP);

    }

}