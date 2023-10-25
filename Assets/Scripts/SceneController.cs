using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Define;
using TMPro;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    public Scenes CurrentScene;

    public TextMeshProUGUI TX;

    public AudioClip TextClip;

    bool isSkip = false;

    List<string> story = new List<string>();

    private void Awake()
    {
        Instance = this;
        story.Add("현대 21세기에 인간의 무차별한 환경오염으로 인해");
        story.Add("지구상에 존재하는 ");
        story.Add("물, 토양, 대기 환경의 오염으로 인해");
        story.Add("과거에 나타나지 않았던");
        story.Add("신종 코로나-19 바이러스가 생성되어");
        story.Add("우리 몸에 들어와 몸속 세포 하나를 감염시키는데");
        story.Add("그치지 않고 연쇄적으로 다른 사람들을 감염시킨다.");
        story.Add("인간들은 신종 코로나-19 바이러스를 퇴치하기 위하여");
        story.Add("백신 개발에 투자하고 연구한 결과 백신 로봇을 개발하여");
        story.Add("새로운 바이러스에 대처하고");
        story.Add("지구상의 환경오염을 개선하려는 움직임이");
        story.Add("여기저기에서 보이기 시작했다...");
    }

    void Start()
    {
        if (CurrentScene == Scenes.Start)
        {
            StopCoroutine(StartText());
            StartCoroutine(StartText());
        }
    }

    void Update()
    {
        
    }

    public void LoadScene(Scenes Type)
    {
        var GM = GameManager.Instance;
        switch (Type)
        {
            case Scenes.Main:
                CurrentScene = Scenes.Main;
                SceneManager.LoadScene("Main");
                break;
            case Scenes.Stage1:
                GM.Stage1Clear = false;
                GM.Stage2Clear = false;
                CurrentScene = Scenes.Stage1;
                GM.GameTimerScore = 0;
                GM.HPScore = 0;
                GM.ItemScore = 0;
                GM.KillMonsterScore = 0;
                GM.PainScore = 0;
                GM.StageScore = 0;
                GM.FeverGauge = 0;
                SceneManager.LoadScene("Stage1");
                break;
            case Scenes.Stage2:
                if (CurrentScene == Scenes.Stage1)
                {
                    GM.PlayerBullet = GM.Player.GetComponent<PlayerController>().BulletLevel;
                    GM.HPScore += GM.Player.GetComponent<PlayerController>().HP * 100;
                    GM.PainScore += (100- GM.Player.GetComponent<PlayerController>().Pain) * 10;
                }
                CurrentScene = Scenes.Stage2;
                SceneManager.LoadScene("Stage2");
                break;
            case Scenes.Finish:
                if (CurrentScene == Scenes.Stage2)
                {
                    GM.PlayerBullet = GM.Player.GetComponent<PlayerController>().BulletLevel;
                    GM.HPScore += GM.Player.GetComponent<PlayerController>().HP * 100;
                }
                CurrentScene = Scenes.Finish;
                SceneManager.LoadScene("Finish");
                break;
        }
    }

    IEnumerator StartText()
    {
        foreach(var i in story)
        {
            if (isSkip) continue;
            TX.text = "";
            foreach(var j in i)
            {
                if (isSkip) continue;
                TX.text += j;
                SoundManager.Instance.SFXPlay("Text", TextClip, 0.1f);
                yield return new WaitForSeconds(0.07f);
            }
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(0.2f);
        LoadScene(Scenes.Main);
    }

    public void Skip()
    {
        isSkip = true;
        LoadScene(Scenes.Main);
    }
}
