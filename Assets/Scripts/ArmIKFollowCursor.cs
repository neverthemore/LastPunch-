using UnityEngine;
using UnityEngine.U2D.IK;

public class ArmIKFollowCursor : MonoBehaviour
{
    public Transform leftHandTarget;   // Точка для слежения левой кисти
    public Transform rightHandTarget;  // Точка для слежения правой кисти
    public LimbSolver2D leftHandIK;    // IK для левой руки
    public LimbSolver2D rightHandIK;   // IK для правой руки
    public Transform leftHand;         // Спрайт или объект левой руки
    public Transform rightHand;        // Спрайт или объект правой руки
    public Transform leftShoulder;     // Положение плеча левой руки
    public Transform rightShoulder;    // Положение плеча правой руки
    public Camera mainCamera;          // Камера для конвертации позиции курсора

    private void Update()
    {
        // Получаем позицию курсора в мировых координатах
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = mainCamera.nearClipPlane;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

        // Сглаживаем переход к новой позиции
        leftHandTarget.position = Vector3.Lerp(leftHandTarget.position, worldPosition, Time.deltaTime * 10f);  // Используем плавное движение
        rightHandTarget.position = Vector3.Lerp(rightHandTarget.position, worldPosition, Time.deltaTime * 10f);

        // Если ЛКМ или ПКМ не нажаты, просто вращаем руки по направлению к курсору
        if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {
            RotateArmTowardsCursor(leftShoulder, leftHand, worldPosition);
            RotateArmTowardsCursor(rightShoulder, rightHand, worldPosition);

            leftHandIK.enabled = false;
            rightHandIK.enabled = false;
        }
        else
        {
            leftHandIK.enabled = Input.GetMouseButton(0);
            rightHandIK.enabled = Input.GetMouseButton(1);
        }
    }


    // Метод для поворота руки по направлению к курсору
    private void RotateArmTowardsCursor(Transform shoulder, Transform hand, Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - shoulder.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Сглаживание угла
        float angle = Mathf.LerpAngle(hand.eulerAngles.z, targetAngle, Time.deltaTime * 10f);
        hand.rotation = Quaternion.Euler(0, 0, angle);
    }
}