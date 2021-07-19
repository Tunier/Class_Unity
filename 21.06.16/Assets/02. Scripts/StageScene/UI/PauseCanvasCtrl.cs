using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseCanvasCtrl : MonoBehaviour
{
    GameManager gm;

    [SerializeField]
    GameObject pauseMenuPanel;

    [SerializeField]
    GameObject optionMenuPanel;

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
        pauseMenuPanel.SetActive(false);
        optionMenuPanel.SetActive(true);
    }

    public void OnclickOptionExit()
    {
        optionMenuPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
    }

    public void OnClickExit()
    {
#if UNITY_EDITOR // �����Ϳ����� ����Ǵ� �ڵ� 
        UnityEditor.EditorApplication.isPlaying = false; // �������� �÷��� ��带 �ߴ�
        //SceneManager.LoadScene("StartMenu");
#else // ����� ���ӿ��� ����Ǵ� �ڵ� 
        Application.Quit();
#endif
    }
}
