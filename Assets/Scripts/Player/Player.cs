using UnityEngine;

public class Player : MonoBehaviour {

    public static Player instance;
    private void Awake() {

        instance = this;
        input = new PlayerInput();

    }

    public PlayerInput input { get; private set; }

    private void OnEnable() {

        input.Enable();

    }
    private void OnDisable() {

        input.Disable();

    }

}