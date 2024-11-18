using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Ссылка на игрока.
    [SerializeField]
    public float distance = 10f; // Расстояние от камеры до игрока.

    [SerializeField]
    public float height = 5f; // Высота камеры над игроком.

    [SerializeField]
    public float rotationSpeed = 90f; // Угол поворота за нажатие клавиши.

    private float currentAngle = 0f;

    void LateUpdate()
    {
        // Поворот камеры с помощью Q и E.
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RotateCamera(-rotationSpeed);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            RotateCamera(rotationSpeed);
        }

        // Обновляем позицию камеры.
        Vector3 offset = new Vector3(
            Mathf.Sin(currentAngle * Mathf.Deg2Rad) * distance,
            height,
            Mathf.Cos(currentAngle * Mathf.Deg2Rad) * distance
        );

        transform.position = player.position + offset;

        // Камера всегда смотрит на игрока.
        transform.LookAt(player.position);
    }

    private void RotateCamera(float angle)
    {
        currentAngle += angle;
        currentAngle %= 360; // Удерживаем угол в пределах 0–360.
    }
}
