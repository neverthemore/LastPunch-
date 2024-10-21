using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // �������� ��������
    public float attackRange = 1f; // �������� �����
    public LayerMask enemyLayers; // ���� ������
    public int attackDamage = 10; // ���� �����

    private Rigidbody rb;
    private Animator animator;
    private Vector3 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // ��������� ����� ��� ��������
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");

        // ������������ �������� ��� ����������
        if (movement.magnitude > 1)
        {
            movement = movement.normalized;
        }
        movement = new Vector3(movement.x, 0, movement.z);
        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        // ���������� ���������� Animator
        float currentSpeed = movement.magnitude;
        animator.SetFloat("Speed", currentSpeed);

        // ������� ��������� �� �����������
        Quaternion rotation = Quaternion.LookRotation(new Vector3(movement.x, 0, movement.z));
        transform.rotation = rotation;

        // ��������� ����� ��� ������� ���
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    void FixedUpdate()
    {
        // ����������� ���������
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    void Attack()
    {
        // ��������������� �������� �����
        animator.SetTrigger("AttackTrigger");

        // ����������� ������� ����� (��������, ������� ���������)
        Vector3 attackPosition = rb.position + movement.normalized * attackRange;

        // ����� ������ � ��������� �����
        Collider[] hitEnemies = Physics.OverlapSphere(attackPosition, attackRange, enemyLayers);

        // ��������� ����� ������� ����� � ���������
        foreach (Collider enemy in hitEnemies)
        {
            // ��������������, ��� � ������ ���� ������ � ������� TakeDamage
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(attackDamage);
            }
        }
    }

    // ������������ ������� ����� � ���������
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 attackPosition = rb != null ? rb.position + movement.normalized * attackRange : Vector3.zero;
        Gizmos.DrawWireSphere(attackPosition, attackRange);
    }
}