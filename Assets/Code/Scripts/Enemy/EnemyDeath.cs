using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyDeath : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private bool isDead = false;
    private EnemyPatrol patrol;

    private void Start()
    {
        animator = GetComponent<Animator>();
        patrol = GetComponent<EnemyPatrol>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Die()
    {
        if (isDead)
            return;

        isDead = true;

        rb.linearVelocity = Vector2.zero;

        if (patrol != null)
        {
            patrol.StopPatrol();
        }

        animator.SetBool("isRunning", false);
        animator.SetTrigger("Death");
        transform.Rotate(0f, 0f, 90f);
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.kKey.wasPressedThisFrame)
        {
            Debug.Log("Debug:EnemyDeath: Debug test called");
            Die();

        }
    }

}
