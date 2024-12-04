using UnityEngine;

public class HandCollider : MonoBehaviour
{
    public int attackDamage = 30; // Урон, который наносит рука
    public string enemyTag = "Enemy"; // Тег врагов

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что мы столкнулись с врагом
        if (other.CompareTag(enemyTag))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage); // Наносим урон врагу
            }
        }
    }
}
