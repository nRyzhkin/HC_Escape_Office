
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TouchControlsKit;

public class GameManager : MonoBehaviour
{
    [Header("Levels")]
    public bool game_on;
    public float level_distance;
    public bool non_level_run;
    public int level_counter;
    public Level current_level;
    [SerializeField] public GameObject[] level_prefabs;

    [Header("Options")]
    public float swipeSensetive = 10f;
    public float fightTimeLose = 0.7f;
    public AnimationCurve fightTimeKick;
    public AnimationCurve fightTimeMoney;
    [Header("Links")]
    public GameObject mainMenuLayer;
    public GameObject fightModeLayer;
    public GameObject infoLayer;
    public Button fightModeMoneyButton;
    public Button fightModeKickButton;
    public RectTransform fightModeMoneyRect;
    public RectTransform fightModeKickRect;
    public List<Details> details;

    private float _fightTime;
    private bool _jumped;
    private PlayerController _playerController;
    private Vector2 _inputAxis;
    private Vector2 _evaluateKick;
    private Vector2 _evaluateMoney;
    // Start is called before the first frame update
    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
    }

    void SetMenuLayer()
    {
        mainMenuLayer.SetActive(true);
    }

    public void StartLevel()
    {
        game_on = true;

        level_distance = _playerController.transform.position.z;
        _playerController.StartLevel();
        infoLayer.SetActive(true);
        non_level_run = false;
        if (current_level) Destroy(current_level.gameObject);
        current_level = GameObject.Instantiate(level_prefabs[level_counter], Vector3.zero, Quaternion.identity).GetComponent<Level>();

        for (int i = 0; i < details.Count; i++)
        {
            details[i].ReturnToStart(level_distance);
        }
    }

    public void EndLevel(bool win)
    {
        game_on = false;
        if (win)
        {
            non_level_run = true;
            level_counter++;
            current_level.Finish();
            Invoke(nameof(StartLevel), 2f);

        } else
        {
            Invoke(nameof(SetMenuLayer), 3f);
            _playerController.money = 0;
        }
    }

    public void StartFightMode()
    {
        Time.timeScale = 0.03f;
        fightModeLayer.SetActive(true);
        _fightTime = 1f;
        if (_playerController.money >= _playerController.GetEmeny().moneyToLose)
        {
            fightModeMoneyButton.interactable = true;
        }
        else
        {
            fightModeMoneyButton.interactable = false;
        }
    }

    public void FightLose()
    {
        _playerController.EndFight(false, _fightTime, false);
        Time.timeScale = 1f;
        fightModeLayer.SetActive(false);
        EndLevel(false);
    }

    public void FightKick()
    {
        fightModeLayer.SetActive(false);
        _playerController.EndFight(true, _fightTime, false);
        Time.timeScale = 1f;
    }

    public void FightGiveMoney()
    {
        fightModeLayer.SetActive(false);
        _playerController.EndFight(true, _fightTime, true);
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        _inputAxis = TCKInput.GetAxis("Touchpad");

        _fightTime -= Time.deltaTime;
        if (_playerController.FightMode())
        {
            _evaluateKick.x = fightTimeKick.Evaluate(_fightTime) * 800f;
            fightModeKickRect.anchoredPosition = _evaluateKick;


            _evaluateMoney.x = fightTimeMoney.Evaluate(_fightTime) * -800f;
            fightModeMoneyRect.anchoredPosition = _evaluateMoney;

            if (_fightTime < fightTimeLose)
            {
                FightLose();
            }
        }

        if (_jumped) return;
        if (_inputAxis.x >= swipeSensetive)
        {
            _jumped = true;
            _playerController.JumpRight();
        }

        if (_inputAxis.x <= -swipeSensetive)
        {
            _jumped = true;
            _playerController.JumpLeft();
        }
    }

    public void StartDragTouchpad()
    {
        _jumped = false;
    }

}
