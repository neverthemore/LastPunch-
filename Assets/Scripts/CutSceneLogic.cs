using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneLogic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("CutScene scene. Press M to return to the main scene");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.M))
        {            
            SceneManager.LoadScene(sceneBuildIndex:1);
        }
    }
}
