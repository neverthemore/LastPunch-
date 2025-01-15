using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutSceneLogic : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private bool _doorBroken = false;
    [SerializeField] private bool _lcmNote = true;
    [SerializeField] private bool _statusTriggerDoor = false;
    [SerializeField] private GameObject _buttonF;
    [SerializeField] private GameObject _brokenDoor;
    [SerializeField] private GameObject _buttonLCM;
    [SerializeField] private GameObject _doorBrokenTrigger;
    [SerializeField] private GameObject _player;
    [SerializeField] private Image _fadeImage;
    [SerializeField] private float _durationFadeTime = 3f;


    void Start()
    {
        Debug.Log("CutScene scene. Press M to return to the main scene");
        _doorBrokenTrigger.SetActive(true);
        _brokenDoor.SetActive(false);
        _buttonF.SetActive(false);
        _buttonLCM.SetActive(false);
        StartCoroutine(FadeOut(_fadeImage, true));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.N))
            StartCoroutine(FadeOut(_fadeImage, false));

        if (Input.GetKey(KeyCode.B))
            StartCoroutine(FadeOut(_fadeImage, true));

        if (Input.GetKey(KeyCode.M))
        {
            SceneManager.LoadScene(sceneBuildIndex: 1);
        }
        if (!GameObject.FindGameObjectWithTag("Door") && !_doorBroken)
        {
            Debug.Log("Broken");
            _brokenDoor.SetActive(true);
            _doorBroken = true;
        }

        _buttonF.SetActive(_statusTriggerDoor && _doorBroken);
        _buttonLCM.SetActive(_statusTriggerDoor && !_doorBroken && _lcmNote);
        if (Input.GetMouseButton(0) && _buttonLCM.activeSelf)
            _lcmNote = false;

        if (_statusTriggerDoor && Input.GetKey(KeyCode.F))
        {
            Debug.Log("Fade in. Return to main scene");
            SceneManager.LoadScene(sceneBuildIndex: 1);
        }
    }
    public void StatusBrokenTrigger(bool status)
    {
        _statusTriggerDoor = status;
        Debug.Log(status);
    }
    private IEnumerator FadeOut(UnityEngine.UI.Image image, bool Out)
    {
        image.gameObject.SetActive(true);
        Color targetImage = image.color;

        float halfDuration = _durationFadeTime / 2;
        float time = 0f;
        float startAlpha = 0f;
        float endAlpha = 0f;

        if (Out)
        {
            startAlpha = 1f;
            endAlpha = 0f;
        }
        else
        {
            startAlpha = 0f;
            endAlpha = 1f;
        }

        while (time < halfDuration)
        {
            time += Time.deltaTime;
            var newAlpha = Mathf.Lerp(startAlpha, endAlpha, time / halfDuration);
            image.color = new Color(targetImage.r, targetImage.g, targetImage.b, newAlpha);
            yield return null;
        }
        yield return new WaitForSeconds(1);
        _player.SetActive(Out);
    }
}
