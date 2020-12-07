using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour
{
    [SerializeField]
    float scroll, speed;

    void Update()
    {
        transform.Translate(-Vector2.up * speed * Time.deltaTime);
        if (transform.localPosition.y <= -scroll)
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + scroll, transform.localPosition.z);
    }
}
