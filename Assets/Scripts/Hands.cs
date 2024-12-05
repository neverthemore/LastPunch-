using UnityEngine;

public class HandCollider : MonoBehaviour
{
    [SerializeField]
    public int attackDamage = 10;


    public void OnTriggerEnter(Collider other)
    {
        {
            // Получаем компонент Enemy на объекте врага
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                // Наносим урон врагу
                enemy.TakeDamage(attackDamage);
            }
        }
    }
}
