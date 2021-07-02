using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextCtrl : MonoBehaviour
{
    public PlayerCtrl player;
    public PlayerWeaponCtrl Pwp;
    
    public Text levelText;
    public Text ATKText;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerCtrl>();
    }

    void LateUpdate()
    {
        levelText.text = "Level : " + player.level;
        ATKText.text = "ATK : " + Pwp.damage;
    }

    
}
