using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

public class Enemy : AttackPattern
{
    public AudioClip EnemyHit;

    public EnemyType Type;
    public Direction Direction;

    public bool isEntered = false;
    public bool canFire = false;

    [Range(0, 100)]
    public int Score;

    public Transform FirePos;

    public GameObject EnemySprite;

    float TurnTimer = 0;
    Vector2 Dir;

    Animator Anim;
    private void Awake()
    {
        EnemySprite = transform.GetChild(0).gameObject;
        Anim = EnemySprite.GetComponent<Animator>();
    }

    void Start()
    {
        EnemyEnter();
    }

    void Update()
    {
        if (isEntered)
        {
            EnemyMove();
        }
        if (canFire)
        {
            canFire = false;
            EnemyFire();
        }
    }

    void EnemyEnter()
    {
        switch (Type)
        {
            case EnemyType.Bacteria:
                if (transform.position.y > 4.5f)
                {
                    transform.Translate(Vector2.down * Speed * Time.deltaTime);
                }
                else
                {
                    isEntered = true;
                }
                break;
            case EnemyType.Germ:
            case EnemyType.Cancer:
                if (transform.position.y > 4.3f)
                {
                    transform.Translate(Vector2.down * Speed * 1.2f * Time.deltaTime);
                }
                else
                {
                    isEntered = true;
                    canFire = true;
                }
                break;
            case EnemyType.Virus:
                if (transform.position.y < 4.3f)
                {
                    transform.Translate(Vector2.up * Speed * 2.6f * Time.deltaTime);
                }
                else
                {
                    isEntered = true;
                    canFire = true;
                    Dir = Vector2.down;
                }
                break;
        }
        if (!isEntered) { Invoke("EnemyEnter", Time.deltaTime); }
    }

    void EnemyMove()
    {
        switch (Type)
        {
            case EnemyType.Bacteria:
                EnemySprite.transform.RotateAround(transform.position, Vector3.back, (int)Direction * Time.deltaTime * Speed * 50);
                transform.Translate(Vector2.down * Speed * 0.4f * Time.deltaTime);
                break;
            case EnemyType.Germ:
            case EnemyType.Virus:
                transform.Translate(Vector2.down * Speed * Time.deltaTime);
                break;
            case EnemyType.Cancer:
                CancerMove();
                break;
        }
    }

    void CancerMove()
    {
        TurnTimer += Time.deltaTime;
        if (TurnTimer > 1f) { TurnTimer = 0f; Dir = Vector2.down; }
        else if (TurnTimer > 0.4f) { Dir = new Vector2((int)Direction, 0); }
        transform.Translate(Dir * Speed * Time.deltaTime);
    }

    void EnemyFire()
    {
        switch (Type)
        {
            case EnemyType.Germ:
                StopCoroutine(base.Striaght_Three(FirePos.position));
                StartCoroutine(base.Striaght_Three(FirePos.position));
                base.ToPlayer_One(FirePos.position);
                Invoke("EnemyFire", Random.Range(0.7f, 0.8f));
                break;
            case EnemyType.Virus:
                StopCoroutine(base.BowShape(FirePos.position));
                StartCoroutine(base.BowShape(FirePos.position));
                Invoke("EnemyFire", Random.Range(0.8f, 0.9f));
                break;
            case EnemyType.Cancer:
                base.Circle(FirePos.position, 12);
                Invoke("EnemyFire", Random.Range(1f, 1.1f));
                break;
        }
    }

    public void OnDamage(int value)
    {
        SoundManager.Instance.SFXPlay("EnemyHit", EnemyHit, 0.05f);
        HP -= value;
        Mathf.Clamp(HP, 0, MaxHP);
        if (HP <= 0)
        {
            StopCoroutine(EnemyDie());
            StartCoroutine(EnemyDie());
        }
        else
        {
            Anim.SetTrigger("Dmg");
        }
    }

    IEnumerator EnemyDie()
    {
        Anim.SetTrigger("Die");
        if (GameManager.Instance.isFever)
        {
            GameManager.Instance.KillMonsterScore += (Score * 2);
        }
        GameManager.Instance.KillMonsterScore += Score;
        if (!GameManager.Instance.isFever)
            GameManager.Instance.FeverGauge += (Score / 5);
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}
