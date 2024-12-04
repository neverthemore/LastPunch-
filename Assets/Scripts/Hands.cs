using UnityEngine;

public class HandCollider : MonoBehaviour
{
    private HandFollowCursor handDamage; // ������ �� ��������� � ������� �����
    public LayerMask enemyLayer;  // ���� ������

    void Start()
    {
        // ���� ��������� HandDamage �� ������������ �������
        handDamage = GetComponentInParent<HandFollowCursor>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���������, ��� ������ ��������� �� ���� ������
        if ((enemyLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            // �������� ��������� Enemy �� ������� �����
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                // ������� ���� �����
                enemy.TakeDamage(handDamage.attackDamage);
            }
        }
    }
}
