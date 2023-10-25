using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public Vector2 Direction;

    public int ATK;

    public float Speed;

    void Start()
    {
        Destroy(gameObject, 1.5f);
    }

    void Update()
    {
        transform.Translate(Direction * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().HPController(ATK);
            Destroy(gameObject);
        }
    }
}
