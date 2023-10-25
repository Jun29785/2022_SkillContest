using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

public class NPC : MonoBehaviour
{
    public NPCType type;

    public List<GameObject> Items;

    bool isTurned = false;

    public AudioClip Hit;

    private void Start()
    {
        Destroy(gameObject, 10f);
    }

    void Update()
    {
        NPCMove();
    }

    void NPCMove()
    {
        switch (type)
        {
            case NPCType.White:
                transform.Translate(Vector2.down * 1.2f * Time.deltaTime);
                break;
            case NPCType.Red:
                if (!isTurned)
                {
                    if (transform.position.y < 4.45f) { transform.Translate(Vector2.up * 1.6f * Time.deltaTime); }
                    else { isTurned = true; transform.rotation = Quaternion.Euler(0, 0, Random.Range(-75f, 75f)); }
                }
                else { transform.Translate(Vector2.down * 1.6f * Time.deltaTime); }
                break;
        }
    }
    public void NPCEffect()
    {
        switch (type)
        {
            case NPCType.White:
                GameObject obj = Instantiate(Items[Random.Range(0, Items.Count)]);
                obj.transform.position = transform.position;
                break;
            case NPCType.Red:
                GameManager.Instance.Player.GetComponent<PlayerController>().Pain += 3;
                break;
        }
        SoundManager.Instance.SFXPlay("NPCHit", Hit,0.1f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.gameObject.layer == 6)
        {
            NPCEffect();
        }
        if (type == NPCType.Red && collision.gameObject.layer == 7)
        {
            NPCEffect();
        }
    }
}
