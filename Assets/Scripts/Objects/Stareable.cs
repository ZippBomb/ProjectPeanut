using UnityEngine;

public class Stareable : MonoBehaviour {
    
    [SerializeField] protected bool isStaredAt = false; 

    public virtual void SetStareAt(bool value) {
        
        isStaredAt = value;

    }

}