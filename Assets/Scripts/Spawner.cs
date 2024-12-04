using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnPoint
    {
        public Transform point; // Точка спавна
        public float spawnRadius = 5f; // Радиус спавна
    }

    public GameObject enemyPrefab; // Префаб врага
    public SpawnPoint[] spawnPoints; // Массив точек спавна
    public float spawnDelay = 5f; // Задержка перед спавном следующего врага
    public int maxEnemies = 5; // Максимальное количество врагов

    private List<GameObject> activeEnemies = new List<GameObject>(); // Список активных врагов

    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }

    void Update()
    {
        // Удаляем из списка уничтоженных врагов
        activeEnemies.RemoveAll(enemy => enemy == null);
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            if (activeEnemies.Count < maxEnemies)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab != null && spawnPoints.Length > 0)
        {
            // Выбираем случайную точку спавна
            SpawnPoint spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Рассчитываем случайную позицию в пределах радиуса
            Vector3 randomOffset = Random.insideUnitSphere * spawnPoint.spawnRadius;
            randomOffset.y = 0; // Не изменяем высоту

            Vector3 spawnPosition = spawnPoint.point.position + randomOffset;

            // Создаем врага и добавляем его в список
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            activeEnemies.Add(enemy);
        }
    }

    // Отображение радиусов спавна в редакторе
    private void OnDrawGizmos()
    {
        if (spawnPoints != null)
        {
            Gizmos.color = Color.magenta;
            foreach (var spawnPoint in spawnPoints)
            {
                if (spawnPoint.point != null)
                {
                    Gizmos.DrawWireSphere(spawnPoint.point.position, spawnPoint.spawnRadius);
                }
            }
        }
    }
}
