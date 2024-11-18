using UnityEngine;

public class CharacterRotation : MonoBehaviour
{
    public Transform cameraTransform; // ������ �� ������.

    private Vector3[] faceDirections = {
        new Vector3(0, 0, 1),  // �����
        new Vector3(-1, 0, 0), // �����
        new Vector3(0, 0, -1), // �����
        new Vector3(1, 0, 0)   // ������
    };

    void Update()
    {
        // ���������� ���� ������ ������ ������.
        float cameraYAngle = cameraTransform.eulerAngles.y;

        // �������� ���� � ��������� 0, 90, 180, 270 ��� �������� 4 �����������.
        float normalizedAngle = Mathf.Round(cameraYAngle / 90f) * 90f;

        // ��������� ������ �����������.
        int directionIndex = (int)(normalizedAngle / 90f) % faceDirections.Length;

        // ������������� ����������� ���������.
        transform.forward = faceDirections[directionIndex];
    }
}
