using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    PlayerCtrl player;
    
    public Text scoreText;
    public Text lifeText;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        scoreText.text = string.Format("{0:00000000}", player.score);
        lifeText.text = "x" + player.life;
    }
}
