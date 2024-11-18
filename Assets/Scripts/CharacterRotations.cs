using UnityEngine;

public class CharacterRotation : MonoBehaviour
{
    public Transform cameraTransform; // Ссылка на камеру.

    private Vector3[] faceDirections = {
        new Vector3(0, 0, 1),  // Вперёд
        new Vector3(-1, 0, 0), // Влево
        new Vector3(0, 0, -1), // Назад
        new Vector3(1, 0, 0)   // Вправо
    };

    void Update()
    {
        // Определяем угол камеры вокруг игрока.
        float cameraYAngle = cameraTransform.eulerAngles.y;

        // Приводим угол к значениям 0, 90, 180, 270 для фиксации 4 направлений.
        float normalizedAngle = Mathf.Round(cameraYAngle / 90f) * 90f;

        // Вычисляем индекс направления.
        int directionIndex = (int)(normalizedAngle / 90f) % faceDirections.Length;

        // Устанавливаем направление персонажа.
        transform.forward = faceDirections[directionIndex];
    }
}
