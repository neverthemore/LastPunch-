using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Ссылка на объект персонажа
    public float height = 5f; // Высота камеры над персонажем
    public float distance = 10f; // Расстояние камеры от персонажа
    public Vector2 offset; // Смещение камеры относительно персонажа
    public float followSpeed = 5f; // Скорость, с которой камера следует за персонажем

    void LateUpdate()
    {
        if (target == null) return;

        // Позиция цели с учётом смещения
        Vector3 targetPosition = target.position + new Vector3(offset.x, offset.y, 0);

        // Позиция камеры с учётом высоты и расстояния
        Vector3 desiredPosition = targetPosition + Vector3.back * distance + Vector3.up * height;

        // Плавное перемещение камеры
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // Поворачиваем камеру так, чтобы она всегда смотрела на персонажа
        transform.LookAt(targetPosition);
    }
}
