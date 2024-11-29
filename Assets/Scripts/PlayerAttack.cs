using UnityEngine;

public class HandAttack : MonoBehaviour
{
    public Transform hand; // ������ �� ������ ����.
    public float attackRange = 1.0f; // ������ ����� ����.
    public int damage = 10; // ����, ������� ��������� �����.
    public LayerMask enemyLayer; // ���� ������.

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // ��������� ������� ����, ����� ��� ��������� �� ��������.
        UpdateHandPosition();

        // ����� ��� ������� ����� ������ ����.
        if (Input.GetMouseButtonDown(0))
        {
            PerformAttack();
        }
    }

    void UpdateHandPosition()
    {
        // �������� ������� ������� � ������� �����������.
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        // ��������� �� ������ ���������.
        Plane plane = new Plane(Vector3.forward, Vector3.zero);
        if (plane.Raycast(ray, out float distance))
        {
            Vector3 worldPosition = ray.GetPoint(distance);
            hand.position = new Vector3(worldPosition.x, worldPosition.y, hand.position.z);
        }
    }

    void PerformAttack()
    {
        // ��������� ������ � ������� �����.
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(hand.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            // ������� ���� �����.
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // ���������� ������ ����� � ���������.
        if (hand != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hand.position, attackRange);
        }
    }
}
