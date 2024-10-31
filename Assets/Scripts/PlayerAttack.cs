using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damage = 10; // Количество урона, наносимого врагу

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
