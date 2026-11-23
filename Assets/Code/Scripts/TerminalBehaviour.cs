using System;
using UnityEngine;

public class TerminalInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject mode_man;
    [SerializeField] private GameObject int_text;

    private GameObject my_node;

    public bool OnInteract() {
        my_node = gameObject.transform.Find("Node").gameObject;
        my_node.GetComponent<EchoNode>().SetState(NodeState.CONTROLED);
        mode_man.GetComponent<ModeManager>().EnterECHOMode(my_node);
        return true;
    }

    public void OnInteractTargetEnter()
    {
        int_text.SetActive(true);
    }
    public void OnInteractTargetExit()
    {
        int_text.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
