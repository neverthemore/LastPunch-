using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // ������ �� ������ ���������
    public float height = 5f; // ������ ������ ��� ����������
    public float distance = 10f; // ���������� ������ �� ���������
    public Vector2 offset; // �������� ������ ������������ ���������
    public float followSpeed = 5f; // ��������, � ������� ������ ������� �� ����������

    void LateUpdate()
    {
        if (target == null) return;

        // ������� ���� � ������ ��������
        Vector3 targetPosition = target.position + new Vector3(offset.x, offset.y, 0);

        // ������� ������ � ������ ������ � ����������
        Vector3 desiredPosition = targetPosition + Vector3.back * distance + Vector3.up * height;

        // ������� ����������� ������
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // ������������ ������ ���, ����� ��� ������ �������� �� ���������
        transform.LookAt(targetPosition);
    }
}
