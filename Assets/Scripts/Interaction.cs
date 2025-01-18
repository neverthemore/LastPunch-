using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private int _shawarmaHealthRegeneration = 20;
    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private string _tag;
    [SerializeField] private string _firstTag = string.Empty;
    [SerializeField] private bool _triggerStatus;
    [SerializeField] private GameObject _buttonF;

    [SerializeField] private GameObject _keyIcon;



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
        //if (Input.GetKeyUp(KeyCode.F) && _triggerStatus)
        //{
        //    if (_tag == "Shawarma" && _firstTag == _tag)
        //        ShawarmaLogic();
        //    else if(_tag == "Key" && _firstTag == _tag)
        //        KeyLogic();
        //}

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
    public void InteractionLogic(string tag, bool status)
    {
        _tag = tag;
        _triggerStatus = status;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _firstTag == "Shawarma")
            InteractionLogic("Shawarma", true);
        if (other.CompareTag("Player") && _firstTag == "Key")
            InteractionLogic("Key", true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && _firstTag == "Shawarma")
            InteractionLogic("Shawarma", false);
        if (other.CompareTag("Player") && _firstTag == "Key")
            InteractionLogic("Key", false);
    }
}
