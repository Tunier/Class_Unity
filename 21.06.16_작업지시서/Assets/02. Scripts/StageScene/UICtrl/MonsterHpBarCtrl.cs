using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHpBarCtrl : MonoBehaviour
{
    public MonsterCtrl mob;
    public Slider hpBar;

    void Start()
    {

    }

    void Update()
    {
        UpdateHpSlider();

        if (hpBar.value <= 1f)
        {
            GameObject.Find("MonsterHpFill").GetComponent<Image>().enabled = false;
        }
        else
        {
            GameObject.Find("MonsterHpFill").GetComponent<Image>().enabled = true;
        }
    }

    void UpdateHpSlider()
    {
        hpBar.value = Mathf.Lerp(hpBar.value, mob.hp / mob.hpMax * 100, Time.deltaTime * 2);
    }
}
