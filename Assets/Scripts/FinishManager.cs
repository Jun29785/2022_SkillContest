using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FinishManager : MonoBehaviour
{
    public static FinishManager Instance;

    public Animator InputAnim;

    public Text WarnText;

    [Header("Rank")]
    public GameObject RankPrefab;

    public Transform RankGrid;

    public TextMeshProUGUI RegisterName;

    public GameObject InputName;

    [Header("Score")]
    public TextMeshProUGUI KillMonster;
    public TextMeshProUGUI Item;
    public TextMeshProUGUI Time;
    public TextMeshProUGUI Stage;
    public TextMeshProUGUI HP;
    public TextMeshProUGUI Pain;
    public TextMeshProUGUI Score_Sum;

    int SumScore;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Score();
    }

    void Update()
    {

    }

    void Score()
    {
        var Gm = GameManager.Instance;
        if (Gm.Stage1Clear) { Gm.StageScore += 3500; }
        if (Gm.Stage2Clear) { Gm.StageScore += 6500; }
        KillMonster.text = "적 처치 점수 : " + Gm.KillMonsterScore + "점";
        Item.text = "아이템 점수 : " + Gm.ItemScore *75 + "점";
        Time.text = "시간 점수 : " + (int)Gm.GameTimerScore*10 + "점";
        Stage.text = "스테이지 점수 : " + Gm.StageScore + "점";
        HP.text = "체력 점수 : " + Gm.HPScore + "점";
        Pain.text = "고통 점수 : " + Gm.PainScore + "점";
        SumScore = Gm.KillMonsterScore + Gm.ItemScore*75 + (int)Gm.GameTimerScore*10 + Gm.StageScore + Gm.HPScore + Gm.PainScore;
        Score_Sum.text = "총 점수 : " + SumScore + "점";
    }

    public void Register()
    {
        if (RegisterName.text.Length > 7)
        {
            WarnText.gameObject.SetActive(true);
            // Text Error (이름을 6자보다 길게 설정할수 없습니다. 다시 입력해주세요)
        }
        else
        {
            Debug.Log("Register");
            GameManager.Instance.Ranks.Add(new Rank(RegisterName.text, SumScore));
            LoadRank();
            StartCoroutine(EnterRegister());
        }
    }

    IEnumerator EnterRegister()
    {
        InputAnim.SetTrigger("Input");
        yield return new WaitForSeconds(1.1f);
        InputName.SetActive(false);
    }

    public void LoadRank()
    {
        for (int i = RankGrid.childCount - 1; i >= 0; i--)
        {
            Destroy(RankGrid.GetChild(i).gameObject);
        }
            
        GameManager.Instance.Ranks.Reverse();
        GameObject obj;
        if (GameManager.Instance.Ranks.Count > 5)
        {
            for (int i = 0; i < 5; i++)
            {
                obj = Instantiate(RankPrefab);
                obj.GetComponent<RankPrefabs>().Score = GameManager.Instance.Ranks[i].Score;
                obj.GetComponent<RankPrefabs>().Name = GameManager.Instance.Ranks[i].Name;
                obj.transform.parent = RankGrid;
            }
        }
        else
        {
            for (int i = 0; i < GameManager.Instance.Ranks.Count; i++)
            {
                obj = Instantiate(RankPrefab);
                obj.GetComponent<RankPrefabs>().Score = GameManager.Instance.Ranks[i].Score;
                obj.GetComponent<RankPrefabs>().Name = GameManager.Instance.Ranks[i].Name;
                obj.transform.parent = RankGrid;
            }
        }
    }

    public void GoMain()
    {
        SceneController.Instance.LoadScene(Define.Scenes.Main);
    }

    public void Retry()
    {
        SceneController.Instance.LoadScene(Define.Scenes.Stage1);
    }
}
