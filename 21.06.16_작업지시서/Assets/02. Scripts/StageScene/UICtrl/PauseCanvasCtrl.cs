using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseCanvasCtrl : MonoBehaviour
{
    GameManager gm;

    private void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void OnclickResume()
    {
        gm.isPause = false;
    }

    public void OnclickOption()
    {
        print("Option");
    }

    public void OnClickExit()
    {
#if UNITY_EDITOR // 에디터에서만 실행되는 코드 
        //UnityEditor.EditorApplication.isPlaying = false; // 에디터의 플레이 모드를 중단
        SceneManager.LoadScene("StartMenu");
#else // 빌드된 게임에서 실행되는 코드 
        Application.Quit();
#endif
    }
}
