using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;

    public void TakeDamage(float damage)
    {
        health -= (int)damage;
        Debug.Log($"Player took {damage} damage! Remaining health: {health}");
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died!");  
        Destroy(gameObject);
    }
}