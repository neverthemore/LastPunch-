using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; // ���� ��� ������������

    private float smoothTime = 0.1f;
    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + new Vector3(0, 6, -7); // ��������� �������� ��� ���������� ������
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        transform.position = smoothedPosition;
    }
}

public class LookAtTarget : MonoBehaviour
{
    public Transform target; // ���� ��� ������������

    void Update()
    {
        transform.LookAt(target);
    }
}