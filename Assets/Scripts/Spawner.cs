using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnPoint
    {
        public Transform point; // ����� ������
        public float spawnRadius = 5f; // ������ ������
    }

    public GameObject enemyPrefab; // ������ �����
    public SpawnPoint[] spawnPoints; // ������ ����� ������
    public float spawnDelay = 5f; // �������� ����� ������� ���������� �����
    public int maxEnemies = 5; // ������������ ���������� ������

    private List<GameObject> activeEnemies = new List<GameObject>(); // ������ �������� ������

    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }

    void Update()
    {
        // ������� �� ������ ������������ ������
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
            // �������� ��������� ����� ������
            SpawnPoint spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // ������������ ��������� ������� � �������� �������
            Vector3 randomOffset = Random.insideUnitSphere * spawnPoint.spawnRadius;
            randomOffset.y = 0; // �� �������� ������

            Vector3 spawnPosition = spawnPoint.point.position + randomOffset;

            // ������� ����� � ��������� ��� � ������
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            activeEnemies.Add(enemy);
        }
    }

    // ����������� �������� ������ � ���������
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
