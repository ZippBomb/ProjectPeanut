using UnityEngine;

public class Wall : Stareable {

    [SerializeField] private float aggresionTime = 3.0f;

    private float unstaredTime = 0.0f;

    private Material material;

    private void Start() {

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
            material = renderer.material;

    }

    private void Update() {

        if (!isStaredAt)
            unstaredTime += Time.deltaTime;

        if (unstaredTime >= aggresionTime)
            material.color = Color.Lerp(material.color, Color.red, Time.deltaTime);

    }

    public override void SetStareAt(bool value) {

        base.SetStareAt(value);
        if (value) {
            
            unstaredTime = 0.0f;
            material.color = Color.white;

        }

    }

}