using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ECHOBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject previous_node;
    [SerializeField] private GameObject current_node;

    [SerializeField] private float move_speed = 30f;
    
    [SerializeField] private float move_buffer_time = 0.2f;
    private GameObject queued_node;

    private float move_duration;
    private float move_progress;

    private bool on_node;

    private Vector2 moveInput;
    private InputSystem_Actions controls;

    private void Awake() {
        controls = new InputSystem_Actions();
    }

    private void OnEnable() {
        controls.Enable();
        controls.Player.Move.performed += OnMove;
        controls.Player.Move.canceled += OnMove;
    }

    private void OnDisable() {
        controls.Player.Move.performed -= OnMove;
        controls.Player.Move.canceled -= OnMove;
        controls.Disable();
    }

    private GameObject GetNodeFromMovementInput(Vector2 move_input) {
        ECHO_Node_Data node_data = current_node.GetComponent<ECHO_Node_Data>();

        if (move_input.y > 0.0f) {
            return node_data.GetNodeUp();
        }
        else if (move_input.y < 0.0f) {
            return node_data.GetNodeDown();
        }
        else if (move_input.x < 0.0f) {
            return node_data.GetNodeLeft();
        }
        else if (move_input.x > 0.0f) {
            return node_data.GetNodeRight();
        }
        else {
            return null;
        }
    }

    private void OnMove(InputAction.CallbackContext context) {
        if (context.canceled) return;

        moveInput = context.ReadValue<Vector2>();

        if (!on_node) {
            if (move_duration - move_progress <= move_buffer_time) {
                queued_node = GetNodeFromMovementInput(moveInput);
                Debug.Log("Buffered input!");
            }
            else {
                Debug.Log("Can't move: Is already moving!");
            }
            return;
        }

        GameObject node = GetNodeFromMovementInput(moveInput);
        TryMoveTo(node);
    }

    private bool TryMoveTo(GameObject new_node) {
        if (new_node == null) {
            Debug.Log("Can't move: No node in that direction!");
            return false;
        }

        Debug.Log("Moving to" + new_node.name);

        previous_node = current_node;
        current_node = new_node;

        move_duration = (current_node.transform.position - previous_node.transform.position).magnitude / move_speed;
        move_progress = 0.0f;
        on_node = false;

        return true;
    }

    void Start() {
        gameObject.transform.position = current_node.transform.position;
        on_node = true;
    }

    void Update() {
        if (on_node) return;

        gameObject.transform.position = Vector2.Lerp(
            current_node.transform.position,
            previous_node.transform.position,
            (move_duration - move_progress) / move_duration
        );

        move_progress += Time.deltaTime;
        if (move_progress > move_duration) {
            move_progress = move_duration;

            gameObject.transform.position = current_node.transform.position;
            on_node = true;

            if (queued_node != null) {
                TryMoveTo(queued_node);
                queued_node = null;
            }
        }
    }
}