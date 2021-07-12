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
#if UNITY_EDITOR // �����Ϳ����� ����Ǵ� �ڵ� 
        //UnityEditor.EditorApplication.isPlaying = false; // �������� �÷��� ��带 �ߴ�
        SceneManager.LoadScene("StartMenu");
#else // ����� ���ӿ��� ����Ǵ� �ڵ� 
        Application.Quit();
#endif
    }
}
