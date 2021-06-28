using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    PlayerScr player;

    public GameObject GameOverImage;
    public Image[] image;

    bool isGameOver = false;

    public float highScore;
    public Text highScroeText;
    public float score;
    public Text scoreText;

    public int digit_score;

    void Start()
    {
        player = GameObject.Find("Plane").GetComponent<PlayerScr>();
        score = 0;
        highScore = PlayerPrefs.GetFloat("highScore");
        highScroeText.text = "HighScore : " + highScore.ToString("f2");
    }

    void Update()
    {
        score += Time.deltaTime;
        scoreText.text = "Score : " + (int)score;

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetFloat("highScore", score);
            PlayerPrefs.Save();
            highScroeText.text = "HighScore : " + highScore.ToString("f2");
        }

        digit_score = (int)score;

        int n1000 = digit_score / 1000;
        int n100 = (digit_score % 1000) / 100;
        int n10 = (digit_score % 100) / 10;
        int n1 = digit_score % 10;

        string fileName = "Numbers/number";
        image[0].sprite = Resources.Load<Sprite>(fileName + n1000);
        image[1].sprite = Resources.Load<Sprite>(fileName + n100);
        image[2].sprite = Resources.Load<Sprite>(fileName + n10);
        image[3].sprite = Resources.Load<Sprite>(fileName + n1);

        for (int i = 0; i < 4; i++)
            image[i].SetNativeSize();

        int j = 50;
        string str = "image" + j;
        str = string.Format("image{0:D4}", j); // 0050으로 바뀜.
        // string.Format : 문자열의 포멧을 바꿀수있음.
        // https://m.blog.naver.com/PostView.naver?isHttpsRedirect=true&blogId=pxkey&logNo=221321776845 참고.

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

        PlayerPrefs.Save();
    }

    public void gameOverFunc()
    {
        Time.timeScale = 0;
        GameOverImage.SetActive(true);
        isGameOver = true;
    }
}
