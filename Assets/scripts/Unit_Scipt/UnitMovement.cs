using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public UnitCard unitCard; // Посилання на UnitCard (ScriptableObject)
    private Vector3 targetPosition;
    private bool hasTarget = false;
    private bool isAttacking = false;
    private bool isDead = false;
    private UnitCardAnimator unitCardAnimator;
    private Rigidbody2D rb;
    private GameObject currentEnemy; // Посилання на ворога
    private float currentHealth; // Індивідуальне здоров'я юніта

    void Start()
    {
        unitCardAnimator = GetComponent<UnitCardAnimator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = unitCard.health; // Копіюємо здоров'я з ScriptableObject
    }

    void Update()
    {
        if (hasTarget && !isAttacking && !isDead)
        {
            MoveToTarget();
        }
    }

    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
        hasTarget = true;
    }

    private void MoveToTarget()
    {
        float step = unitCard.speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        if ((targetPosition - transform.position).sqrMagnitude < 0.01f)
        {
            hasTarget = false;
            unitCardAnimator?.SetIdle(true);
        }
        else
        {
            unitCardAnimator?.SetIdle(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            currentEnemy = other.gameObject;
            isAttacking = true;
            rb.velocity = Vector2.zero;
            unitCardAnimator?.SetAttack(true);
            InvokeRepeating("Attack", 0f, 1f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            isAttacking = false;
            unitCardAnimator?.SetAttack(false);
            CancelInvoke("Attack");
            currentEnemy = null;
        }
    }

    void Attack()
    {
        if (currentEnemy != null)
        {
            currentEnemy.GetComponent<EnemyAI>().TakeDamage(unitCard.damage);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Змінюємо індивідуальне здоров'я юніта

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (!isDead)
        {
            Collider2D col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;
            isDead = true;
            unitCardAnimator?.SetDeath(true);
            Destroy(gameObject, 2f); // Видаляємо юніта через 1 секунду
        }
    }
}
