using UnityEngine;

public class PlayerArmRotation : MonoBehaviour
{
    public Transform leftShoulder; // ������ �� ����� �����
    public Transform rightShoulder; // ������ �� ������ �����
    public Transform leftArm; // ������ �� ����� ����
    public Transform rightArm; // ������ �� ������ ����
    public Camera mainCamera; // ������ �� �������� ������

    void Update()
    {
        RotateArmsTowardsMouse();
    }

    void RotateArmsTowardsMouse()
    {
        // �������� ������� ������� �� ������
        Vector3 mouseScreenPosition = Input.mousePosition;

        // ����������� �������� ���������� � �������
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, mainCamera.transform.position.z));
        mouseWorldPosition.z = 0; // ������� �������, ��� ��� �������� � 2D

        // ������������ ����������� �� ����� �� �������
        Vector3 leftDirection = mouseWorldPosition - leftShoulder.position;
        Vector3 rightDirection = mouseWorldPosition - rightShoulder.position;

        // ������������ ���� ��������
        float leftAngle = Mathf.Atan2(leftDirection.y, leftDirection.x) * Mathf.Rad2Deg;
        float rightAngle = Mathf.Atan2(rightDirection.y, rightDirection.x) * Mathf.Rad2Deg;

        // ������������ �����
        leftShoulder.rotation = Quaternion.Euler(0, 0, leftAngle);
        rightShoulder.rotation = Quaternion.Euler(0, 0, rightAngle);

        // ������������ ����
        leftArm.rotation = leftShoulder.rotation; // ���� ������� �� ������
        rightArm.rotation = rightShoulder.rotation; // ���� ������� �� ������
    }
}
