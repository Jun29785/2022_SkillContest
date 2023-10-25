using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Actor
{
    Vector2 Direction;

    [Header("Audio")]
    public AudioClip Attack;
    public AudioClip Hit;
    public AudioClip AttackBomb;
    public AudioClip ac_God;

    public int Pain;

    public int BulletLevel;

    public List<GameObject> Bullet;

    public Transform FirePos;

    bool canFire;
    public float FireSpeed;
    float FireTime;

    bool isGod = false;

    public bool isBomber;

    public Animator Anim;

    public bool isEntered;

    private void Awake()
    {
        GameManager.Instance.Player = this.gameObject;
    }

    void Start()
    {
        Initialize();
        PlayerEnter();
    }

    private void Update()
    {
        FireTime += Time.deltaTime;
        if (!canFire && FireTime > FireSpeed)
        {
            FireTime = 0;
            canFire = true;
        }
        if (Input.GetKeyDown(KeyCode.A)) { Anim.SetBool("Left", true); }
        if (Input.GetKeyUp(KeyCode.A)) { Anim.SetBool("Left", false); }
        if (Input.GetKeyDown(KeyCode.D)) { Anim.SetBool("Right", true); }
        if (Input.GetKeyUp(KeyCode.D)) { Anim.SetBool("Right", false); }


        if (Input.GetKeyDown(KeyCode.Y)) { Anim.SetTrigger("God"); }
    }

    void PlayerEnter()
    {
        if (transform.position.y < -3f)
        {
            transform.Translate(Vector2.up * Speed * 1.4f * Time.deltaTime);
        }
        else
        {
            isEntered = true;
        }
        if (!isEntered) { Invoke("PlayerEnter", Time.deltaTime); }
    }

    void Initialize()
    {
        switch (SceneController.Instance.CurrentScene)
        {
            case Define.Scenes.Stage1:
                BulletLevel = 0;
                HP = 100;
                Pain = 10;
                FireSpeed = 0.2f;
                canFire = true;
                isBomber = false;
                break;
            case Define.Scenes.Stage2:
                BulletLevel = GameManager.Instance.PlayerBullet;
                HP = 100;
                Pain = 30;
                FireSpeed = 0.2f;
                canFire = true;
                isBomber = false;
                break;
        }
    }

    private void FixedUpdate()
    {
        HPController(0);
        PainController(0);
        if (isEntered)
        {
            Move();
            transform.Translate(Direction * Speed * Time.fixedDeltaTime);
        }
    }

    void Move()
    {
        Direction = Vector2.zero;
        if (Input.GetKey(KeyCode.A) && transform.position.x > -8.6f) { Direction += Vector2.left; }
        if (Input.GetKey(KeyCode.S) && transform.position.y > -4.7f) { Direction += Vector2.down; }
        if (Input.GetKey(KeyCode.D) && transform.position.x < 8.6f) { Direction += Vector2.right; }
        if (Input.GetKey(KeyCode.W) && transform.position.y < 4.7f) { Direction += Vector2.up; }

        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && canFire) { canFire = false; Fire(); }
    }

    void Fire()
    {
        GameObject obj;
        if (isBomber)
        {
            SoundManager.Instance.SFXPlay("BombAttack", AttackBomb,0.07f);
            obj = Instantiate(Bullet[Bullet.Count - 1]);
            obj.GetComponent<PlayerBullet>().Speed = 2.5f;
        }
        else
        {
            SoundManager.Instance.SFXPlay("PlayerAttack", Attack,0.07f);
            obj = (GameObject)Instantiate(Bullet[BulletLevel]);
        }
        obj.transform.position = FirePos.position;
        obj.GetComponent<PlayerBullet>().ATK = ATK;
    }

    public void HPController(int value)
    {
        if (value > 0 && GameManager.Instance.ForceGod) { return; }
        if (value > 0 && isGod) { return; }
        HP -= value * GameManager.Instance.balance;
        if (value > 0 && !isGod)
        {
            Anim.SetTrigger("GetDamage");
            SoundManager.Instance.SFXPlay("PlayerHit", Hit,0.07f);
            StopCoroutine(GodMod(1.5f));
            StartCoroutine(GodMod(1.5f));
        }
        if (HP > MaxHP) { HP = MaxHP; }
        if (HP < 0) { HP = 0; }
        if (HP <= 0)
        {
            //GameOver
            StartCoroutine(OnDie("체력 게이지로 인한 게임 오버"));
        }
    }

    public void PainController(int value)
    {
        Pain += (int)(value * GameManager.Instance.balance);
        if (Pain <0) { Pain = 0; }
        if (Pain >= 100)
        {
            //GameOver
            StartCoroutine(OnDie("고통 게이지로 인한 게임 오버"));
        }
    }

    IEnumerator OnDie(string msg)
    {
        UIManager.Instance.Message(msg);
        yield return new WaitForSeconds(1.3f);
        SceneController.Instance.LoadScene(Define.Scenes.Finish);
    }

    public void God()
    {
        SoundManager.Instance.SFXPlay("PlayerGod", ac_God, 0.12f);
        Anim.SetTrigger("God");
        StopCoroutine(GodMod(3f));
        StartCoroutine(GodMod(3f));
    }

    IEnumerator GodMod(float time)
    {
        isGod = true;
        yield return new WaitForSeconds(time);
        isGod = false;
    }

    public void SpeedIncrease()
    {
        StopCoroutine(FireSpeedIncrease());
        StartCoroutine(FireSpeedIncrease());
    }

    public void Bomb()

    {
        StopCoroutine(BomberMod());
        StartCoroutine(BomberMod());
    }

    IEnumerator FireSpeedIncrease()
    {
        FireSpeed = 0.1f;
        yield return new WaitForSeconds(7f);
        FireSpeed = 0.2f;
    }

    public IEnumerator BomberMod()
    {
        FireSpeed = 0.9f;
        isBomber = true;
        yield return new WaitForSeconds(9f);
        FireSpeed = 0.2f;
        isBomber = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            HPController(collision.GetComponentInParent<Enemy>().ATK/2);
        }
    }
}
