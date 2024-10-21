using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Скорость движения
    public float attackRange = 1f; // Диапазон атаки
    public LayerMask enemyLayers; // Слой врагов
    public int attackDamage = 10; // Урон атаки

    private Rigidbody rb;
    private Animator animator;
    private Vector3 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Получение ввода для движения
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");

        // Нормализация движения для диагоналей
        if (movement.magnitude > 1)
        {
            movement = movement.normalized;
        }
        movement = new Vector3(movement.x, 0, movement.z);
        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        // Обновление параметров Animator
        float currentSpeed = movement.magnitude;
        animator.SetFloat("Speed", currentSpeed);

        // Поворот персонажа по горизонтали
        Quaternion rotation = Quaternion.LookRotation(new Vector3(movement.x, 0, movement.z));
        transform.rotation = rotation;

        // Обработка атаки при нажатии ЛКМ
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    void FixedUpdate()
    {
        // Перемещение персонажа
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    void Attack()
    {
        // Воспроизведение анимации атаки
        animator.SetTrigger("AttackTrigger");

        // Определение позиции атаки (например, впереди персонажа)
        Vector3 attackPosition = rb.position + movement.normalized * attackRange;

        // Поиск врагов в диапазоне атаки
        Collider[] hitEnemies = Physics.OverlapSphere(attackPosition, attackRange, enemyLayers);

        // Нанесение урона каждому врагу в диапазоне
        foreach (Collider enemy in hitEnemies)
        {
            // Предполагается, что у врагов есть скрипт с методом TakeDamage
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(attackDamage);
            }
        }
    }

    // Визуализация области атаки в редакторе
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 attackPosition = rb != null ? rb.position + movement.normalized * attackRange : Vector3.zero;
        Gizmos.DrawWireSphere(attackPosition, attackRange);
    }
}