using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextCtrl : MonoBehaviour
{
    public PlayerCtrl player;
    public PlayerWeaponCtrl Pwp;

    public Text lvtext;
    public Text levelText;
    public Text strText;
    public Text dexText;
    public Text atkText;
    public Text defText;
    public Text critText;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerCtrl>();
    }

    void LateUpdate()
    {
        lvtext.text = "Lv : " + player.level;
        
        levelText.text = "Level : " + player.level;
        strText.text = "Str : " + (int)player.str;
        dexText.text = "Dex : " + (int)player.dex;
        atkText.text = "Atk : " + (int)Pwp.damage;
        defText.text = "Def : " + (int)player.def;
        critText.text = "Crit : " + Pwp.critcalChance + " %";
    }

    
}
