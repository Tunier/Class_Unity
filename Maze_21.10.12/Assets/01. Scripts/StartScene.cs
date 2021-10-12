using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    private void Awake()
    {
        Screen.SetResolution(1920, 1080, true);
    }

    public void StartBtnClick()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitBtnClick()
    {
        Application.Quit();
    }
}
