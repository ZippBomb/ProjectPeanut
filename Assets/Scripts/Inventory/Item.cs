using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Items/Item", order = 0)]
public class Item : ScriptableObject {
    
    public new string name = "Item";
    public int id = -1;

    public virtual void Use() {

        Debug.Log("Item " + name + " was used.");

    }

}