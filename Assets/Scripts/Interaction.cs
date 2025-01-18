using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private System.Random _rand = new();
    [SerializeField] private bool _triggerStatus;
    [SerializeField] private string _tag;
    [SerializeField] private string _firstTag = string.Empty;
    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private GameObject _buttonF;

    [SerializeField] private int _shawarmaHealthRegeneration = 20;

    [SerializeField] private GameObject _keyIcon;

    [SerializeField] private Sprite _trashCanOpen;
    [SerializeField] private GameObject _shawarma;
    [SerializeField] private GameObject _spawn;



    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
            _player = GameObject.FindGameObjectWithTag("Player");

        _playerHealth = _player.GetComponent<PlayerHealth>();
        _firstTag = gameObject.tag;
    }

    // Update is called once per frame
    void Update()
    {
        _buttonF.SetActive(_triggerStatus && _firstTag == _tag);
        if (_tag == "Shawarma")
        {
            if (Input.GetKeyUp(KeyCode.F) && _triggerStatus)
                ShawarmaLogic();
        }
        else if (_tag == "Key")
        {
            if (Input.GetKeyUp(KeyCode.F) && _triggerStatus)
                KeyLogic();
        }
        else if (_tag == "TrashCan")
        {
            if (Input.GetKeyUp(KeyCode.F) && _triggerStatus)
                TrashCanLogic();
        }
    }

    public void ShawarmaLogic()
    {
        _playerHealth.GetHealth(_shawarmaHealthRegeneration);
        Destroy(transform.parent.gameObject);
        //Destroy(gameObject);
    }
    public void KeyLogic()
    {
        _keyIcon.SetActive(true);
        Destroy(transform.parent.gameObject);
    }
    public void TrashCanLogic()
    {
        StartCoroutine(TrashCanShake());
    }
    public void InteractionLogic(string tag, bool status)
    {
        _tag = tag;
        _triggerStatus = status;
    }

    #region auxiliary scripts
    private IEnumerator TrashCanShake()
    {
        float shake = 1f;
        float time = 0.07f;
        transform.parent.gameObject.transform.rotation = Quaternion.Euler(-shake, shake, -shake);
        yield return new WaitForSeconds(time);
        transform.parent.gameObject.transform.rotation = Quaternion.Euler(shake, -shake, shake);
        yield return new WaitForSeconds(time);
        transform.parent.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        yield return new WaitForSeconds(time * 2);
        transform.parent.gameObject.GetComponent<SpriteRenderer>().sprite = _trashCanOpen;

        int choice = _rand.Next(0, 2);
        if (choice == 0)
            Instantiate(_shawarma, _spawn.transform.position, _spawn.transform.rotation);
        gameObject.SetActive(false);
        _buttonF.SetActive(false);
    }
    #endregion
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _firstTag == "Shawarma")
            InteractionLogic("Shawarma", true);
        if (other.CompareTag("Player") && _firstTag == "Key")
            InteractionLogic("Key", true);
        if (other.CompareTag("Player") && _firstTag == "TrashCan")
            InteractionLogic("TrashCan", true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && _firstTag == "Shawarma")
            InteractionLogic("Shawarma", false);
        if (other.CompareTag("Player") && _firstTag == "Key")
            InteractionLogic("Key", false);
        if (other.CompareTag("Player") && _firstTag == "TrashCan")
            InteractionLogic("TrashCan", false);
    }
}
