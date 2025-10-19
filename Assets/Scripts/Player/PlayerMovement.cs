using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] private float speed = 1.0f;

    private MainInput.PlayerActions inputMap;

    private Rigidbody rb;
    private Vector3 direction;

    private void Start() {

        inputMap = Player.instance.input;

        rb = GetComponent<Rigidbody>();
        Game.Assert(rb != null, "No RigidBody on object '" + gameObject.name + "' required by PlayerMovement");

    }
    private void Update() {

        if (!Player.instance.alive) return;

        Vector2 input = inputMap.Move.ReadValue<Vector2>();
        direction = transform.right * input.x + transform.forward * input.y;

    }
    private void FixedUpdate() {

        if (!Player.instance.alive) return;

        Vector3 force = direction.normalized * speed * 1000.0f;
        rb.AddForce(force);

    }

}