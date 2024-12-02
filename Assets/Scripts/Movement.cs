using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 10;
    public float rotationSpeed = 200;
    public Transform skin;
    public Transform cam;
    float xInput;
    float yInput;

    CharacterController cc;
    float rotateY = 0;
    float currentRotateY = 0;

    public Direction lastDir = Direction.Right;


    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");

        cc.Move(transform.forward * yInput * Time.deltaTime * speed + transform.right * xInput * Time.deltaTime * speed);
        if (Input.GetKeyDown(KeyCode.E))
        {
            rotateY += 90;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            rotateY -= 90;
        }
        currentRotateY = Mathf.Lerp(currentRotateY, rotateY, rotationSpeed * 0.001f);
        transform.rotation = Quaternion.Euler(0, currentRotateY, 0);
        Vector3 dir = (transform.position - cam.transform.position).normalized;
        dir.y = 0;


        if (lastDir == Direction.Right) skin.forward = dir;
        else skin.forward = -dir;

        //if(xInput < 0 && lastDir == Direction.Right)
        //{
        //    lastDir = Direction.Left;
        //}
        //else if(xInput > 0 && lastDir == Direction.Left)
        //{
        //    lastDir = Direction.Right;
        //}

    }

    public void TurnLeft()
    {
        lastDir = Direction.Left;
    }

    public void TurnRight()
    {
        lastDir = Direction.Right;
    }


}

public enum Direction
{
    Right, Left
}
