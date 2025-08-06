using UnityEngine;

public class Wall : MonoBehaviour {

    [SerializeField] private bool staredAt = false;

    public void SetStare(bool value) {

        staredAt = value;

    }

}