using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankPrefabs : MonoBehaviour
{
    public int Rank;
    public string Name;
    public int Score;

    public TextMeshProUGUI Rank_Text;
    public TextMeshProUGUI Name_Text;
    public TextMeshProUGUI Score_Text;

    void Update()
    {
        Rank_Text.text = Rank.ToString() + "µî";
        Name_Text.text = Name.ToString();
        Score_Text.text = Score.ToString() + "Á¡";
    }
}
