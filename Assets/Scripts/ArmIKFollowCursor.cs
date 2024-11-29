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
    public float armSpeed = 4f;        // Скорость движения руки

    public Collider leftHandCollider; // Коллайдер левой руки
    public Collider rightHandCollider; // Коллайдер правой руки
    public int attackDamage = 10;       // Урон атаки

    private void Update()
    {
        // Получаем позицию курсора в мировых координатах
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = mainCamera.nearClipPlane;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

        // Управляем скоростью движения руки в зависимости от атаки
        float leftHandSpeed = Input.GetMouseButton(0) ? armSpeed : 0.2f;
        float rightHandSpeed = Input.GetMouseButton(1) ? armSpeed : 0.2f;

        // Обновляем позиции рук
        leftHandTarget.position = (worldPosition - leftShoulder.position).normalized * leftHandSpeed + leftShoulder.position;
        rightHandTarget.position = (worldPosition - rightShoulder.position).normalized * rightHandSpeed + rightShoulder.position;

        // Включаем или отключаем коллайдеры атаки
        leftHandCollider.enabled = Input.GetMouseButton(0);
        rightHandCollider.enabled = Input.GetMouseButton(1);
    }

    private void OnTriggerEnter(Collider collision)
    {
        // Обрабатываем столкновение с врагом
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
            }
        }
    }
}
