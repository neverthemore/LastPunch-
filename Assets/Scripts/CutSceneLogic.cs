using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutSceneLogic : MonoBehaviour
{
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

    [SerializeField] private int _idText = 0;
    [SerializeField] private System.Random _rand = new();
    [SerializeField] private bool _textShow = false;

    [SerializeField] private GameObject _enemyOne;
    [SerializeField] private GameObject _enemyTwo;

    [SerializeField] private GameObject _textObjectLeft;
    [SerializeField] private GameObject _textObjectRight;

    [SerializeField] private SpriteRenderer _faceObjectLeft;
    [SerializeField] private SpriteRenderer _faceObjectRight;

    [SerializeField] private TMP_Text _textLeft;
    [SerializeField] private TMP_Text _textRight;

    [SerializeField] private Sprite _faceHero;
    [SerializeField] private Sprite _faceOne;
    [SerializeField] private Sprite _faceTwo;
    [SerializeField] private Sprite _faceMather;

    [SerializeField] private Animator _enemyOneAnimator;
    [SerializeField] private Animator _enemyTwoAnimator;


    void Start()
    {
        StartCoroutine(FadeOut(_fadeImage, true));
        if (GameObject.FindGameObjectWithTag("Door"))
        {
            _typeScene = 1;
        }
        if (GameObject.FindGameObjectWithTag("Enemy"))
        {
            _typeScene = 2;

            _enemyOneAnimator.enabled = false;
            _enemyTwoAnimator.enabled = false;

            if (_enemyOne != null)
                _enemy = _enemyOne.GetComponent<Enemy>();
            else if (_enemyTwo != null)
                _enemy = _enemyTwo.GetComponent<Enemy>();
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.M))
            SceneManager.LoadScene(sceneBuildIndex: 2);

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
        else if (_typeScene == 2)
        {
            if (_idText < 6)
            {
                if (_textShow && Input.GetMouseButtonUp(0))
                {
                    _idText++;
                    _textShow = false;
                    TextData();
                }
            }

            //dialog method ebat, (iddialoga)
            //id++ esli press F
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
        if (!GameObject.FindGameObjectWithTag("Door") && _typeScene == 1)
            SceneManager.LoadScene(sceneBuildIndex: 1);
        if (_typeScene == 2)
        {
            _player.SetActive(false);
            TextData();
        }
        //_text.SetActive(true);
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
        else if (_typeScene == 2)
        {
            yield return null;
            _movement.StatusCutscene(false);
        }
    }

    private void TextData()
    {
        _textObjectLeft.SetActive(false);
        _textObjectRight.SetActive(false);

        switch (_idText)
        {
            case 0:
                {
                    _textShow = true;
                    _textObjectRight.SetActive(true);
                    _textRight.text = "Где ключи?! Говори!";
                    _faceObjectRight.sprite = _faceTwo;
                    break;
                }
            case 1:
                {
                    _textShow = true;
                    _textObjectRight.SetActive(true);
                    _textRight.text = "Ты че? Их уже забрали.";
                    _faceObjectRight.sprite = _faceOne;
                    break;
                }
            case 2:
                {
                    _textShow = true;
                    _textObjectLeft.SetActive(true);
                    _textLeft.text = "Прошу вас, смилуйтесь...";
                    _faceObjectLeft.sprite = _faceMather;
                    _player.SetActive(true);
                    break;
                }
            case 3:
                {
                    _textShow = true;
                    _textObjectRight.SetActive(true);
                    _textRight.text = "Опа. Квартирку не перепутал, месье?";
                    _faceObjectRight.sprite = _faceTwo;
                    break;
                }
            case 4:
                {
                    _textShow = true;
                    _textObjectLeft.SetActive(true);
                    _textLeft.text = "Сынок!";
                    _faceObjectLeft.sprite = _faceMather;
                    break;
                }
            case 5:
                {
                    _textShow = true;
                    _textObjectLeft.SetActive(true);
                    _textLeft.text = "Я че-то не понял. Ща каждому накатаю.";
                    _faceObjectLeft.sprite = _faceMather;
                    break;
                }
            case 6:
                {
                    _textShow = true;
                    _textObjectRight.SetActive(true);
                    _textRight.text = "Где ключи?! Говори!";
                    _faceObjectRight.sprite = _faceTwo;
                    break;
                }
            case 7:
                {
                    _textShow = true;
                    _textObjectRight.SetActive(true);
                    _textRight.text = "Где ключи?! Говори!";
                    _faceObjectRight.sprite = _faceTwo;
                    break;
                }

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
    public void ReturnControl()
    {
        StartCoroutine(LerpSceneMovement());
    }
    #endregion
}
