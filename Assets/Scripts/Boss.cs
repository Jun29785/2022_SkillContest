using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

public class Boss : AttackPattern
{
    public BossType type;

    [Range(0, 1000)]
    public int Score;

    [Header("Audio")]
    public AudioClip BossHit;

    [Header("FirePos")]
    public Transform FirePos;
    public Transform L_FirePos;
    public Transform R_FirePos;

    [Header("Enemy")]
    public List<GameObject> EnemyGroup;

    public Animator Anim;

    public Direction Direction;

    bool isEntered = false;
    bool canFire = false;
    bool isDash = false;
    Vector2 dir;

    public Animator Warn;

    public float BossTimer = 0f;

    private void Awake()
    {
        if (Anim == null)
        {
            Anim = transform.GetChild(0).GetComponent<Animator>();
            Debug.Log("anim");
        }
    }

    void Start()
    {
        if (type != BossType.Mini_Covid_19)
        {
            GameManager.Instance.Boss = this.gameObject;
            UIManager.Instance.BossWarning.SetActive(true);
        }
        BossEnter();
    }

    void Update()
    {
        if (dir == Vector2.left && transform.position.x > -7.4f)
            transform.Translate(Speed * dir * Time.deltaTime);
        if (dir == Vector2.right && transform.position.x < 7.4f)
            transform.Translate(Speed * dir * Time.deltaTime);
        BossTimer += Time.deltaTime;
        if (canFire && type != BossType.Mini_Covid_19) { BossMove(); UIManager.Instance.BossHPBar = true; }
        if (canFire) { canFire = false; BossTimer = 0f; BossAttack(); }
    }

    void BossMove()
    {
        switch (type)
        {
            case BossType.Covid_19:
            case BossType.Evolved_Covid_19:
                var i = Random.Range(0, 3);
                if (i == 0)
                {
                    dir = Vector2.right;
                }
                else if (i == 1)
                {
                    dir = Vector2.left;
                }
                else
                {
                    dir = Vector2.zero;
                }
                Invoke("BossMove", 2.5f);
                break;
        }

    }

