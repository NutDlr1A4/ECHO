using System;
using System.Collections.Generic;
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

    [Serializable] public class ConnectionPair {
        public GameObject node_1;
        public ConnectionPoint connection_point_1;
        public GameObject node_2;
        public ConnectionPoint connection_point_2;
    };
    [SerializeField] private List<ConnectionPair> initial_connections = new();

    public void ConnectNodes(GameObject node1, ConnectionPoint con1, GameObject node2, ConnectionPoint con2) {
        GameObject new_connection = Instantiate(connection_prefab, connections_parent);
        new_connection.GetComponent<EchoNodeConnection>().Initialize(node1, node2);

        var n1 = node1.GetComponent<EchoNode>();
        var n2 = node2.GetComponent<EchoNode>();
        n1.node_dict[con1] = node2;
        n1.connection = new_connection;
        n2.node_dict[con2] = node1;
        n2.connection = new_connection;
    }

    public void RemoveConnection(GameObject node1, ConnectionPoint con1, GameObject node2, ConnectionPoint con2) {
        var n1 = node1.GetComponent<EchoNode>();
        var n2 = node2.GetComponent<EchoNode>();

        var connection = node1.GetComponent<EchoNodeConnection>();
        if (connection != node2.GetComponent<EchoNodeConnection>()) {
            Debug.Log("Could not remove connection, node connection mismatch");
        }
        
        n1.node_dict.Remove(con1);
        n2.node_dict.Remove(con2);

        Destroy(n1.connection);
    }

    public GameObject GetConnectedNode(GameObject node, ConnectionPoint con) {
        if (node.GetComponent<EchoNode>().node_dict.ContainsKey(con)) {
            return node.GetComponent<EchoNode>().node_dict[con];
        }

        return null;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        foreach(ConnectionPair conpair in initial_connections) {
            ConnectNodes(
                conpair.node_1,
                conpair.connection_point_1,
                conpair.node_2,
                conpair.connection_point_2
            );
        }
    }

    // Update is called once per frame
    void Update() {
        
    }
}
