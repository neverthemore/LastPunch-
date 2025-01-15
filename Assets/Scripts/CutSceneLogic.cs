using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneLogic : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private bool _doorBroken = false;
    [SerializeField] private bool _statusTriggerDoor = false;
    [SerializeField] private GameObject _buttonF;
    [SerializeField] private GameObject _doorBrokenTrigger;

    void Start()
    {
        Debug.Log("CutScene scene. Press M to return to the main scene");
        _doorBrokenTrigger.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            SceneManager.LoadScene(sceneBuildIndex: 1);
        }
        if (!GameObject.FindGameObjectWithTag("Door") && !_doorBroken)
        {
            Debug.Log("Broken");
            _doorBroken = true;
            _doorBrokenTrigger.SetActive(true);
        }

        
        if (_statusTriggerDoor && Input.GetKey(KeyCode.F))
        {
            Debug.Log("Fade in. Return to main scene");
            SceneManager.LoadScene(sceneBuildIndex: 1);
        }
    }
    public void StatusBrokenTrigger(bool status)
    {
        _statusTriggerDoor = status;
        _buttonF.SetActive(status);
        Debug.Log(status);
    }
}
