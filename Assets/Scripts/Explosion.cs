using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Explosion
{
    public static GameObject explosion;
    public static float explosionTime;

    public static GameObject Explode(GameObject target, float size = 1.0f)
    {
        GameObject instance = GameObject.Instantiate(explosion, target.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        instance.transform.localScale *= size;

        GameObject.Destroy(instance, explosionTime);
        GameObject.Destroy(target);
        return instance;
    }
}
