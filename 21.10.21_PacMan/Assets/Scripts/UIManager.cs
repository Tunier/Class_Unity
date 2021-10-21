using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text highScoreText;
    [SerializeField] Text curScoreText;
    [SerializeField] Player player;

    [SerializeField] GameObject EndCanvas;
    [SerializeField] Text EndText;

    void Update()
    {
        highScoreText.text = $"{Stage_Manager.Instance.highScore}";
        curScoreText.text = $"{player.score}";

        if (player.isDie)
            EndText.text = "You Lose!!!";
        else if (Stage_Manager.Instance.isClear)
            EndText.text = "Clear!!!";

        EndCanvas.SetActive(player.isDie || Stage_Manager.Instance.isClear);
    }

    public void ExitBtnClick()
    {
        Application.Quit();
    }

    public void RestartBtnClick()
    {
        SceneManager.LoadScene(0);
    }
}
