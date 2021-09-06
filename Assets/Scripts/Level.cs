using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Transform finish_point;
    public GameObject[] confetti;
    public int confetti_count;


    private void Update()
    {
        if (confetti_count > 0)
        {
            confetti_count--;
            confetti[confetti_count].SetActive(true);
        }
    }

    public void Finish()
    {
        confetti_count = confetti.Length;
    }
}
