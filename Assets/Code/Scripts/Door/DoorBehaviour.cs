using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour, IInteractable, IHackable
{
    [SerializeField] private bool open = false;
    [SerializeField] private bool interactable = false;
    [SerializeField] private bool hackable = true;

    [SerializeField] private GameObject label;

    private Animator animator;
    private EchoNode my_node;

    public bool OnHack() {
        if (hackable) Toggle();
        return hackable;
    }

    public bool OnInteract() {
        if (interactable) {
            Toggle();
        }
        else {
            label.GetComponent<TextMeshProUGUI>().SetText("It won't move...");
            label.SetActive(true);
        }
        return interactable;
    }

    public void OnInteractTargetEnter() {
        if (interactable) {
            label.GetComponent<TextMeshProUGUI>().SetText("Open door (Space)");
            label.SetActive(true);
        }
    }
    public void OnInteractTargetExit() {
        label.SetActive(false);
    }

    public void Toggle() {
        SetState(!open);
    }

    public void Open() { 
        SetState(true);
    }

    public void Close() {
        SetState(false);
    }

    public bool IsOpen() { return open; }

    private void SetState(bool state) {
        open = state; // This? State? Hit the road dude

        animator.SetBool("IsOpen", open);
        
        if (open) {
            my_node.SetState(NodeState.NEUTRAL_INACTIVE);
        }
        else {
            my_node.SetState(NodeState.NEUTRAL_ACTIVE);
        }
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        my_node = gameObject.transform.Find("Node").GetComponent<EchoNode>();
        SetState(open);
    }

    void Start()  {
        
    }

    void Update()
    {
        
    }
}