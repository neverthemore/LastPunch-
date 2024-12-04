using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f; // Скорость движения врага
    public float detectionRange = 10f; // Радиус обнаружения игрока
    public int health = 30; // Здоровье врага

    private Transform player; // Ссылка на игрока

    void Start()
    {
        // Поиск игрока по тегу
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Если игрок в радиусе обнаружения, двигаться к нему
            if (distanceToPlayer <= detectionRange)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;
            }
        }
    }

    // Метод для получения урона
    public void TakeDamage(int attackDamage)
    {
        health -= attackDamage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject); // Уничтожаем врага
    }
}