    void BossEnter()
    {
        switch (type)
        {
            case BossType.Covid_19:
            case BossType.Evolved_Covid_19:
                if (transform.position.y > 3f) { transform.Translate(Vector2.down * Speed * Time.deltaTime); }
                else { isEntered = true; canFire = true; }
                break;
            case BossType.Mini_Covid_19:
                if (Mathf.Abs(transform.position.x) < 3f)
                {
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2((int)Direction * 3, transform.position.y), Speed * Time.deltaTime);
                }
                else { isEntered = true; canFire = true; }
                break;
        }
        if (!isEntered) { Invoke("BossEnter", Time.deltaTime); }
    }

    void BossAttack()
    {
        switch (type)
        {
            case BossType.Covid_19:
                Covid_19_Pattern();
                break;
            case BossType.Evolved_Covid_19:
                Evolved_Covid_19_Pattern();
                break;
            case BossType.Mini_Covid_19:
                ToPlayer_One(FirePos.position);
                Invoke("BossAttack", 0.7f);
                break;

        }
    }

    void Covid_19_Pattern()
    {
        if (BossTimer < 1f)
        {
            BossPattern(Random.Range(0, 2));
        }
        else if (BossTimer < 3f)
        {
            BossPattern(Random.Range(0, 4));
        }
        else
        {
            BossPattern(Random.Range(0, 6));
        }
    }

    void Evolved_Covid_19_Pattern()
    {
        if (BossTimer < 3f)
        {
            BossPattern(Random.Range(0, 4));
        }
        else if (BossTimer < 7f)
        {
            BossPattern(Random.Range(0, 6));
        }
        else
        {
            BossPattern(Random.Range(0, 9));
        }
    }


    void BossPattern(int num)
    {
        switch (num)
        {
            case 0:
                base.ToPlayer_One(FirePos.position);
                break;
            case 1:
                StopCoroutine(BowShape(FirePos.position));
                StartCoroutine(BowShape(FirePos.position));
                break;
            case 2:
                base.ToPlayer_One(L_FirePos.position);
                base.ToPlayer_One(R_FirePos.position);
                StopCoroutine(Striaght_Three(FirePos.position));
                StartCoroutine(Striaght_Three(FirePos.position));
                break;
            case 3:
                SpawnEnemy(0, L_FirePos);
                SpawnEnemy(0, R_FirePos);
                break;
            case 4:
                Circle(L_FirePos.position, 6);
                Circle(R_FirePos.position, 6);
                break;
            case 5:
                SpawnEnemy(Random.Range(1, 3), L_FirePos);
                SpawnEnemy(Random.Range(1, 3), R_FirePos);
                break;
            case 6:
                SpawnEnemy(3, L_FirePos);
                SpawnEnemy(4, R_FirePos);
                break;
            case 7:
                isDash = true;
                StopCoroutine(DashCor());
                StartCoroutine(DashCor());
                break;
            case 8:
                Circle(L_FirePos.position, 12);
                Circle(R_FirePos.position, 12);
                StopCoroutine(Striaght_Three(FirePos.position));
                break;
        }
        if (isDash)
        {
            Invoke("BossAttack", 1.7f);
            return;
        }
        Invoke("BossAttack", Random.Range(0.7f, 1f));
    }

    IEnumerator DashCor()
    {
        Warn.SetTrigger("Warn");
        yield return new WaitForSeconds(1.1f);
        Dash();
    }

    void SpawnEnemy(int num, Transform pos)
    {
        if (Random.Range(0, 3) > 1) { return; }
        GameObject obj = Instantiate(EnemyGroup[num]);
        obj.transform.position = pos.position;
        if (num >= 3) { return; }
        obj.GetComponent<Enemy>().isEntered = true;
        obj.GetComponent<Enemy>().canFire = true;
        if (obj.GetComponent<Enemy>().Type == EnemyType.Bacteria)
        {
            obj.GetComponent<Enemy>().Direction = Direction.Center;
            obj.GetComponent<Enemy>().Speed = 1.5f;
        }
    }

    void Dash()
    {
        Debug.Log("Dash");
        if (transform.position.y > -3.5f && isDash)
        {
            transform.Translate(Vector2.down * Speed * 9 * Time.deltaTime);
            Invoke("Dash", Time.deltaTime);
            return;
        }
        if (isDash)
            isDash = false;
        if (!isDash && transform.position.y < 3f)
        {
            transform.Translate(Vector2.up * Speed * 8.5f * Time.deltaTime);
            Invoke("Dash", Time.deltaTime);
        }
        else
        {
            transform.position = new Vector2(transform.position.x, 3f);
        }
    }

    public void OnDamage(int value)
    {
        if (!isEntered) { return; }
        Anim.SetTrigger("Dmg");
        SoundManager.Instance.SFXPlay("BossHit", BossHit, 0.03f);
        HP -= value;
        if (HP > MaxHP) { HP = MaxHP; }
        if (HP <= 0)
        {
            OnDie();
        }
    }

    void OnDie()
    {
        if (GameManager.Instance.isFever)
        {
            GameManager.Instance.KillMonsterScore += (Score * 2);
        }
        GameManager.Instance.KillMonsterScore += Score;
        switch (type)
        {
            case BossType.Covid_19:
                GameManager.Instance.Stage1Clear = true;
                StartCoroutine(GameMessage("스테이지 1 클리어!",Scenes.Stage2));
                break;
            case BossType.Evolved_Covid_19:
                GameManager.Instance.Stage2Clear = true;
                StartCoroutine(GameMessage("스테이지 2 클리어!",Scenes.Finish));
                break;
            case BossType.Mini_Covid_19:
                Destroy(gameObject);
                break;
        }
    }

    IEnumerator GameMessage(string msg, Scenes stg)
    {
        UIManager.Instance.Message(msg);
        Debug.Log("End2");
        yield return new WaitForSeconds(1.3f);
        Debug.Log("End3");
        SceneController.Instance.LoadScene(stg);
        Destroy(gameObject);
    }
}
