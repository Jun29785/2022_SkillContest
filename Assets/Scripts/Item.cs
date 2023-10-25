using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

public class Item : MonoBehaviour
{
    public ItemType Type;

    public AudioClip GetItem;

    void Update()
    {
        transform.Translate(Vector2.down * 1.5f * Time.deltaTime);
    }

    void ItemEffect(GameObject player)
    {
        switch (Type)
        {
            case ItemType.BulletUpgrade:
                if (player.GetComponent<PlayerController>().BulletLevel < 4) player.GetComponent<PlayerController>().BulletLevel += 1;
                UIManager.Instance.GetItem("�Ѿ� ���׷��̵�");
                break;
            case ItemType.God:
                player.GetComponent<PlayerController>().God();
                UIManager.Instance.GetItem("����");
                break;
            case ItemType.Heal:
                player.GetComponent<PlayerController>().HPController(-20);
                UIManager.Instance.GetItem("ü�� ������ ȸ��");
                break;
            case ItemType.Pain:
                player.GetComponent<PlayerController>().PainController(-20);
                UIManager.Instance.GetItem("���� ������ ȸ��");
                break;
            case ItemType.BulletSpeed:
                player.GetComponent<PlayerController>().SpeedIncrease();
                UIManager.Instance.GetItem("�Ѿ� �ӵ� ����");
                break;
            case ItemType.BomberMod:
                player.GetComponent<PlayerController>().Bomb();
                UIManager.Instance.GetItem("��ź ��� ����");
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager.Instance.SFXPlay("GetItem", GetItem,1.2f);
            ItemEffect(collision.gameObject);
            GameManager.Instance.ItemScore += 1;
            Destroy(gameObject);
        }
    }
}
