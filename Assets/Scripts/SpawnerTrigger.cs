using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    public EnemySpawner enemySpawner; // Ссылка на EnemySpawner
    private bool hasSpawned = false; // Флаг, чтобы избежать повторного спавна

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasSpawned)
        {
            enemySpawner.SpawnEnemies(); // Спавн врагов
            hasSpawned = true; // Установка флага, чтобы избежать повторного спавна
            Destroy(gameObject); // Удаление триггера
        }
    }


}