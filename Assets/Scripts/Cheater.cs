using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheater : MonoBehaviour
{
    static Cheater instance = null;

    private void Start()
    {
        if (instance != null)
            Destroy(this);
        else
            instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            ++GameManager.Get().Boosters;
        if (Input.GetKey(KeyCode.C))
        {
            Debug.Log("Money, money, money!");
            GameManager.Get().Coins += 100000;
        }
    }
}
