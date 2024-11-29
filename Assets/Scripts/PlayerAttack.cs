using UnityEngine;

public class HandAttack : MonoBehaviour
{
    public Transform hand; // Ссылка на объект руки.
    public float attackRange = 1.0f; // Радиус атаки руки.
    public int damage = 10; // Урон, который наносится врагу.
    public LayerMask enemyLayer; // Слой врагов.

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Обновляем позицию руки, чтобы она следовала за курсором.
        UpdateHandPosition();

        // Атака при нажатии левой кнопки мыши.
        if (Input.GetMouseButtonDown(0))
        {
            PerformAttack();
        }
    }

    void UpdateHandPosition()
    {
        // Получаем позицию курсора в мировых координатах.
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        // Плоскость на уровне персонажа.
        Plane plane = new Plane(Vector3.forward, Vector3.zero);
        if (plane.Raycast(ray, out float distance))
        {
            Vector3 worldPosition = ray.GetPoint(distance);
            hand.position = new Vector3(worldPosition.x, worldPosition.y, hand.position.z);
        }
    }

    void PerformAttack()
    {
        // Проверяем врагов в радиусе атаки.
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(hand.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            // Наносим урон врагу.
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Отображаем радиус атаки в редакторе.
        if (hand != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hand.position, attackRange);
        }
    }
}
