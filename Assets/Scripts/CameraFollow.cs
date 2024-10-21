using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; // Цель для отслеживания

    private float smoothTime = 0.1f;
    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + new Vector3(0, 6, -7); // Примерные значения для размещения камеры
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        transform.position = smoothedPosition;
    }
}

public class LookAtTarget : MonoBehaviour
{
    public Transform target; // Цель для отслеживания

    void Update()
    {
        transform.LookAt(target);
    }
}