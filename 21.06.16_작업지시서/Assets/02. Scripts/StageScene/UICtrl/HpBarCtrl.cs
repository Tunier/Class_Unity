using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarCtrl : MonoBehaviour
{
    PlayerCtrl player;
    public Slider hpBar;
    public Text hpText;

    void Start()
    {
        player = GameObject.FindWithTag("PLAYER").GetComponent<PlayerCtrl>();
    }

    void Update()
    {
        UpdateHpSlider();

        hpText.text = player.hp + " / " + player.hpMax;
    }

    void UpdateHpSlider()
    {
        hpBar.value = Mathf.Lerp(hpBar.value, player.hp / player.hpMax * 100, Time.deltaTime * 2);

        if ((player.state == PlayerCtrl.State.DIE) && (hpBar.value <= 0.5f))
        {
            GameObject.Find("PlayerHpFill").GetComponent<Image>().enabled = false;
        }
        else
        {
            GameObject.Find("PlayerHpFill").GetComponent<Image>().enabled = true;
        }
    }
}
