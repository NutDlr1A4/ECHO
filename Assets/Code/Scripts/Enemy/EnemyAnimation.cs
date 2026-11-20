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

    public void isHostile(bool hostile)
    {
        if (animator.GetBool("isHostile") != hostile)
        animator.SetBool("isHostile", hostile);
    }
    public void PlayIdle()
    {
        animator.SetBool("isRunning", false);
    }
    public void PlayAttack()
    {
        animator.SetBool("isRunning", false);
        animator.SetTrigger("isHostile");
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