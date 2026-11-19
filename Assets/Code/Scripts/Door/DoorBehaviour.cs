using System;
using Unity.VisualScripting;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour, IInteractable, IHackable
{
    [SerializeField] private bool open = false;

    public void OnHack() {
        Toggle();
    }

    public void OnInteract() {
        Toggle();
    }

    public void OnInteractTargetEnter() {}
    public void OnInteractTargetExit() {}

    public void Toggle() {
        open = !open;
    }

    public void Open() { 
        open = true;
    }

    public void Close() {
        open = false;
    }

    public bool IsOpen() { return open; }

    void Start() {
        
    }

    void Update()
    {
        // Update the shit needed to open/close it (hitboxes and sprites)

        GameObject rect_open = gameObject.transform.Find("RectOpen").gameObject;
        GameObject rect_closed = gameObject.transform.Find("RectClosed").gameObject;

        rect_open.SetActive(open);
        rect_open.GetComponent<BoxCollider2D>().gameObject.SetActive(open);
        rect_closed.SetActive(!open);
        rect_closed.GetComponent<BoxCollider2D>().gameObject.SetActive(!open);
    }
}
