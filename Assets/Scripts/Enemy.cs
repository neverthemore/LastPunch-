using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 50; // Здоровье врага.

    public void TakeDamage(int damage)
    {
        // Уменьшаем здоровье врага.
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Логика уничтожения врага.
        Destroy(gameObject);
    }
}
