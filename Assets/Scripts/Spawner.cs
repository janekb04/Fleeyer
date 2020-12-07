using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    GameObject enemy;
    [SerializeField]
    public Vector2 spawnArea;
    [SerializeField]
    float baseSpawnFrequency, spawnAcceleration;
    [SerializeField]
    float minScale, maxScale;
    [SerializeField]
    float hpMultiplier, scoreMultiplier;

    float spawnFrequency;
    Coroutine spawnCoroutine;

    private void OnEnable()
    {
        spawnFrequency = baseSpawnFrequency;

        spawnCoroutine = StartCoroutine(Spawn());
    }

    private void OnDisable()
    {
        StopCoroutine(spawnCoroutine);
    }

    private void Update()
    {
        spawnFrequency += spawnAcceleration * Time.deltaTime;
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            Vector2 pos = new Vector2(Random.Range(transform.position.x - spawnArea.x, transform.position.x + spawnArea.x),
                                      Random.Range(transform.position.y - spawnArea.y, transform.position.y + spawnArea.y));
            float rotation = Random.Range(0, 360);
            float scale = Random.Range(minScale, maxScale);

            GameObject spawnedObject = Instantiate(enemy, pos, Quaternion.Euler(0, 0, rotation));
            spawnedObject.transform.localScale *= scale;
            Rock spawnedRock = spawnedObject.GetComponent<Rock>();
            spawnedRock.startHp = scale * hpMultiplier;
            spawnedRock.multiplier = scoreMultiplier;

            yield return new WaitForSeconds(1 / spawnFrequency);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 bounds = spawnArea;
        Vector2 position = transform.position;

        Vector2 topLeft = new Vector2(position.x - bounds.x, position.y + bounds.y);
        Vector2 bottomLeft = new Vector2(position.x - bounds.x, position.y - bounds.y);
        Vector2 topRight = new Vector2(position.x + bounds.x, position.y + bounds.y);
        Vector2 bottomRight = new Vector2(position.x + bounds.x, position.y - bounds.y);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(bottomLeft, topLeft);
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(bottomLeft, bottomRight);
    }
}
