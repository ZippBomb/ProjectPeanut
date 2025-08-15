using UnityEngine;

public class Wall : MonoBehaviour {

    [SerializeField] private float aggresionTime = 3.0f;

    [SerializeField] private bool staredAt = false;

    private float unstaredTime = 0.0f;

    private Material material;

    private void Start() {

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
            material = renderer.material;

    }

    private void Update() {

        if (!staredAt)
            unstaredTime += Time.deltaTime;

        if (unstaredTime >= aggresionTime)
            material.color = Color.Lerp(material.color, Color.red, Time.deltaTime);

    }

    public void SetStare(bool value) {

        staredAt = value;
        if (staredAt) {

            unstaredTime = 0.0f;
            material.color = Color.white;

        }

    }

}