using UnityEngine;

public class UnitCardAnimator : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Метод для зміни анімації Idle
    public void SetIdle(bool isIdle)
    {
        if (animator != null)
        {
            animator.SetBool("isIdle", isIdle);
        }
    }

    // Метод для анімації атаки
    public void SetAttack(bool isAttacking)
    {
        if (animator != null)
        {
            animator.SetBool("isAttacking", isAttacking);
        }
    }

    // Метод для анімації смерті
    public void SetDeath(bool isDead)
    {
        if (animator != null)
        {
            animator.SetBool("isDead", isDead);
        }
    }
}
