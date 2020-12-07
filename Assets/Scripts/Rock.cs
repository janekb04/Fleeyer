using UnityEngine;

public class Rock : MonoBehaviour
{
    public float startHp;
    public float multiplier;
    float hp;

    public float HP
    {
        get => hp;
    }

    Transform spaceship;

    private void Start()
    {
        hp = startHp;

        spaceship = GameManager.Get().Spaceship.transform;
    }

    public void Damage(float amount)
    {
        hp -= amount;
        if (hp <= 0)
        {
            Die();
        }
    }

    private void Update()
    {
        if (spaceship != null && Vector2.Distance(transform.position, spaceship.position) > GameManager.Get().RenderDistance)
        {
            Destroy(gameObject);
        }
    }

    public GameObject Die()
    {
        GameManager gameManager = GameManager.Get();

        GameObject explosion = Explosion.Explode(gameObject);
        gameManager.Score += (int)(startHp * multiplier);
        if (startHp > gameManager.RockSizeToSpawnBoosterThreshold && Random.value <= gameManager.BoosterSpawnChance)
            Instantiate(gameManager.Booster, transform.position, Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f)));
        return explosion;
    }
}
