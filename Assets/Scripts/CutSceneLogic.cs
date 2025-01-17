using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutSceneLogic : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private bool _statusTriggerDoor = false;
    [SerializeField] private bool _doorCheck = true;
    [SerializeField] private GameObject _buttonF;
    [SerializeField] private GameObject _brokenDoor;
    [SerializeField] private GameObject _buttonLCM;
    [SerializeField] private GameObject _doorHitbox;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _bam;
    [SerializeField] private GameObject _crack;
    [SerializeField] private GameObject _babah;
    [SerializeField] private Image _fadeImage;
    [SerializeField] private float _durationFadeTime = 3f;
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Movement _movement;
    [SerializeField] private int _typeScene;
    [SerializeField] private System.Random _rand = new();


    void Start()
    {
        Debug.Log("CutScene scene. Press M to return to the main scene");

        _doorHitbox.SetActive(false);
        _buttonF.SetActive(false);
        _buttonLCM.SetActive(false);
        StartCoroutine(FadeOut(_fadeImage, true));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.M))
            SceneManager.LoadScene(sceneBuildIndex: 1);

        if (_typeScene == 1)
        {
            if (_statusTriggerDoor && _doorCheck && Input.GetKey(KeyCode.F))
            {
                _doorCheck = false;
                _enemy.Shake();
                _doorHitbox.SetActive(true);
                _buttonF.SetActive(false);
                _buttonLCM.SetActive(true);
            }
            if (_statusTriggerDoor && !_doorCheck && _buttonLCM.activeSelf && Input.GetMouseButton(0))
            {
                _buttonLCM.SetActive(false);
            }
            if (!GameObject.FindGameObjectWithTag("Door") && _statusTriggerDoor)
            {
                _brokenDoor.SetActive(true);
                _crack.SetActive(false);
                _babah.SetActive(false);
                _buttonF.SetActive(true);
                if (Input.GetKey(KeyCode.F))
                {
                    _movement.StatusCutscene(true);
                    StartCoroutine(FadeOut(_fadeImage, false, true));
                }
            }
            if (!GameObject.FindGameObjectWithTag("Door") && !_statusTriggerDoor && !_doorCheck)
            {
                _buttonF.SetActive(false);
                _buttonLCM.SetActive(false);
            }
        }
    }
    public void StatusBrokenTrigger(bool status)
    {
        _statusTriggerDoor = status;
        Debug.Log(status);
    }
    private IEnumerator FadeOut(UnityEngine.UI.Image image, bool Out, bool end = false)
    {
        image.gameObject.SetActive(true);
        Color targetImage = image.color;

        float halfDuration = _durationFadeTime / 2;
        float time = 0f;
        float startAlpha;
        float endAlpha;

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
        if (end)
            yield return new WaitForSeconds(1);
        while (time < halfDuration)
        {
            time += Time.deltaTime;
            var newAlpha = Mathf.Lerp(startAlpha, endAlpha, time / halfDuration);
            image.color = new Color(targetImage.r, targetImage.g, targetImage.b, newAlpha);
            yield return null;
        }
        yield return new WaitForSeconds(1);
        _player.SetActive(Out);
        if (!GameObject.FindGameObjectWithTag("Door"))
            SceneManager.LoadScene(sceneBuildIndex: 1);
    }
    private IEnumerator LerpSceneMovement()
    {
        if (_typeScene == 1)
        {
            //_enemy.Shake();
            _bam.SetActive(true);
            yield return new WaitForSeconds(1);
            _buttonF.SetActive(true);
            _bam.SetActive(false);
            _movement.StatusCutscene(false);
        }
    }

    #region Input data, sceneOne
    public void SelectEffects(bool status)
    {
        if (status)
        {
            int select;
            select = _rand.Next(0, 2);
            if (select == 1)
                _crack.SetActive(true);
            else
                _babah.SetActive(true);
        }
        else
        {
            _crack.SetActive(false);
            _babah.SetActive(false);
        }
    }
    public void ReturnControl(int typeCutscene)
    {
        _typeScene = typeCutscene;
        StartCoroutine(LerpSceneMovement());

    }
    #endregion
}
