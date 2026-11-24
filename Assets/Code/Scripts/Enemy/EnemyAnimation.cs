using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator was not found in the enemy's children.", gameObject);
        }
    }

    public void IsHostile(bool hostile)
    {
        animator.SetBool("isHostile", hostile);
        if (hostile)
        {
            animator.ResetTrigger("Holster");
            animator.SetTrigger("Draw");
        }
        else
        {
            animator.ResetTrigger("Draw");
            animator.SetTrigger("Holster");
        }
    }
    public void PlayIdle()
    {
        animator.SetBool("isRunning", false);
    }
    public void PlayAttack()
    {
        animator.SetBool("isRunning", false);
        animator.SetBool("IsHostile", true);
    }
    public void PlayRun()
    {
        animator.SetBool("isRunning", true);
    }

    public void PlayDeath()
    {
        animator.SetBool("isRunning", false);
        animator.SetTrigger("Death");
    }
}