using UnityEngine;

public class RoomDoor : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        animator.SetBool("IsOpen", !animator.GetBool("IsOpen"));
    }
}