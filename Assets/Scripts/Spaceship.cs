using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    [SerializeField]
    GameObject missle;
    [SerializeField]
    Transform launcher;
    [SerializeField]
    float missleSpeed;
    [SerializeField]
    float speed, angularSpeed;
    [SerializeField]
    GameObject shield;
    [SerializeField]
    float shieldLenght;
    [SerializeField]
    LineRenderer leftLaser, rightLaser;
    [SerializeField]
    float laserLenght, laserRadius;
    [SerializeField]
    GameObject hull, turbo;
    [SerializeField]
    float turboLenght;

    GameManager game;
    Rigidbody2D rb;
    Coroutine shieldCoroutine;
    bool inFTL = false;

    void Start()
    {
        Missle.speed = missleSpeed;
        game = GameManager.Get();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (CrossPlatformInput.Left)
            rb.MoveRotation(rb.rotation + angularSpeed * Time.deltaTime);
        if (CrossPlatformInput.Right)
            rb.MoveRotation(rb.rotation - angularSpeed * Time.deltaTime);
        if (CrossPlatformInput.Shoot && !inFTL)
            Instantiate(missle, (Vector2)launcher.position, transform.rotation);
        if (CrossPlatformInput.Lasers && game.Boosters >= game.LaserCost)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(rb.position, transform.up, 10.0f, 1 << game.EnemyLayer);
            if (hitInfo.collider != null)
            {
                game.Boosters -= game.LaserCost;

                StartCoroutine(Lasers(hitInfo.collider.gameObject));
            }
        }
        if (CrossPlatformInput.Shield && game.Boosters >= game.ShieldCost)
        {
            if (shieldCoroutine != null)
                StopCoroutine(shieldCoroutine);
            game.Boosters -= game.ShieldCost;
            shieldCoroutine = StartCoroutine(Shield());
        }
        if (CrossPlatformInput.Turbo && game.Boosters >= game.TurboCost)
        {
            if (!inFTL)
            {
                game.Boosters -= game.TurboCost;
                inFTL = true;
                StartCoroutine(Turbo());
            }
        }

        rb.MovePosition(transform.position +  transform.up * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == GameManager.Get().EnemyLayer)
        {
            Explosion.Explode(gameObject);
            GameManager.Get().EndGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == GameManager.Get().EnemyLayer)
        {
            collider.gameObject.GetComponent<Rock>().Die();
        }
    }

    IEnumerator Shield()
    {
        Debug.Log("Shields up!");
        shield.SetActive(true);
        float timeLeft = shieldLenght;
        while (timeLeft > 0)
        {
            GameManager.Get().UIManager.ShieldIconFill = Mathf.InverseLerp(0, shieldLenght, timeLeft);
            timeLeft -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        shield.SetActive(false);
    }

    IEnumerator Lasers(GameObject target)
    {
        Debug.Log("Lasers!");
        leftLaser.enabled = true;
        rightLaser.enabled = true;

        Collider2D[] nearby = Physics2D.OverlapCircleAll(target.transform.position, laserRadius, 1 << GameManager.Get().EnemyLayer);
        LineRenderer[] lines = new LineRenderer[nearby.Length];
        for (int i = 0; i < lines.Length; ++i)
        {
            lines[i] = nearby[i].gameObject.AddComponent<LineRenderer>();
            lines[i].transform.parent = transform;
            lines[i].gameObject.layer = 0;
            lines[i].widthCurve = leftLaser.widthCurve;
            lines[i].colorGradient = leftLaser.colorGradient;
            lines[i].sharedMaterial = leftLaser.sharedMaterial;
        }
        target.layer = 0;

        float timeLeft = laserLenght;
        while (timeLeft > 0)
        {
            leftLaser.SetPosition(0, leftLaser.transform.position);
            leftLaser.SetPosition(1, target.transform.position);

            rightLaser.SetPosition(0, rightLaser.transform.position);
            rightLaser.SetPosition(1, target.transform.position);

            for (int i = 0; i < lines.Length; ++i)
            {
                lines[i].SetPosition(0, target.transform.position);
                lines[i].SetPosition(1, nearby[i].transform.position);
            }

            timeLeft -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        leftLaser.enabled = false;
        rightLaser.enabled = false;

        for (int i = 0; i < nearby.Length; ++i)
            nearby[i].GetComponent<Rock>().Die();

        target.GetComponent<Rock>().Die();
    }

    IEnumerator Turbo()
    {
        Debug.Log("Max Warp!");

        hull.SetActive(false);
        turbo.SetActive(true);
        speed *= 10;
        yield return new WaitForSeconds(turboLenght);

        hull.SetActive(true);
        turbo.SetActive(false);
        speed /= 10;
        inFTL = false;
    }
}
