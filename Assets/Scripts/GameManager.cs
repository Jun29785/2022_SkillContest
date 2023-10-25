using Define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<Rank> Ranks = new List<Rank>();

    public GameObject Player;

    public GameObject Boss;

    public int KillMonsterScore;

    public float GameTimerScore;

    public int ItemScore;

    public int StageScore;

    public int HPScore;

    public int PainScore;

    public int PlayerBullet;

    public bool ForceGod;

    public GameObject White;
    public GameObject Red;

    public int balance;

    public int FeverGauge;

    public bool isFever = false;

    float fevertimer;

    public bool Stage1Clear = false;
    public bool Stage2Clear = false;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (SceneController.Instance.CurrentScene == Scenes.Main && Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if(FeverGauge <= 0)
        {
            FeverGauge = 0;
        }

        if (FeverGauge >= 100)
        {
            FeverGauge = 100;
        }
        if (isFever)
        {
            fevertimer += Time.deltaTime;
            if(fevertimer > .1f)
            {
                fevertimer = 0f;
                FeverGauge -= 2;
            }
        }
        CheatKey();
        if (Player == null) { Player = GameObject.FindGameObjectWithTag("Player"); }
        if (Player != null && Player.GetComponent<PlayerController>().isEntered)
        {
            GameTimerScore += Time.deltaTime;
        }
    }
    
    public void CheatKey()
    {
        if (Input.GetKeyDown(KeyCode.F2)) // Stage Change
        {
            if (SceneController.Instance.CurrentScene == Scenes.Stage1)
            {
                Debug.Log("stage1");
                SceneController.Instance.LoadScene(Scenes.Stage2);
            }
            else if (SceneController.Instance.CurrentScene == Scenes.Stage2)
            {
                Debug.Log("stage2");
                SceneController.Instance.LoadScene(Scenes.Stage1);
            }
        }
        if (Input.GetKeyDown(KeyCode.F3)) // God
        {
            if (ForceGod) { ForceGod = false; }
            else { ForceGod = true; }
        }
        if (Input.GetKeyDown(KeyCode.F4)) //white
            Spawn(NPCType.White);
        if (Input.GetKeyDown(KeyCode.F5)) //red
            Spawn(NPCType.Red);
        if (Input.GetKeyDown(KeyCode.F6)) // Bullet Level UP
        {
            if (SceneController.Instance.CurrentScene == Scenes.Stage1 || SceneController.Instance.CurrentScene == Scenes.Stage2)
            {
                if (Player.GetComponent<PlayerController>().BulletLevel <4)
                    Player.GetComponent<PlayerController>().BulletLevel += 1;
            }   
        }
        if (Input.GetKeyDown(KeyCode.F7))// Bullet Level Down
        {
            if (SceneController.Instance.CurrentScene == Scenes.Stage1 || SceneController.Instance.CurrentScene == Scenes.Stage2)
            {
                if (Player.GetComponent<PlayerController>().BulletLevel > 0)
                    Player.GetComponent<PlayerController>().BulletLevel -= 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.F8)) // Kill All Monster
        {
            GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
            foreach( var i in enemy)
            {
                Destroy(i.transform.parent.gameObject);
            }
        }
    }

    void Spawn(NPCType type)
    {
        GameObject obj;
        switch (type)
        {
            case NPCType.White:
                obj = Instantiate(White);
                obj.transform.position = new Vector2(Random.Range(-8f, 8f), Random.Range(7f, 11f));
                break;
            case NPCType.Red:
                obj = Instantiate(Red);
                obj.transform.position = new Vector2(Random.Range(-8f, 8f), Random.Range(-7f, -11f));
                break;
        }
    }
}

public class Rank
{
    public string Name;
    public int Score;
    public Rank(string n, int s)
    {
        Name = n;
        Score = s;
    }
}