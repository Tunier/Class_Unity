using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public Text levelText;
    public Text hpText;
    public Text mpText;
    public Text monsterHpText;

    PlayerCtrl player;
    public MonsterCtrl mob;
    public GameObject pauseCanvas;

    public bool isPause;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        player = GameObject.FindWithTag("PLAYER").GetComponent<PlayerCtrl>();

        isPause = false;
    }

    private void Update()
    {
        PlayerPrefs.Save();

        levelText.text = "Level : " + player.level;
        hpText.text = player.hp + " / " + player.hpMax;
        mpText.text = player.mp + " / " + player.mpMax;
        monsterHpText.text = mob.hp + " / " + mob.hpMax;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = true;
            //UnityEditor.EditorApplication.isPlaying = false;
        }
        
        if (isPause)
        {
            Time.timeScale = 0f;
            pauseCanvas.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            pauseCanvas.SetActive(false);
        }
    }
}
