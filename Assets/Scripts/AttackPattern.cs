using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPattern : Actor
{
    public GameObject Bullet;

    protected void CreateBullet(Vector2 pos,Vector2 dir,float speed)
    {
        GameObject obj = (GameObject)Instantiate(Bullet);
        obj.transform.position = pos;
        obj.GetComponent<EnemyBullet>().Direction = dir;
        obj.GetComponent<EnemyBullet>().ATK = ATK;
        obj.GetComponent<EnemyBullet>().Speed = speed;
    }

    protected void CreateBullet(Vector2 pos, Quaternion rot, Vector2 dir, float speed)
    {
        GameObject obj = (GameObject)Instantiate(Bullet);
        obj.transform.position = pos;
        obj.transform.rotation = rot;
        obj.GetComponent<EnemyBullet>().Direction = dir;
        obj.GetComponent<EnemyBullet>().ATK = ATK;
        obj.GetComponent<EnemyBullet>().Speed = speed;
    }

    protected IEnumerator Striaght_Three(Vector2 pos)
    {
        Quaternion rot;
        for (int i = 0; i < 3; i++)
        {
            rot = Quaternion.Euler(0, 0, -10 + (10 * i));
            CreateBullet(pos, rot, Vector2.down, 8.5f);
            yield return new WaitForSeconds(0.02f);
        }
    }

    protected void ToPlayer_One(Vector2 pos)
    {
        Vector2 dir = (GameManager.Instance.Player.transform.position - transform.position).normalized;
        CreateBullet(pos,dir, 8.5f);
    }

    protected IEnumerator BowShape(Vector2 pos)
    {
        Quaternion rot;
        for (int i = 0; i<6; i++)
        {
            rot = Quaternion.Euler(0, 0, -90 + (i * 30));
            CreateBullet(pos, rot, Vector2.down, 8.5f);
            yield return new WaitForSeconds(0.07f);
        }
    }

    protected void Circle(Vector2 pos, int count)
    {
        Quaternion rot;
        for (int i = 0; i <= count; i++)
        {
            rot = Quaternion.Euler(0, 0, (360 / count) * i);
            CreateBullet(pos, rot, Vector2.up, 6.5f);
        }
    }
}
