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
        strText.text = "Str : " + Mathf.Round(player.str);
        dexText.text = "Dex : " + Mathf.Round(player.dex);
        atkText.text = "Atk : " + Mathf.Round(player.minDamage) + " ~ " + Mathf.Round(player.maxDamage);
        defText.text = "Def : " + Mathf.Round(player.def);
        critText.text = "Crit : " + player.critcalChance + " %";
    }
}
