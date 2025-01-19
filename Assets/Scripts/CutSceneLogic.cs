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
    [SerializeField] private GameObject _wallLeft;
    [SerializeField] private Image _fadeImage;
    [SerializeField] private float _durationFadeTime = 3f;
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Movement _movement;

    [SerializeField] private int _typeScene;

    [SerializeField] private int _idText = 0;
    [SerializeField] private System.Random _rand = new();
    [SerializeField] private bool _textShow = false;
    [SerializeField] private bool _textAccepted = false;
    [SerializeField] private bool _aftherFight = false;
    [SerializeField] private bool _statusTriggerMather = false;
    [SerializeField] private bool _endScene = false;
    [SerializeField] private bool _endConf = false;

    [SerializeField] private GameObject _enemyOne;
    [SerializeField] private GameObject _enemyTwo;

    [SerializeField] private GameObject _matherTrigger;

    [SerializeField] private GameObject _textLCM;

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
        else if (GameObject.FindGameObjectWithTag("Enemy"))
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
                _wallLeft.SetActive(true);
                _brokenDoor.SetActive(true);
                _crack.SetActive(false);
                _babah.SetActive(false);
                _buttonF.SetActive(true);
                if (Input.GetKey(KeyCode.F))
                {
                    _movement.TurnOffAnimatoin();
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
            if(!GameObject.FindGameObjectWithTag("Player") && _idText == 6)
                SceneManager.LoadScene(sceneBuildIndex: 2);
            if (_endScene && !_endConf)
            {
                _endConf = true;
                StartCoroutine(FadeOut(_fadeImage, false, true));
            }
            if (!GameObject.FindGameObjectWithTag("Enemy") && _statusTriggerMather && _matherTrigger.activeSelf)
            {
                _buttonF.SetActive(true);
                if (Input.GetKey(KeyCode.F))
                {
                    _player.GetComponent<HandFollowCursor>().enabled = false;
                    _movement.TurnOffAnimatoin();
                    _matherTrigger.SetActive(false);
                    _aftherFight = false;
                    _textAccepted = true;
                    TextData();
                }
            }
            else
                _buttonF.SetActive(false);
            if (!GameObject.FindGameObjectWithTag("Enemy") && !_textAccepted && _idText == 6)
            {
                Debug.Log("321");
                _aftherFight = true;
                _textShow = false;
                _idText++;
                _matherTrigger.SetActive(true);
            }

            if (_idText != 2 && _idText != 6 && _idText != 9)
            {
                if (_textShow && Input.GetMouseButtonUp(0) && !_aftherFight)
                {
                    _idText++;
                    _textShow = false;
                    TextData();
                }
            }
            else if (_idText == 2 && _textAccepted)
            {
                _textAccepted = false;
                if (_textLCM.activeSelf)
                    _textLCM.SetActive(false);
                _player.SetActive(true);
            }
            else if (_idText == 6 && _textAccepted)
            {
                StartCoroutine(BeforeFight());
            }
            else if (_idText == 9 && _textAccepted)
            {                
                StartCoroutine(EndScene());
            }
        }
    }
    public void StatusBrokenTrigger(bool status)
    {
        _statusTriggerDoor = status;
        Debug.Log(status);
    }
    public void StatusMatherTrigger(bool status)
    {
        _statusTriggerMather = status;
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
        if (_typeScene != 2)
            _player.SetActive(Out);
        if (!GameObject.FindGameObjectWithTag("Door") && _typeScene == 1)
            SceneManager.LoadScene(sceneBuildIndex: 2);
        if (_typeScene == 2)
        {
            if (_idText != 9)
            {
                _textAccepted = true;
                TextData();
            }
            else
            {
                SceneManager.LoadScene(sceneBuildIndex: 3);
            }
        }
    }
    private IEnumerator BeforeFight()
    {
        if (_idText == 6)
        {
            yield return new WaitForSeconds(2);
            _player.GetComponent<HandFollowCursor>().enabled = true;
            _player.GetComponent<Movement>().enabled = true;
            _enemyOneAnimator.enabled = true;
            _enemyTwoAnimator.enabled = true;
            _enemyOne.GetComponent<Enemy>().enabled = true;
            _enemyTwo.GetComponent<Enemy>().enabled = true;
        }
        _textLCM.SetActive(false);
        _textObjectLeft.SetActive(false);
        _textObjectRight.SetActive(false);
        _textAccepted = false;
    }
    private IEnumerator EndScene()
    {
        _endScene = true;
        yield return new WaitForSeconds(0.25f);
        _player.GetComponent<Movement>().enabled = true;
        _movement.StatusCutscene(true);
        yield return new WaitForSeconds(1);
        _textLCM.SetActive(false);
        _textObjectLeft.SetActive(false);
        _textObjectRight.SetActive(false);
        _textAccepted = false;
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
            yield return new WaitForSeconds(1);
            _movement.StatusCutscene(false);
            if (!_textAccepted)
                _idText++;
            _textAccepted = true;
            _textLCM.SetActive(true);
            _player.GetComponent<Movement>().enabled = false;
        }
    }

    private void TextData()
    {
        _textObjectLeft.SetActive(false);
        _textObjectRight.SetActive(false);

        if (_textAccepted)
        {
            switch (_idText)
            {
                case 0:
                    {
                        _textShow = true;
                        _textObjectRight.SetActive(true);
                        _textLCM.SetActive(true);
                        _textRight.text = "Где ключи?! Говори!";
                        _faceObjectRight.sprite = _faceTwo;
                        break;
                    }
                case 1:
                    {
                        _textShow = true;
                        _textObjectRight.SetActive(true);
                        _textLCM.SetActive(true);
                        _textRight.text = "Ты че? Их уже забрали.";
                        _faceObjectRight.sprite = _faceOne;
                        break;
                    }
                case 2:
                    {
                        _textShow = true;
                        _textObjectLeft.SetActive(true);
                        _textLCM.SetActive(true);
                        _textLeft.text = "Прошу вас, смилуйтесь...";
                        _faceObjectLeft.sprite = _faceMather;
                        break;
                    }
                case 4:
                    {
                        _textShow = true;
                        _textObjectRight.SetActive(true);
                        _textLCM.SetActive(true);
                        _textRight.text = "Опа. Квартирку не перепутал, месье?";
                        _faceObjectRight.sprite = _faceTwo;
                        break;
                    }
                case 5:
                    {
                        _textShow = true;
                        _textObjectLeft.SetActive(true);
                        _textLCM.SetActive(true);
                        _textLeft.text = "Сынок!";
                        _faceObjectLeft.sprite = _faceMather;
                        break;
                    }
                case 6:
                    {
                        _textShow = true;
                        _textObjectLeft.SetActive(true);
                        _textLCM.SetActive(false);
                        _textLeft.text = "Я че-то не понял. Ща каждому накатаю.";
                        _faceObjectLeft.sprite = _faceHero;                        
                        break;
                    }
                case 7:
                    {
                        _textShow = true;
                        _textObjectLeft.SetActive(true);
                        _textLCM.SetActive(true);
                        _textLeft.text = "Мам! Что они с тобой сделали?";
                        _faceObjectLeft.sprite = _faceHero;
                        _player.GetComponent<Movement>().enabled = false;
                        break;
                    }
                case 8:
                    {
                        _textShow = true;
                        _textObjectLeft.SetActive(true);
                        _textLCM.SetActive(true);
                        _textLeft.text = "Забрали тачку бати! Царство ему небесное";
                        _faceObjectLeft.sprite = _faceMather;
                        break;
                    }
                case 9:
                    {
                        _textShow = true;
                        _textObjectLeft.SetActive(true);
                        _textLCM.SetActive(false);
                        _textLeft.text = "Мой бимер?! Скоро буду";
                        _faceObjectLeft.sprite = _faceHero;
                        break;
                    }
                default:
                    {
                        break;
                    }

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
