using UnityEngine;

public class TestPatrol_Movement : MonoBehaviour
{
    [SerializeField] private Vector2 point_1;
    [SerializeField] private Vector2 point_2;
    [SerializeField] private float patrol_speed;

    private Vector2 current_target;
    private bool a2b = true;

    private void SwitchTarget() {
        if (a2b) {
            a2b = false;
            current_target = point_2;
        }
        else {
            a2b = true;
            current_target = point_1;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        current_target = point_1;
        gameObject.transform.position = point_1;
    }

    // Update is called once per frame
    void Update() {
        Vector2 delta = current_target - (Vector2)gameObject.transform.position;
        Vector2 move_distance = delta.normalized * patrol_speed * Time.deltaTime;

        if (move_distance.sqrMagnitude >= delta.sqrMagnitude) {
            gameObject.transform.position = current_target;
            SwitchTarget();
        }
        else {
            gameObject.transform.position += (Vector3)move_distance;
        }
    }
}
