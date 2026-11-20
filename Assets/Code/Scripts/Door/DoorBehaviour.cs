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
        if (state != open) {
            animator.SetBool("IsOpen", !animator.GetBool("IsOpen"));
        }
        open = state; // This? State? Hit the road dude
    }

    void Awake()
    {
        Debug.Log(gameObject + ": " + open);
        animator = GetComponent<Animator>();
        animator.SetBool("IsOpen", open);
    }

    void Start()  {
        
    }

    void Update()
    {
        
    }
}