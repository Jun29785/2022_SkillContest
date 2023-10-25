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
        story.Add("���� 21���⿡ �ΰ��� �������� ȯ��������� ����");
        story.Add("������ �����ϴ� ");
        story.Add("��, ���, ��� ȯ���� �������� ����");
        story.Add("���ſ� ��Ÿ���� �ʾҴ�");
        story.Add("���� �ڷγ�-19 ���̷����� �����Ǿ�");
        story.Add("�츮 ���� ���� ���� ���� �ϳ��� ������Ű�µ�");
        story.Add("��ġ�� �ʰ� ���������� �ٸ� ������� ������Ų��.");
        story.Add("�ΰ����� ���� �ڷγ�-19 ���̷����� ��ġ�ϱ� ���Ͽ�");
        story.Add("��� ���߿� �����ϰ� ������ ��� ��� �κ��� �����Ͽ�");
        story.Add("���ο� ���̷����� ��ó�ϰ�");
        story.Add("�������� ȯ������� �����Ϸ��� ��������");
        story.Add("�������⿡�� ���̱� �����ߴ�...");
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
