using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager instance;

    public GameObject Help;

    public GameObject Enemy;

    public GameObject Item;

    public GameObject NPC;

    public GameObject SelectDifficulty;

    public void CancelSelect()
    {
        SelectDifficulty.SetActive(false);
    }

    public void OnStart()
    {
        SelectDifficulty.SetActive(true);
    }

    public void SelectDifficultyMenu(int balance)
    {
        GameManager.Instance.balance = balance;
        SceneController.Instance.LoadScene(Define.Scenes.Stage1);
    }

    public void HelpButton()
    {
        Help.SetActive(true);
    }

    public void HelpBack()
    {
        Help.SetActive(false);
    }

    public void EnemyIn()
    {
        Enemy.SetActive(true);
    }

    public void EnemyBack()
    {
        Enemy.SetActive(false);
    }

    public void ItemIn()
    {
        Item.SetActive(true);
    }

    public void ItemBack()
    {
        Item.SetActive(false);
    }

    public void NPCIn()
    {
        NPC.SetActive(true);
    }

    public void NPCBack()
    {
        NPC.SetActive(false);
    }
}
