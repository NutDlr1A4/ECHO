using System;
using UnityEngine;

public enum ConnectionPoint {
    TOP,
    BOTTOM,
    LEFT,
    RIGHT
}

public class EchoNodeManager : MonoBehaviour
{
    [SerializeField] private Transform connections_parent;
    [SerializeField] private GameObject connection_prefab;

    public void ConnectNodes(GameObject node1, ConnectionPoint con1, GameObject node2, ConnectionPoint con2) {
        GameObject new_connection = Instantiate(connection_prefab, connections_parent);
        new_connection.GetComponent<EchoNodeConnection>().Initialize(node1, node2);

        var n1 = node1.GetComponent<EchoNode>();
        var n2 = node2.GetComponent<EchoNode>();
        n1.node_dict[con1] = node2;
        n1.connection = new_connection.GetComponent<EchoNodeConnection>();
        n2.node_dict[con2] = node1;
        n2.connection = new_connection.GetComponent<EchoNodeConnection>();
    }

    public void RemoveConnection(GameObject node1, ConnectionPoint con1, GameObject node2, ConnectionPoint con2) {
        // TODO: Implement this
        return;
    }

    public GameObject GetConnectedNode(GameObject node, ConnectionPoint con) {
        if (node.GetComponent<EchoNode>().node_dict.ContainsKey(con)) {
            return node.GetComponent<EchoNode>().node_dict[con];
        }

        return null;
    } 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        ConnectNodes(
            GameObject.Find("/MiscNodes/Node_LeftBottom"),
            ConnectionPoint.RIGHT,
            GameObject.Find("/MiscNodes/Node_MiddleBottom"),
            ConnectionPoint.LEFT
        );

        ConnectNodes(
            GameObject.Find("/MiscNodes/Node_MiddleBottom"),
            ConnectionPoint.RIGHT,
            GameObject.Find("/MiscNodes/Node_RightBottom"),
            ConnectionPoint.LEFT
        );

        ConnectNodes(
            GameObject.Find("/MiscNodes/Node_MiddleBottom"),
            ConnectionPoint.TOP,
            GameObject.Find("/MiscNodes/Node_MiddleTop"),
            ConnectionPoint.BOTTOM
        );

        ConnectNodes(
            GameObject.Find("/MiscNodes/Node_MiddleTop"),
            ConnectionPoint.LEFT,
            GameObject.Find("/MiscNodes/Node_LeftTop"),
            ConnectionPoint.RIGHT
        );

        ConnectNodes(
            GameObject.Find("/MiscNodes/Node_RightBottom"),
            ConnectionPoint.TOP,
            GameObject.Find("/MiscNodes/Node_RightTop"),
            ConnectionPoint.BOTTOM
        );
    }

    // Update is called once per frame
    void Update() {
        
    }
}
