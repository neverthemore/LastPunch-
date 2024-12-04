using UnityEngine;

public class HandCollider : MonoBehaviour
{
    private HandFollowCursor handDamage; // Ссылка на компонент с данными урона
    public LayerMask enemyLayer;  // Слой врагов

    void Start()
    {
        // Ищем компонент HandDamage на родительском объекте
        handDamage = GetComponentInParent<HandFollowCursor>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что объект находится на слое врагов
        if ((enemyLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            // Получаем компонент Enemy на объекте врага
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                // Наносим урон врагу
                enemy.TakeDamage(handDamage.attackDamage);
            }
        }
    }
}
