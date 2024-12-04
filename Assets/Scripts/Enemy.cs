using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f; // �������� �������� �����
    public float detectionRange = 10f; // ������ ����������� ������
    public int health = 30; // �������� �����

    private Transform player; // ������ �� ������

    void Start()
    {
        // ����� ������ �� ����
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // ���� ����� � ������� �����������, ��������� � ����
            if (distanceToPlayer <= detectionRange)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;
            }
        }
    }

    // ����� ��� ��������� �����
    public void TakeDamage(int attackDamage)
    {
        health -= attackDamage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject); // ���������� �����
    }
}
