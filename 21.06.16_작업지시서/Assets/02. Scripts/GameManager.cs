using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public Text scoreText;
    public Text levelText;
    public Text hpText;
    public Text mpText;

    PlayerCtrl player;
    public GameObject pauseCanvas;

    public bool isPause;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
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

        scoreText.text = "Score : " + player.score;
        levelText.text = "Level : " + player.level;
        hpText.text = player.hp + " / " + player.hpMax;
        mpText.text = player.mp + " / " + player.mpMax;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
