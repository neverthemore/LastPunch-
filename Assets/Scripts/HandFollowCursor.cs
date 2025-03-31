using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HandFollowCursor : MonoBehaviour
{
    public CinemachineVirtualCamera cam;
    public Camera camBrain;
    Vector3 mousePosition;
    Vector3 mousePositionInWorld;
    
    public Transform head;

    [SerializeField] private Transform leftTarget;

    [SerializeField] private Transform rightTarget;

    public float rotationSpeed = 180f;
    [SerializeField]
    float angle;
    [SerializeField]
    float realAngle;

    public float maxAngle = 70;
    public float minAngle = -50;


    public float speed = 5f;
    Movement movement;

   

    void Start()
    {
       
        movement = GetComponent<Movement>();

        

    }

    void Update()
    {
        
       mousePosition = Input.mousePosition;
        mousePosition.z = Vector3.Distance(cam.transform.position, transform.position);
        mousePositionInWorld = camBrain.ScreenToWorldPoint(mousePosition);

      Vector3 dir = (mousePositionInWorld - transform.position).normalized;
        angle = Vector3.SignedAngle(transform.right, dir, transform.forward);

        leftTarget.transform.position = mousePositionInWorld;

        rightTarget.transform.position = mousePositionInWorld;






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

           
        

        if (angle > 90)
            movement.TurnLeft();
        else if (angle < 90 && angle > 0)
            movement.TurnRight();
        else if (angle < 0 && angle < -90)
            movement.TurnLeft();
        else if (angle > -90)
            movement.TurnRight();
    }

    private void LateUpdate()
    {
        float headangle = realAngle < 90 && realAngle > -90
            ? Mathf.Clamp(realAngle, -30, 30)
            : (realAngle > 0
                ? Mathf.Clamp(-realAngle + 180, -30, 30)
                : Mathf.Clamp(-realAngle - 180, -30, 30));

        // Заменяем transform.forward на Vector3.forward для локальной оси
        head.localRotation = Quaternion.AngleAxis(headangle, Vector3.forward);

        Debug.Log(realAngle);
    }

   
}