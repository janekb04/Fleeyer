using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager gameManager = GameManager.Get();
        if (collision.collider.gameObject.layer == gameManager.PlayerLayer)
        {
            ++gameManager.Boosters;
            Destroy(gameObject);
        }
    }
}
