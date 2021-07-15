using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHpBarCtrl : MonoBehaviour
{
    public GameObject hpBar;
    public Slider hpBarSlider;
    public Text mobName;
    public Text monsterHpText;

    public PlayerWeaponCtrl Pwp;
    MonsterCtrl mob;

    void Start()
    {
        hpBar.SetActive(false);
    }
    void LateUpdate()
    {
        if (Pwp.hitmob != null)
        {
            mob = Pwp.hitmob;

            hpBar.SetActive(true);

            UpdateHpSlider();

            mobName.text = "Lv." + mob.level + " " + mob.sName;
            monsterHpText.text = mob.hp + " / " + mob.hpMax;
        }
        else
        {
            hpBar.SetActive(false);
        }
    }

    void UpdateHpSlider()
    {
        hpBarSlider.value = Mathf.Lerp(hpBarSlider.value, mob.hp / mob.hpMax * 100, Time.deltaTime * 2);

        if (mob.state == MonsterCtrl.State.DIE)
        {
            GameObject.Find("MonsterHpFill").GetComponent<Image>().enabled = false;
        }
        else
        {
            GameObject.Find("MonsterHpFill").GetComponent<Image>().enabled = true;
        }
    }
}
