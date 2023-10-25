using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroll : MonoBehaviour
{
    [Range(0,3f)]
    public float Speed;

    void Update()
    {
        transform.Translate(Vector2.down * Time.deltaTime * Speed);
        if (transform.position.y < -11.4f)
        {
            transform.position = new Vector2(transform.position.x, 11f);
        }
    }
}
