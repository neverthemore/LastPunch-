using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private int _shawarmaHealthRegeneration = 20;
    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private string _tag;
    [SerializeField] private bool _triggerStatus;
    [SerializeField] private GameObject _buttonF;



    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
            _player = GameObject.FindGameObjectWithTag("Player");

        _playerHealth = _player.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        _buttonF.SetActive(_triggerStatus);
        if (Input.GetKeyUp(KeyCode.F) && _triggerStatus)
        {
            if (_tag == "Shawarma")
                ShawarmaLogic();
        }
    }

    public void ShawarmaLogic()
    {
        _playerHealth.GetHealth(_shawarmaHealthRegeneration);
        Destroy(transform.parent.gameObject);
        //Destroy(gameObject);
    }
    public void InteractionLogic(string tag, bool status)
    {
        _tag = tag;
        _triggerStatus = status;
    }
}
