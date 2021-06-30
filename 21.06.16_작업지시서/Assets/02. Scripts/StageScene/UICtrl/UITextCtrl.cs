using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextCtrl : MonoBehaviour
{
    public Text levelText;
    public PlayerCtrl player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerCtrl>();
    }

    void Update()
    {
        levelText.text = "Level : " + player.level;
    }

    
}
