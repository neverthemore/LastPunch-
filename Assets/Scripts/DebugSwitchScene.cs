using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugSwitchScene : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.I))
            SceneManager.LoadScene(sceneBuildIndex: 1);
        if (Input.GetKey(KeyCode.O))
            SceneManager.LoadScene(sceneBuildIndex: 2);
        if (Input.GetKey(KeyCode.P))
            SceneManager.LoadScene(sceneBuildIndex: 3);
    }
}
