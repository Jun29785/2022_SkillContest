using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [Header("Stats")]
    public int HP;
    [Range(0,10f)]
    public float Speed;
    public int ATK;

    public int MaxHP;

    private void Start()
    {

    }
}
