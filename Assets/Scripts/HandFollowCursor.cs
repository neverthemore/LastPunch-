using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandFollowCursor : MonoBehaviour
{
    public CinemachineVirtualCamera cam;
    public Camera camBrain;
    Vector3 mousePosition;
    Vector3 mousePositionInWorld;
    public LayerMask enemyLayer;
    public Transform handLeft;
    public Transform handRight;

    [SerializeField]
    float handLeftDistanceValue;
    float handLeftDistance;
    float currentHandLeftDistance;
    float targetHandLeftDistance;

    [SerializeField]
    float handRightDistanceValue;
    float handRightDistance;
    float currentHandRightDistance;
    float targetHandRightDistance;
    public float hitLeftDistanceFactor = 1.5f;
    public float hitRightDistanceFactor = 1.8f;

    [SerializeField]
    float angle;
    [SerializeField]
    float realAngle;

    public float maxAngle = 70;
    public float minAngle = -50;
    public float hitSpeed = 100;

    public Collider leftHandCollider;
    public Collider rightHandCollider;

    Movement movement;

    // Исходные позиции рук относительно тела
    private Vector3 defaultLeftHandPosition;
    private Vector3 defaultRightHandPosition;

    void Start()
    {
        movement = GetComponent<Movement>();

        handLeftDistance = Vector3.Distance(transform.position, handLeft.position);
        currentHandLeftDistance = handLeftDistance;
        targetHandLeftDistance = handLeftDistance;
        handLeftDistance = handLeftDistanceValue;

        handRightDistance = Vector3.Distance(transform.position, handRight.position);
        currentHandRightDistance = handRightDistance;
        targetHandRightDistance = handRightDistance;
        handRightDistance = handRightDistanceValue;
        // Сохраняем исходные позиции рук
        defaultLeftHandPosition = handLeft.localPosition;
        defaultRightHandPosition = handRight.localPosition;
    }

    void Update()
    {
        // Получаем позицию мыши в мировых координатах
        mousePosition = Input.mousePosition;
        mousePosition.z = Vector3.Distance(cam.transform.position, transform.position);
        mousePositionInWorld = camBrain.ScreenToWorldPoint(mousePosition);

        // Вычисляем угол поворота
        Vector3 dir = (mousePositionInWorld - transform.position).normalized;
        angle = Vector3.SignedAngle(transform.right, dir, transform.forward);
        if (movement.lastDir == Direction.Right)
        {
            realAngle = Mathf.Clamp(angle, minAngle, maxAngle);
        }
        else
        {
            if (angle > 0)
                realAngle = Mathf.Clamp(angle, 180 - maxAngle, 180);
            else
                realAngle = Mathf.Clamp(angle, -180, -180 - minAngle);
        }

        // Если нажата левая кнопка мыши
        if (Input.GetMouseButtonDown(0))
        {
            targetHandLeftDistance = handLeftDistance * hitLeftDistanceFactor;
            leftHandCollider.enabled = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            targetHandLeftDistance = handLeftDistance;
            leftHandCollider.enabled = false;
        }

        // Если нажата правая кнопка мыши
        if (Input.GetMouseButtonDown(1))
        {
            targetHandRightDistance = handRightDistance * hitRightDistanceFactor;
            rightHandCollider.enabled = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            targetHandRightDistance = handRightDistance;
            rightHandCollider.enabled = false;
        }

        // Двигаем руки только если кнопки нажаты
        if (Input.GetMouseButton(0))
        {
            currentHandLeftDistance = Mathf.MoveTowards(currentHandLeftDistance, targetHandLeftDistance, hitSpeed * Time.deltaTime);
            handLeft.position = transform.position + Quaternion.AngleAxis(realAngle, transform.forward) * transform.right * currentHandLeftDistance;
        }
        else
        {
            handLeft.localPosition = defaultLeftHandPosition; // Возвращаем руку в исходное положение
        }

        if (Input.GetMouseButton(1))
        {
            currentHandRightDistance = Mathf.MoveTowards(currentHandRightDistance, targetHandRightDistance, hitSpeed * Time.deltaTime);
            handRight.position = transform.position + Quaternion.AngleAxis(realAngle, transform.forward) * transform.right * currentHandRightDistance;
        }
        else
        {
            handRight.localPosition = defaultRightHandPosition; // Возвращаем руку в исходное положение
        }

        // Поворот персонажа в зависимости от угла
        if (angle > 90)
            movement.TurnLeft();
        else if (angle < 90 && angle > 0)
            movement.TurnRight();
        else if (angle < 0 && angle < -90)
            movement.TurnLeft();
        else if (angle > -90)
            movement.TurnRight();
    }
}

