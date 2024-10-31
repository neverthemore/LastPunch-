using UnityEngine;
using UnityEngine.U2D.IK;

public class ArmIKFollowCursor : MonoBehaviour
{
    public Transform leftHandTarget;   // Точка для слежения левой кисти
    public Transform rightHandTarget;  // Точка для слежения правой кисти
    public LimbSolver2D leftHandIK;    // IK для левой руки
    public LimbSolver2D rightHandIK;   // IK для правой руки
    public Camera mainCamera;          // Камера для конвертации позиции курсора

    private void Update()
    {
        // Получаем позицию курсора в мировых координатах
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = mainCamera.nearClipPlane;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

        // Обновляем позиции целей для IK
        leftHandTarget.position = worldPosition;
        rightHandTarget.position = worldPosition;
    }
}
