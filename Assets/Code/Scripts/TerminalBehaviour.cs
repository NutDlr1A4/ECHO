using System;
using UnityEngine;

public class TerminalInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject mode_man;

    public bool OnInteract() {
        GameObject my_node = gameObject.transform.Find("Node").gameObject;
        mode_man.GetComponent<ModeManager>().EnterECHOMode(my_node);
        return true;
    }

    public void OnInteractTargetEnter() {}
    public void OnInteractTargetExit() {}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
