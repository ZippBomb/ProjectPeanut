using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {

    public static PlayerMovement instance;

    private void Awake() {

        instance = this;
        
    }

    public float speed = 1.0f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 10.0f;
    [SerializeField] private float airSpeed = 1.0f;

    [Space]
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;

    private MainInput.PlayerActions inputMap;

    private Rigidbody rb;
    private Vector3 direction;
    private float speedMultiplier = 1.0f;

    private void Start() {

        inputMap = Game.input.Player;

        rb = GetComponent<Rigidbody>();
        Game.Assert(rb != null, "No RigidBody on object '" + gameObject.name + "' required by PlayerMovement");

        inputMap.Jump.performed += Jump;

    }
    private void Update() {

        GroundCheck();

        Vector2 input = inputMap.Move.ReadValue<Vector2>();
        direction = transform.right * input.x + transform.forward * input.y;

    }
    private void FixedUpdate() {

        Vector3 force = direction.normalized * speedMultiplier * 1000.0f;
        rb.AddForce(force);

    }

    private void GroundCheck() {
        
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && speedMultiplier != speed)
            speedMultiplier = speed;
        else if(!isGrounded && speedMultiplier != airSpeed)
            speedMultiplier = airSpeed;

    }

    private void Jump(InputAction.CallbackContext ctx) {
        
        if (!isGrounded) return;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0.0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

    }

}
