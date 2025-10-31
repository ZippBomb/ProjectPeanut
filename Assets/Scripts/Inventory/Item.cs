using UnityEngine;

public enum Hand : byte {

    None = 0,

    Left,
    Right,

}

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item", order = 0)]
public class Item : ScriptableObject {
    
    [Header("General")]
    public new string name = "Item";
    public int id = -1;

    [Header("Visual")]
    public Mesh mesh;
    public Material material;
    public Vector3 rotation;

    public virtual void Use(Hand hand) {

        Debug.Log("Item " + name + " was used.");

    }

}