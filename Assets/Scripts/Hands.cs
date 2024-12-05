using UnityEngine;

public class HandCollider : MonoBehaviour
{
    [SerializeField]
    public int attackDamage = 10;


    public void OnTriggerEnter(Collider other)
    {
        {
            // �������� ��������� Enemy �� ������� �����
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                // ������� ���� �����
                enemy.TakeDamage(attackDamage);
            }
        }
    }
}
