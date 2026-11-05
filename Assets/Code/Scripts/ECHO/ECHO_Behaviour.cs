using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ECHOBehaviour : MonoBehaviour
{
    [SerializeField] private EchoNodeManager node_man;

    [SerializeField] private GameObject initial_node;
    private GameObject current_node;

    [SerializeField] private float move_speed = 30f;

    [SerializeField] private float move_buffer_time = 0.2f;
    private float last_buffer_time = 0.0f;
    private GameObject queued_node;

    private bool on_node;

    private Vector2 moveInput;
    private InputSystem_Actions controls;

    [SerializeField] private GameObject arrow_prefab;
    private Dictionary<ConnectionPoint, GameObject> arrows = new();

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
        ConnectionPoint dir;
        if (move_input.y > 0.0f) {
            dir = ConnectionPoint.TOP;
        }
        else if (move_input.y < 0.0f) {
            dir = ConnectionPoint.BOTTOM;
        }
        else if (move_input.x < 0.0f) {
            dir = ConnectionPoint.LEFT;;
        }
        else if (move_input.x > 0.0f) {
            dir = ConnectionPoint.RIGHT;
        }
        else {
            return null;
        }

        return node_man.GetConnectedNode(current_node, dir);
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

        current_node = new_node;
        on_node = false;

        UnsetArrows();

        return true;
    }

    private void SetArrows() {
        foreach (KeyValuePair<ConnectionPoint, GameObject> item in arrows) {
            var component = item.Value.GetComponent<MovementArrow>();
            component.node_at = current_node;
            component.node_point_at = node_man.GetConnectedNode(current_node, item.Key);

            if (component.node_point_at != null) {
                component.SetNewPos();
                item.Value.SetActive(true);
            }
        }
    }

    private void UnsetArrows() {
        foreach (KeyValuePair<ConnectionPoint, GameObject> item in arrows) {
            item.Value.SetActive(false);

            var component = item.Value.GetComponent<MovementArrow>();
            component.node_point_at = null;
        }
    }

    void Start() {
        gameObject.transform.position = initial_node.transform.position;
        current_node = initial_node;
        on_node = true;


        foreach (ConnectionPoint key in System.Enum.GetValues(typeof(ConnectionPoint))) {
            arrows.Add(key, Instantiate(arrow_prefab));
        }

        SetArrows();
    }

    void Update() {
        if (on_node) {
            gameObject.transform.position = current_node.transform.position;
        }
        else {
            last_buffer_time += Time.deltaTime;
        
            Vector2 delta = current_node.transform.position - gameObject.transform.position;
            Vector2 move_distance = delta.normalized * move_speed * Time.deltaTime;

            if (move_distance.sqrMagnitude >= delta.sqrMagnitude) {
                gameObject.transform.position = current_node.transform.position;
                on_node = true;

                if (last_buffer_time <= move_buffer_time) {
                    TryMoveTo(queued_node);
                    queued_node = null;
                }
                else {
                    SetArrows();
                }
            }
            else {
                gameObject.transform.position += (Vector3)move_distance;
            }
        }

        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -1);
    }
}