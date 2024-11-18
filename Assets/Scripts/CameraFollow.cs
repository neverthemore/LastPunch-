using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // ������ �� ������.
    [SerializeField]
    public float distance = 10f; // ���������� �� ������ �� ������.

    [SerializeField]
    public float height = 5f; // ������ ������ ��� �������.

    [SerializeField]
    public float rotationSpeed = 90f; // ���� �������� �� ������� �������.

    private float currentAngle = 0f;

    void LateUpdate()
    {
        // ������� ������ � ������� Q � E.
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RotateCamera(-rotationSpeed);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            RotateCamera(rotationSpeed);
        }

        // ��������� ������� ������.
        Vector3 offset = new Vector3(
            Mathf.Sin(currentAngle * Mathf.Deg2Rad) * distance,
            height,
            Mathf.Cos(currentAngle * Mathf.Deg2Rad) * distance
        );

        transform.position = player.position + offset;

        // ������ ������ ������� �� ������.
        transform.LookAt(player.position);
    }

    private void RotateCamera(float angle)
    {
        currentAngle += angle;
        currentAngle %= 360; // ���������� ���� � �������� 0�360.
    }
}
