using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBarCtrl : MonoBehaviour
{
    PlayerCtrl player;
    public Slider expBar;
    public Text ExpText;

    void Start()
    {
        player = GameObject.FindWithTag("PLAYER").GetComponent<PlayerCtrl>();
    }

    void Update()
    {
        UpdateMpSlider();
        
        ExpText.text = player.exp + " / " + player.expMax;
    }

    void UpdateMpSlider()
    {
        expBar.value = Mathf.Lerp(expBar.value, player.exp / player.expMax * 100, Time.deltaTime * 2);
        
        if (expBar.value <= 2f)
        {
            GameObject.Find("PlayerExpFill").GetComponent<Image>().enabled = false;
        }
        else
        {
            GameObject.Find("PlayerExpFill").GetComponent<Image>().enabled = true;
        }

    }
}
