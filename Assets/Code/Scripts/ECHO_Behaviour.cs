using System.Collections;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class ECHOBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject initial_node;
    private GameObject previous_node;
    private GameObject current_node;

    [SerializeField] private float move_speed = 30f;

    [SerializeField] private float move_buffer_time = 0.2f;
    private float last_buffer_time = 0.0f;
    private GameObject queued_node;

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

    private float DistToTarget() {
        return (current_node.transform.position - gameObject.transform.position).magnitude;
    }

    private bool DoInputBuffer() {
        return DistToTarget() <= (move_speed * move_buffer_time); 
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
            if (DoInputBuffer()) {
                queued_node = GetNodeFromMovementInput(moveInput);
                last_buffer_time = 0.0f;
                Debug.Log("Buffered input!");
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
        on_node = false;

        return true;
    }

    void Start() {
        gameObject.transform.position = initial_node.transform.position;
        current_node = initial_node;
        on_node = true;
    }

    void Update() {
        if (on_node) {
            gameObject.transform.position = current_node.transform.position;
            return;
        }

        last_buffer_time += Time.deltaTime;
        
        Vector2 delta = current_node.transform.position - gameObject.transform.position;
        Vector2 move_distance = delta.normalized * move_speed * Time.deltaTime;

        if (move_distance.sqrMagnitude >= delta.sqrMagnitude) {
            gameObject.transform.position = current_node.transform.position;
            on_node = true;

            if (last_buffer_time <= move_buffer_time) {
                TryMoveTo(queued_node);
            }
            queued_node = null;
        }
        else {
            gameObject.transform.position += (Vector3)move_distance;
        }
    }
}