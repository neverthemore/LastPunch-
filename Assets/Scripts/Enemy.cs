using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f; // �������� �������� �����
    public float detectionRange = 10f; // ������ ����������� ����
    public int health = 30; // �������� �����
    public Camera mainCamera; // ������, �� ������� ���� ������ ��������

    //public string sortinglayer; // ���� �������� ��� ����� 

    private Transform target; // ���� (����� ��� ������ ������)
    private Rigidbody rb; // ������ �� Rigidbody �����

    void Start()
    {
        // ����� ������ �� ����
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            target = playerObject.transform;
        }

        // �������� Rigidbody
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.freezeRotation = true; // ��������� ���������� �������� �����
        }

        // ���� ������ �� �������, ����� ��������
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        
    }

    //private void Update()
    //{
    //    Setsortinglayer(sortinglayer);
    //}

    public void Setsortinglayer(string layer)
    {
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.sortingLayerName = layer;
        }
    }
    void FixedUpdate()
    {
        // ���������, ���� �� ����
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            // ���� ���� � ������� �����������, ��������� � ���
            if (distanceToTarget <= detectionRange)
            {
                // ������������ ����������� � ����
                Vector3 direction = (target.position - transform.position).normalized;

                // ��������
                rb.velocity = new Vector3(direction.x * moveSpeed, rb.velocity.y, direction.z * moveSpeed);
            }
            else
            {
                rb.velocity = Vector3.zero; // ������������� ��������, ���� ���� ��� ����
            }
        }

        // ������� ����� ������� �������� � ������
        FaceCamera();
    }

    private void FaceCamera()
    {
        if (mainCamera != null)
        {
            Vector3 cameraDirection = mainCamera.transform.position - transform.position;

            // ������� ������������ ������������, ����� ���� �� ����������
            cameraDirection.y = 0;

            // ������������ ����� � ������� ������
            if (cameraDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(-cameraDirection);
            }
        }
    }

    // ����� ��� ��������� �����
    public void TakeDamage(int attackDamage)
    {
        health -= attackDamage; // ��������� �������� �����
        Debug.Log($"Enemy took {attackDamage} damage! Remaining health: {health}");

        if (health <= 0)
        {
            Die(); // ���������� �����, ���� �������� <= 0
        }
    }

    private void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject); // ���������� ������ �����
    }
}
