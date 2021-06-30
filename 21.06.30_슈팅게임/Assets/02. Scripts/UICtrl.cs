using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICtrl : MonoBehaviour
{
    public PlayerCtrl player;
    public Text scoreText;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerCtrl>();
    }

    void Update()
    {
        scoreText.text = string.Format("{0:000000}", player.score);
    }
}
