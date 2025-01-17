using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    public float speed = 10;
    public float rotationSpeed = 200;
    public Transform skin;
    public Transform cam;
    float xInput;
    float yInput;

    [SerializeField] CutSceneLogic cutSceneLogic;
    CharacterController cc;
    float rotateY = 0;
    float currentRotateY = 0;

    public Direction lastDir = Direction.Right;

    private int _rotationCount = 0; // 0 - Back/ 1 - Left/ 2 - Front/ 3 - Right
    private bool _rotateLeft, _rotateRight, _rotateAllways = false;
    private bool _cutscene = false;
    private bool _endScene = false;
    [SerializeField] private bool _sceneOne; // switch inspector only
    [SerializeField] private bool _sceneTwo; // switch inspector only

    [SerializeField] private GameObject _buttonQ; // иконка Q
    [SerializeField] private GameObject _buttonE; // иконка E

    [SerializeField] private GameObject[] _arrTriggerZone = new GameObject[5]; // массив триггер зон
    [SerializeField] private GameObject[] _arrWallsTypeOne = new GameObject[4];
    [SerializeField] private GameObject[] _arrWallsTypeTwo = new GameObject[4];

    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private int _currentWaypoint = 0;
    [SerializeField] private int _needWaypoint = 0;

    private PlayerHealth playerHealth;
    private Animator animator;
    void Start()
    {
        cc = GetComponent<CharacterController>();
        _buttonE.SetActive(false);
        _buttonQ.SetActive(false);
        SwitchMethod(_arrWallsTypeOne, false);
        animator = GetComponentInChildren<Animator>();
        playerHealth = GetComponent<PlayerHealth>();
        if (_arrWallsTypeOne.Length == 0 && _arrWallsTypeTwo.Length == 0 && _arrTriggerZone.Length == 0)
            _cutscene = true;
        if (_cutscene)
        {
            if (_sceneOne)
                _needWaypoint = 4;
            else if (_sceneTwo)
                _needWaypoint = 2;
        }
    }

    // Update is called once per frame
    private void FixedUpdate() //only cutscene logic
    {
        if (_cutscene)
        {
            if (_sceneOne)
            {
                if (_endScene)
                {
                    _sceneOne = false;
                    _needWaypoint = 5;
                }
            }
            animator.SetBool("Walk", true);
            if (_currentWaypoint != _needWaypoint)
            {
                if (transform.localPosition == _waypoints[_currentWaypoint].position)
                    _currentWaypoint++;
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, _waypoints[_currentWaypoint].position, speed * Time.fixedDeltaTime);
            }
            else
            {
                animator.SetBool("Walk", false);
                if (_sceneOne)
                    cutSceneLogic.ReturnControl();
            }
        }
    }
    void Update()
    {
        if (!_cutscene)
        {
            if (playerHealth != null && !playerHealth.isStunned)
            {

                xInput = Input.GetAxis("Horizontal");
                yInput = Input.GetAxis("Vertical");

            }
            if (xInput != 0 || yInput != 0)
            {
                animator.SetBool("Walk", true);
            }
            else
            {
                animator.SetBool("Walk", false);
            }
        }

        cc.Move(transform.forward * yInput * Time.deltaTime * speed + transform.right * xInput * Time.deltaTime * speed);

        if (Input.GetKeyDown(KeyCode.Q) && (_rotateLeft || _rotateAllways))
        {
            rotateY += 90;
            _rotationCount++;
            _rotateLeft = false;
            _buttonQ.SetActive(false);

        }
        if (Input.GetKeyDown(KeyCode.E) && (_rotateRight || _rotateAllways))
        {
            rotateY -= 90;
            _rotationCount--;
            _rotateRight = false;
            _buttonE.SetActive(false);
        }

        if (_rotationCount % 2 == 0)
        {
            SwitchMethod(_arrWallsTypeTwo, true);
            SwitchMethod(_arrWallsTypeOne, false);
        }
        else
        {
            SwitchMethod(_arrWallsTypeTwo, false);
            SwitchMethod(_arrWallsTypeOne, true);
        }

        currentRotateY = Mathf.Lerp(currentRotateY, rotateY, rotationSpeed * 0.001f);
        transform.rotation = Quaternion.Euler(0, currentRotateY, 0);
        Vector3 dir = (transform.position - cam.transform.position).normalized;
        dir.y = 0;


        if (lastDir == Direction.Right) skin.forward = dir;
        else skin.forward = -dir;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 6;
            animator.SetBool("Run", true);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 3;
            animator.SetBool("Run", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Broken Door"))
            cutSceneLogic.StatusBrokenTrigger(true);
        if (other.CompareTag("Rotation") && !_rotateAllways)
        {
            if (other.gameObject == _arrTriggerZone[_rotationCount])
            {
                _rotateLeft = true;
                _buttonQ.SetActive(true);
            }
            else
            {
                _rotateRight = true;
                _buttonE.SetActive(true);
            }
        }
        if (other.CompareTag("Free Rotation"))
            _rotateAllways = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Broken Door"))
            cutSceneLogic.StatusBrokenTrigger(false);
        if (other.CompareTag("Rotation"))
        {
            _buttonE.SetActive(false);
            _buttonQ.SetActive(false);
            _rotateLeft = false;
            _rotateRight = false;
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

    private void SwitchMethod(GameObject[] arrWalls, bool turnOn)
    {
        for (int i = 0; i < arrWalls.Length; i++)
        {
            arrWalls[i].SetActive(turnOn);
        }
    }

    #region Cutscene Logic

    public void StatusCutscene(bool status)
    {
        _cutscene = status;
        if ((_sceneOne || _sceneTwo) && status)
            _endScene = true;
    }
    #endregion
}

public enum Direction
{
    Right, Left
}
