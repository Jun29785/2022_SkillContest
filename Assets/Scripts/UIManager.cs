using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Fever")]
    public Slider FeverSlider;
    public TextMeshProUGUI FeverText;

    [Header("Game")]
    public GameObject PauseMenu;
    public GameObject GameMessge;

    [Header("Player")]
    public Slider HP;
    public TextMeshProUGUI HPText;
    public Slider Pain;
    public TextMeshProUGUI PainText;
    public TextMeshProUGUI ScoreText;
    public Transform ItemTextParent;
    public GameObject ItemText;

    [Header("Boss")]
    public Slider BossHP;
    public TextMeshProUGUI BossHPText;
    public GameObject BossWarning;

    [Header("Cheat")]
    public GameObject Cheat;
    public Slider HPCheat;
    public Slider PainCheat;
    public TextMeshProUGUI PainCheatText;
    public TextMeshProUGUI HPCheatText;

    public bool BossHPBar = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        HP.maxValue = GameManager.Instance.Player.GetComponent<PlayerController>().HP;
        Pain.maxValue = 100;
    }

    private void Update()
    {
        CheatUpdate();
        PlayerUpdate();
        FeverUpdate();
        if (BossHPBar && !BossHP.gameObject.activeSelf) { BossHP.gameObject.SetActive(true); }
        if (BossHP.gameObject.activeSelf) { BossUpdate(); }
    }

    void FeverUpdate()
    {
        var GM = GameManager.Instance;
        FeverSlider.value = GM.FeverGauge;
        if (GM.FeverGauge >= 100)
        {
            GM.isFever = true;
        }
        if (GM.isFever & GM.FeverGauge <= 0)
        {
            GM.isFever = false;
        }
        if (GM.FeverGauge < 100 && !GM.isFever)
        {
            FeverText.text = GM.FeverGauge + "%";
        }
        else
        {
            FeverText.text = "Fever";
        }
    }

    void PlayerUpdate()
    {
        var Player = GameManager.Instance.Player.GetComponent<PlayerController>();
        HPText.text = Player.HP.ToString() + "%";
        PainText.text = Player.Pain.ToString() + "%";
        HP.value = Player.HP;
        Pain.value = Player.Pain;
        ScoreText.text = "Á¡¼ö : " + GameManager.Instance.KillMonsterScore.ToString();
    }

    void BossUpdate()
    {
        var Boss = GameManager.Instance.Boss.GetComponent<Boss>();
        BossHP.maxValue = Boss.MaxHP;
        BossHP.value = Boss.HP;
        BossHPText.text = ((int)(((float)Boss.HP / (float)Boss.MaxHP) * 100)).ToString() + "%";
    }

    void CheatUpdate()
    {
        PainCheatText.text = ((int)PainCheat.value).ToString() + "%";
        HPCheatText.text = ((int)HPCheat.value).ToString() + "%";
        if (Cheat.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                GameManager.Instance.Player.GetComponent<PlayerController>().HP = (int)HPCheat.value;
                GameManager.Instance.Player.GetComponent<PlayerController>().Pain = (int)PainCheat.value;
                Cheat.SetActive(false);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                HPCheat.value = GameManager.Instance.Player.GetComponent<PlayerController>().HP;
                PainCheat.value = GameManager.Instance.Player.GetComponent<PlayerController>().Pain;
                Cheat.SetActive(true);
            }
        }
    }

    public void GetItem(string name)
    {
        GameObject obj = Instantiate(ItemText);
        obj.transform.parent = ItemTextParent;
        obj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; 
        obj.GetComponent<TextMeshProUGUI>().text = name + " È¹µæ";
        Destroy(obj, 0.5f);
    }

    public void OnClickPause(int i)
    {
        if (i == 0)
        {
            Time.timeScale = 1f;
            PauseMenu.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            PauseMenu.SetActive(true);
        }
    }

    public void GoMenu()
    {
        Time.timeScale = 1f;
        SceneController.Instance.LoadScene(Define.Scenes.Main);
    }

    public void Message(string msg)
    {
        GameMessge.SetActive(true);
        GameMessge.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = msg;
        Debug.Log("End1");
    }
}
