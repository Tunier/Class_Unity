using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButtonCtrl : MonoBehaviour
{
    public void OnclickStart()
    {
        LoadingSceneController.Instance.LoadScene("Game_Field_Scene");
    }
    public void OnClickExit()
    {
        Application.Quit();
    }
}
