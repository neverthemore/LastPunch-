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

    private int _rotationCount = 0; // 0 - Back/ 1 - Left/ 2 - Front/ 3 - Right
    private bool _rotateLeft, _rotateRight, _rotateAllways = false;
    [SerializeField] private GameObject _buttonQ;
    [SerializeField] private GameObject _buttonE;


    void Start()
    {
        cc = GetComponent<CharacterController>();
        _buttonE.SetActive(false);
        _buttonQ.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");

        cc.Move(transform.forward * yInput * Time.deltaTime * speed + transform.right * xInput * Time.deltaTime * speed);
        if (Input.GetKeyDown(KeyCode.E) && (_rotateLeft|| _rotateAllways))
        {
            rotateY += 90;
            _rotationCount++;
            _rotateLeft = false;
            _buttonE.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Q) && (_rotateRight || _rotateAllways))
        {
            rotateY -= 90;
            _rotationCount--;
            _rotateRight = false;
            _buttonQ.SetActive(false);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rotation"))
        {
            if (_rotationCount % 2 == 0)
            {
                _rotateRight = true;
                _buttonQ.SetActive(true);
            }
            else
            { 
                _rotateLeft = true;
                _buttonE.SetActive(true);
            }                        
        }
        if (other.CompareTag("Free Rotation"))
            _rotateAllways = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Rotation"))
        {
            _buttonE.SetActive(false);
            _buttonQ.SetActive(false);
        }
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
