using System;
using Unity.VisualScripting;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour, IInteractable, IHackable
{
    [SerializeField] private bool open = false;

    private Animator animator;

    public void OnHack() {
        OnInteract();
    }

    public void OnInteract() {
        Toggle();
    }

    public void OnInteractTargetEnter() {}
    public void OnInteractTargetExit() {}

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
        animator = GetComponent<Animator>();
    }

    void Start() {
        
    }

    void Update()
    {
        
    }
}