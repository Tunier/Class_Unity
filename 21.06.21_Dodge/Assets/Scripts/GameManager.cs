using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameObject gameoverText;
    public GameObject HpText;
    public Text timeText;
    public Text recordText;

    private float surviveTime;
    private bool isGameover;

    PlayerCtrl player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        //else
        //{
        //    if (instance != this)
        //        Destroy(this.gameObject);
        //}
    }

    void Start()
    {
        surviveTime = 0f;
        isGameover = false;
        player = GameObject.FindWithTag("Player").GetComponent<PlayerCtrl>();
    }

    void Update()
    {
        if (!isGameover)
        {
            surviveTime += Time.deltaTime;
            timeText.text = "Time : " + surviveTime.ToString("F2");
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("SampleScene");
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
        HpText.GetComponent<Text>().text = "Hp : " + player.hp;
    }

    public void GameOver()
    {
        isGameover = true;
        gameoverText.SetActive(true);
        HpText.SetActive(false);

        float bestTime = PlayerPrefs.GetFloat("BestTime");

        // PlayerPrefs : 유니티가 실행되는 동안 발생한 데이터 일부를 영구적으로 저장해주는 클래스
        // float, int, string형태를 저장할수있다.

        if (surviveTime > bestTime)
        {
            bestTime = surviveTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
            PlayerPrefs.Save();
        }

        recordText.text = "BestTime : " + bestTime.ToString("F2");
    }
}
