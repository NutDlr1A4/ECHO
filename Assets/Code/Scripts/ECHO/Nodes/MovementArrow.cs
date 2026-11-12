using UnityEngine;

public class MovementArrow : MonoBehaviour
{
    public GameObject node_at;
    public GameObject node_point_at;

    public void SetNewPos() {
        gameObject.transform.position = node_at.transform.position;

        Vector2 direction = node_point_at.transform.position - gameObject.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 1);

        gameObject.transform.Translate(1, 0, 0);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (node_point_at == null) return;
        SetNewPos();
    }
}
