using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    public int Level;
    public int Atk;
    public int Skill_Lv;
}

public class PlayerTest : MonoBehaviour
{
    public PlayerStats playerStats;

    SkillDatabase skillDB;

    private void Awake()
    {
        skillDB = FindObjectOfType<SkillDatabase>();

        playerStats.Level = 5;
        playerStats.Atk = 10;
        playerStats.Skill_Lv = 1;
    }

    void Start()
    {
    }

    void Update()
    {

    }
}
