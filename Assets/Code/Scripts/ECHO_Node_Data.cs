using UnityEngine;

public class ECHO_Node_Data : MonoBehaviour
{
    [SerializeField] private GameObject up;
    [SerializeField] private GameObject down;
    [SerializeField] private GameObject left;
    [SerializeField] private GameObject right;

    public GameObject GetNodeUp() {
        return up;
    }
    public GameObject GetNodeDown() {
        return down;
    } 
    public GameObject GetNodeLeft() {
        return left;
    } 
    public GameObject GetNodeRight() {
        return right;
    } 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }
}
