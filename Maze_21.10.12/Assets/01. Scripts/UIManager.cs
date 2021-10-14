using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoSingletone<UIManager>
{
    [SerializeField]
    Image timeImage;
    [SerializeField]
    TextMeshProUGUI timeText;
    [SerializeField]
    Text scoreText;
    [SerializeField]
    Text curScoreText;

    [SerializeField]
    Toggle minimapToggle;
    [SerializeField]
    GameObject minimap;

    [SerializeField]
    GameObject pauseCanvas;
    [SerializeField]
    GameObject clearCanvas;
    [SerializeField]
    GameObject gameOverCanvas;

    [SerializeField]
    InputField infld;

    [SerializeField]
    Text highScoreUserNameText;
    [SerializeField]
    Text highScoreText;

    void Start()
    {
        if (PlayerPrefs.HasKey("HighScoreUserName"))
        {
            highScoreUserNameText.text = PlayerPrefs.GetString("HighScoreUserName");
            highScoreText.text = $"{PlayerPrefs.GetFloat("HighScore"):0}";
        }
        else
        {
            highScoreUserNameText.text = "Tunier";
            highScoreText.text = "15";
        }
    }

    void Update()
    {
        timeImage.fillAmount = GameManager.Instance.limitTime / 30;
        timeText.text = $"{GameManager.Instance.limitTime:0.00}";
        scoreText.text = $"Score : {GameManager.Instance.limitTime:0}";
        curScoreText.text = $"{GameManager.Instance.limitTime:0}";

        minimap.gameObject.SetActive(minimapToggle.isOn);

        if (!GameManager.Instance.isClear && !GameManager.Instance.isGameover)
            pauseCanvas.SetActive(GameManager.Instance.isPause);
        else
        {
            clearCanvas.SetActive(GameManager.Instance.isClear);
            pauseCanvas.SetActive(false);
        }

        gameOverCanvas.SetActive(GameManager.Instance.isGameover);
    }

    public void PauseBtnClick()
    {
        GameManager.Instance.isPause = !GameManager.Instance.isPause;
    }

    public void StartSceneBtnClick()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void ExitGameBtnClick()
    {
        Application.Quit();
    }

    public void SaveBtnClick()
    {
        if (PlayerPrefs.GetFloat("HighScore") < GameManager.Instance.limitTime)
        {
            PlayerPrefs.SetFloat("HighScore", GameManager.Instance.limitTime);
            PlayerPrefs.SetString("HighScoreUserName", infld.text);
        }

        SceneManager.LoadScene("StartScene");
    }
}
