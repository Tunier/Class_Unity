using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextCtrl : MonoBehaviour
{
    public GameObject damagePrint;
    public Text damageText;

    public PlayerWeaponCtrl Pwp;
    public Canvas UICanvs;

    void Start()
    {
        damageText.text = "" + Pwp.damage;
    }

    void Update()
    {

    }
}
