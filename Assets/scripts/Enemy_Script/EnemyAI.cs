using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public UnitCard unitCard; // Посилання на ScriptableObject UnitCard
    public float attackCooldown = 1f;
    private bool isAttacking = false;
    private bool isDead = false;
    private Rigidbody2D rb;
    private float currentHealth;
    private UnitCardAnimator unitCardAnimator;

    private UnitMovement targetUnit; // Посилання на об'єкт юніта, а не на ScriptableObject!

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        unitCardAnimator = GetComponent<UnitCardAnimator>();
        currentHealth = unitCard.health; // Ініціалізація здоров'я ворога з UnitCard
    }

    void Update()
    {
        if (!isAttacking && !isDead)
        {
            MoveTowardsTarget();
        }
    }

    private void MoveTowardsTarget()
    {
        transform.position += Vector3.left * unitCard.speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Unit")) // Якщо торкнувся юніта
        {
            isAttacking = true;
            unitCardAnimator?.SetAttack(true);
            targetUnit = other.GetComponent<UnitMovement>(); // Отримуємо об'єкт юніта
            rb.velocity = Vector2.zero;
            InvokeRepeating("Attack", 0f, attackCooldown);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Unit")) // Якщо юніт вийшов з радіусу
        {
            isAttacking = false;
            unitCardAnimator?.SetAttack(false);
            targetUnit = null; // Обнуляємо посилання на юніта
            CancelInvoke("Attack");
        }
    }

    void Attack()
    {
        if (targetUnit != null) // Якщо юніт ще живий
        {
            targetUnit.TakeDamage(unitCard.damage); // Використовуємо метод пошкодження юніта
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        isDead = true;
        unitCardAnimator?.SetDeath(true);
        Destroy(gameObject, 2f); // Видаляємо ворога після смерті
    }
}
