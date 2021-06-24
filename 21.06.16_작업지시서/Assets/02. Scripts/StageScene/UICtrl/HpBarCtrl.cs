using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarCtrl : MonoBehaviour
{
    PlayerCtrl player;
    public Slider hpBar;

    void Start()
    {
        player = GameObject.FindWithTag("PLAYER").GetComponent<PlayerCtrl>();
    }

    void Update()
    {
        UpdateHpSlider();

        if ((player.state == PlayerCtrl.State.DIE) && (hpBar.value <= 2f))
        {
            GameObject.Find("PlayerHpFill").GetComponent<Image>().enabled = false;
        }
        else
        {
            GameObject.Find("PlayerMpFill").GetComponent<Image>().enabled = true;
        }
    }

    void UpdateHpSlider()
    {
        hpBar.value = Mathf.Lerp(hpBar.value, player.hp / player.hpMax * 100, Time.deltaTime * 2);
    }
}
