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

    PlayerCtrl player;
    MonsterCtrl mob;

    void Start()
    {
        player = FindObjectOfType<PlayerCtrl>();

        hpBar.SetActive(false);
    }

    void LateUpdate()
    {
        if (player.hitmob != null)
        {
            if (player.hitmob != mob)
            {
                mob = player.hitmob;
                hpBarSlider.value = 100f;
            }

            hpBar.SetActive(true);

            UpdateHpSlider();

            mobName.text = "Lv." + mob.level + " " + mob.sName;
            monsterHpText.text = Mathf.RoundToInt(mob.hp) + " / " + mob.hpMax;
        }
        else
        {
            hpBar.SetActive(false);
        }

    }

    /// <summary>
    /// hp���� value���� Lerp�����ְ�, ���Ͱ� ������ hp�� �����̴� �̹����� ����.
    /// </summary>
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
