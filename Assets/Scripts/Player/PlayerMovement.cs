using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public static PlayerMovement instance;

    private void Awake() {

        instance = this;
        
    }

    public float speed = 1.0f;

    private MainInput.PlayerActions inputMap;

    private Rigidbody rb;
    private Vector3 direction;

    private void Start() {

        inputMap = Game.input.Player;

        rb = GetComponent<Rigidbody>();
        Game.Assert(rb != null, "No RigidBody on object '" + gameObject.name + "' required by PlayerMovement");

    }
    private void Update() {

        Vector2 input = inputMap.Move.ReadValue<Vector2>();
        direction = transform.right * input.x + transform.forward * input.y;

    }
    private void FixedUpdate() {

        Vector3 force = direction.normalized * speed * 1000.0f;
        rb.AddForce(force);

    }

}
