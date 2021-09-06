using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Animator anim;
    public Transform handTransform;
    public Vector3 handPlayerOffset;
    public int moneyToLose;

    private bool _winFight;
    private BoxCollider _boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    public void StartFight()
    {
        _boxCollider.enabled = false;
        anim.SetBool("fight", true);
    }

    public void EndFight(bool win, float ostTime)
    {
        _winFight = win;
        Invoke(nameof(AnimateEndFight), ostTime - 0.6f);
    }

    void AnimateEndFight()
    {
        anim.SetBool("dead", !_winFight);
        anim.SetBool("dance", _winFight);
    }
}
