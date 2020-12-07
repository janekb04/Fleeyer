using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Missle : MonoBehaviour
{
    Rigidbody2D rb;
    Transform spaceship;

    public static float speed = 5;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up.normalized * speed;

        spaceship = GameManager.Get().Spaceship.transform;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == GameManager.Get().EnemyLayer)
        {
            collision.gameObject.GetComponent<Rock>().Damage(1);
            Explosion.Explode(gameObject, 0.15f);
        }
    }

    private void Update()
    {
        if (spaceship != null && Vector2.Distance(transform.position, spaceship.position) > GameManager.Get().RenderDistance)
            Destroy(gameObject);
    }
}
