using System.Collections.Generic;
using UnityEngine;


public class EchoNodeConnection : MonoBehaviour
{
    private List<Transform> nodes;

    public void Initialize(GameObject node1, GameObject node2) {
        nodes = new List<Transform>{node1.transform, node2.transform};
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        gameObject.transform.position = nodes[0].position;

        Vector2 direction = nodes[1].position - gameObject.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        
        gameObject.transform.localScale = new Vector3(direction.magnitude, 1, 1);

        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 1);
    }
}
