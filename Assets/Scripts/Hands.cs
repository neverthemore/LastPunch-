using UnityEngine;

public class Hands : MonoBehaviour
{
    public Collider leftHandCollider; // Коллайдер для левой руки
    public Collider rightHandCollider; // Коллайдер для правой руки

    private void OnTriggerEnter(Collider other)
    {
        // Проверка на столкновение с хитбоксами врага
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                // Проверяем, какой коллайдер активен и наносим соответствующий урон
                int damage = 0;

                if (leftHandCollider.enabled) // Проверка для левой руки
                {
                    if (other.gameObject.name.Contains("Head")) // Хитбокс головы
                    {
                        damage = enemy.headDamage; // Используем свойство из Enemy
                    }
                    else if (other.gameObject.name.Contains("Body")) // Хитбокс тела
                    {
                        damage = enemy.bodyDamage; // Используем свойство из Enemy
                    }
                    else if (other.gameObject.name.Contains("Legs")) // Хитбокс ног
                    {
                        damage = enemy.legDamage; // Используем свойство из Enemy
                    }
                }

                if (rightHandCollider.enabled) // Проверка для правой руки
                {
                    if (other.gameObject.name.Contains("Head")) // Хитбокс головы
                    {
                        damage = enemy.headDamage; // Используем свойство из Enemy
                    }
                    else if (other.gameObject.name.Contains("Body")) // Хитбокс тела
                    {
                        damage = enemy.bodyDamage; // Используем свойство из Enemy
                    }
                    else if (other.gameObject.name.Contains("Legs")) // Хитбокс ног
                    {
                        damage = enemy.legDamage; // Используем свойство из Enemy
                    }
                }

                // Наносим урон, если есть
                if (damage > 0)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }
    }
}