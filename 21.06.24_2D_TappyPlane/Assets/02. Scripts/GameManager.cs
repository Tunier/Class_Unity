using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    PlayerScr player;

    public GameObject GameOverImage;

    bool isGameOver = false;

    void Start()
    {
        player = GameObject.Find("Plane").GetComponent<PlayerScr>();
    }

    void Update()
    {
        if (isGameOver == true)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene("GameScene");
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    public void gameOverFunc()
    {
        Time.timeScale = 0;
        GameOverImage.SetActive(true);
        isGameOver = true;
    }
}
