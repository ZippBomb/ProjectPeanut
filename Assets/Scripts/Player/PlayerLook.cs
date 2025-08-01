using UnityEngine;

public class PlayerLook : MonoBehaviour {

    [SerializeField] private float sensitivity = 1.0f;
    [SerializeField] private Transform camera;

    private MainInput.PlayerActions inputMap;
    private float pitch;

    private void Start() {

        inputMap = Player.instance.input;
        pitch = camera.localEulerAngles.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
    private void Update() {

        Vector2 input = inputMap.Look.ReadValue<Vector2>() * sensitivity * 15.0f * Time.deltaTime;
        pitch = Mathf.Clamp(pitch - input.y, -90.0f, 90.0f);

        transform.Rotate(Vector3.up, input.x);
        camera.localRotation = Quaternion.AngleAxis(pitch, Vector3.right);

    }

}