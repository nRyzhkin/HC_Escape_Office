using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Current")]
    public int money;

    [Header("Options")]
    public float runSpeed = 1f;
    [Range(-1, 1)] public int runLine;
    public Animator anim;
    public LayerMask enemyLayer;
    public LayerMask lootLayer;
    public GameObject runEffect;
    public GameObject lootEffectPrefab;


    private GameManager _gameManager;
    public bool _isFightMode;
    private bool _isJump;
    private Vector3 _linePosition;
    private float _speed;
    private CamFollow _camFollow;
    private Vector3 _lastPosition;
    private RaycastHit _hit;
    private Enemy _activeEnemy;

    public Enemy GetEmeny()
    {
        return _activeEnemy;
    }

    public bool FightMode()
    {
        return _isFightMode;
    }
    // Start is called before the first frame update
    void Start()
    {
        _camFollow = FindObjectOfType<CamFollow>();
        _gameManager = FindObjectOfType<GameManager>();
        _speed = 0;
        runEffect.SetActive(false);
    }
    public void StartLevel()
    {

        anim.SetBool("death", false);
        anim.SetBool("run", true);
        transform.position = Vector3.zero;
        StartRun();
        _camFollow.StartNewLevel(_gameManager.level_distance);
    }

    public void JumpLeft()
    {
        if (_isJump || runLine <= -1) return;
        _isJump = true;
        runLine--;
        ChangeLine();
    }

    public void JumpRight()
    {
        if (_isJump || runLine >= 1) return;
        _isJump = true;
        runLine++;
        ChangeLine();
    }

    public void ChangeLine()
    {
        _isJump = false;
        _linePosition.x = runLine;
    }

    public void CheckEnemy()
    {
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out _hit, 1f + runSpeed / 50f, enemyLayer))
        {
            _activeEnemy = _hit.collider.GetComponent<Enemy>();
            _activeEnemy.StartFight();
            transform.position = _activeEnemy.transform.position - transform.forward * 0.5f;
            anim.transform.localPosition = Vector3.up * 0.2f;
            StartFight();
        }
    }


    public void CheckLoot()
    {
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out _hit, 1f + runSpeed / 50f, lootLayer))
        {
            _hit.collider.gameObject.SetActive(false);
            GameObject.Instantiate(lootEffectPrefab, _hit.transform.position, transform.rotation);
            money += 10;
        }
    }

    public void StartFight()
    {
        runEffect.SetActive(false);
        anim.transform.localRotation = Quaternion.identity;
        anim.SetBool("fight", true);
        _camFollow.lookMode = 1;
        _isFightMode = true;
        _speed = 0;
        _gameManager.StartFightMode();
    }

    public void EndFight(bool winValue, float ostTime, bool paid)
    {
        anim.SetBool("fight", false);
        if (paid)
        {
            _activeEnemy.EndFight(true, ostTime);
        }
        else
        {
            _activeEnemy.EndFight(!winValue, ostTime);
        }
        _isFightMode = false;
        if (winValue)
        {
            if(paid)
            {
                money -= _activeEnemy.moneyToLose;
            }
            Invoke(nameof(StartRun), ostTime - 0.2f);
        }
        else
        {
            _camFollow.lookMode = 2;
            EndLevel(false);
        }
    }

    void StartRun()
    {
        runEffect.SetActive(true);
        anim.transform.localPosition = Vector3.zero;
        anim.transform.localRotation = Quaternion.identity;
        _speed = runSpeed;
        _camFollow.lookMode = 0;
        Time.timeScale = 1f;
    }

    public void EndLevel(bool winValue)
    {
        anim.transform.localPosition = Vector3.zero;
        anim.SetBool("run", false);
        anim.SetBool("death", !winValue);

    }

    // Update is called once per frame
    void Update()
    {
        _lastPosition = transform.localPosition;
        transform.localPosition += transform.forward * Time.fixedDeltaTime * _speed;
        _linePosition.y = transform.localPosition.y;
        _linePosition.z = transform.localPosition.z;
        if (_speed > 0.1f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _linePosition, Time.deltaTime * runSpeed * 5f);
        }
        _camFollow.runVelocity = Mathf.Lerp(_camFollow.runVelocity, (_lastPosition.x - transform.localPosition.x) * _camFollow.rotationForce, Time.deltaTime * 5f);
        if (_isFightMode)
        {
            anim.transform.position = _activeEnemy.handTransform.position + _activeEnemy.handPlayerOffset;
        }
        else
        {
            CheckEnemy();
            CheckLoot();
            if(_gameManager.game_on)
            {
                if (transform.position.z >= _gameManager.current_level.finish_point.position.z)
                {
                    EndLevel(true);
                    _gameManager.EndLevel(true);
                }
            }
        }
    }
}
