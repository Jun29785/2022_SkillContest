using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

public class PlayerBullet : MonoBehaviour
{
    public PlayerBulletType type;

    [Range(0f, 10f)]
    public float Speed;

    public int ATK;

    public GameObject Bomb_Frag;

    private void Start()
    {
        Destroy(gameObject, 1.5f);
        Invoke("BulletEvent", 0.15f);

    }

    void Update()
    {
        BulletMove();
    }

    void BulletMove()
    {
        transform.Translate(Vector2.up * Speed * Time.deltaTime);
    }

    void BulletEvent()
    {
        switch (type)
        {
            case PlayerBulletType.Side:
                if (transform.position.x > transform.parent.position.x) { transform.rotation = Quaternion.Euler(0, 0, -30); }
                else { transform.rotation = Quaternion.Euler(0, 0, 30); }
                break;
            case PlayerBulletType.Bomb:
                Invoke("Bomb", 0.2f);
                break;
        }
    }

    void Bomb()
    {
        GameObject obj;
        for (int i = 0; i < 12; i++)
        {
            obj = Instantiate(Bomb_Frag);
            obj.transform.position = transform.position;
            obj.transform.rotation = Quaternion.Euler(0, 0, 30 * i);
            obj.GetComponent<PlayerBullet>().ATK = ATK / 3 * 2;
            obj.GetComponent<PlayerBullet>().Speed = 9.5f;
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (type == PlayerBulletType.Bomb) { Bomb(); }
            if (collision.gameObject.layer == 9)
            {
                collision.GetComponentInParent<Boss>().OnDamage(ATK);
            }
            else
            {
                collision.GetComponentInParent<Enemy>().OnDamage(ATK);
            }
            Destroy(gameObject);
        }
        if (collision.CompareTag("NPC"))
        {
            // NPC Effect
            collision.GetComponent<NPC>().NPCEffect();
            Destroy(gameObject);
        }
    }
}
