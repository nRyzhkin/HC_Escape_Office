using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoLayer : MonoBehaviour
{

    public Image levelProgressBar;
    public Text moneyText;
    public Animator anim;


    private PlayerController _playerController;
    private GameManager _gameManager;
    private int _money;
    // Start is called before the first frame update
    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _gameManager = FindObjectOfType<GameManager>();
        UpdateMoney(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(_money != _playerController.money)
        {
            if(_money > _playerController.money)
            {
                UpdateMoney(false);
            } else
            {
                UpdateMoney(true);
            }
        }

        if(_gameManager.current_level)
        {
            levelProgressBar.fillAmount = _playerController.transform.position.z / _gameManager.current_level.finish_point.position.z;
        }
    }

    void UpdateMoney(bool plus)
    {
        _money = _playerController.money;
        moneyText.text = "$" + _money;
        if(plus)
        {
            anim.Play("moneyTextPlus");
        } 
        else
        {
            anim.Play("moneyTextMinus");
        }
    }
}
