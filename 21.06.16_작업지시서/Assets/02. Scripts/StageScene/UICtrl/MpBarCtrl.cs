using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MpBarCtrl : MonoBehaviour
{
    PlayerCtrl player;
    public Slider mpBar;

    void Start()
    {
        player = GameObject.FindWithTag("PLAYER").GetComponent<PlayerCtrl>();
    }

    void Update()
    {
        UpdateMpSlider();

        if (mpBar.value <= 2f)
        {
            GameObject.Find("PlayerMpFill").GetComponent<Image>().enabled = false;
        }
        else
        {
            GameObject.Find("PlayerMpFill").GetComponent<Image>().enabled = true;
        }
    }

    void UpdateMpSlider()
    {
        mpBar.value = Mathf.Lerp(mpBar.value, player.mp / player.mpMax * 100, Time.deltaTime * 2);
    }
}
