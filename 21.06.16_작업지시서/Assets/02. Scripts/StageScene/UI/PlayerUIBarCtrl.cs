using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIBarCtrl : MonoBehaviour
{
    PlayerCtrl player;
    
    [SerializeField]
    Slider hpBar;
    [SerializeField]
    Text hpText;

    [SerializeField]
    Slider mpBar;
    [SerializeField]
    Text mpText;

    [SerializeField]
    Slider expBar;
    [SerializeField]
    Text expText;

    void Start()
    {
        player = GameObject.FindWithTag("PLAYER").GetComponent<PlayerCtrl>();
    }

    void Update()
    {
        UpdateHpSlider();
        UpdateMpSlider();
        UpdateExpSlider();

        hpText.text = player.hp + " / " + player.hpMax;
        mpText.text = player.mp + " / " + player.mpMax;
        expText.text = player.exp + " / " + player.expMax;
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

    void UpdateMpSlider()
    {
        mpBar.value = Mathf.Lerp(mpBar.value, player.mp / player.mpMax * 100, Time.deltaTime * 2);

        if (mpBar.value <= 2f)
        {
            GameObject.Find("PlayerExpFill").GetComponent<Image>().enabled = false;
        }
        else
        {
            GameObject.Find("PlayerExpFill").GetComponent<Image>().enabled = true;
        }
    }

    private void UpdateExpSlider()
    {
        expBar.value = Mathf.Lerp(expBar.value, player.exp / player.expMax * 100, Time.deltaTime * 2);

        if ((player.state == PlayerCtrl.State.DIE) && (expBar.value <= 0.2f))
        {
            GameObject.Find("PlayerHpFill").GetComponent<Image>().enabled = false;
        }
        else
        {
            GameObject.Find("PlayerHpFill").GetComponent<Image>().enabled = true;
        }
    }
}
