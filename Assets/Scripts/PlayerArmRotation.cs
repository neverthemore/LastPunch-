using UnityEngine;

public class PlayerArmRotation : MonoBehaviour
{
    public Transform leftShoulder; // Ссылка на левое плечо
    public Transform rightShoulder; // Ссылка на правое плечо
    public Transform leftArm; // Ссылка на левую руку
    public Transform rightArm; // Ссылка на правую руку
    public Camera mainCamera; // Ссылка на основную камеру

    void Update()
    {
        RotateArmsTowardsMouse();
    }

    void RotateArmsTowardsMouse()
    {
        // Получаем позицию курсора на экране
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Преобразуем экранные координаты в мировые
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, mainCamera.transform.position.z));
        mouseWorldPosition.z = 0; // Убираем глубину, так как работаем в 2D

        // Рассчитываем направление от плеча до курсора
        Vector3 leftDirection = mouseWorldPosition - leftShoulder.position;
        Vector3 rightDirection = mouseWorldPosition - rightShoulder.position;

        // Рассчитываем углы поворота
        float leftAngle = Mathf.Atan2(leftDirection.y, leftDirection.x) * Mathf.Rad2Deg;
        float rightAngle = Mathf.Atan2(rightDirection.y, rightDirection.x) * Mathf.Rad2Deg;

        // Поворачиваем плечи
        leftShoulder.rotation = Quaternion.Euler(0, 0, leftAngle);
        rightShoulder.rotation = Quaternion.Euler(0, 0, rightAngle);

        // Поворачиваем руки
        leftArm.rotation = leftShoulder.rotation; // Рука следует за плечом
        rightArm.rotation = rightShoulder.rotation; // Рука следует за плечом
    }
}