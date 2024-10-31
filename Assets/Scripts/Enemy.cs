using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 30;
    public Transform player;
    public float speed = 2.0f;

    private void Update()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject); // ”ничтожаем врага
    }
}
