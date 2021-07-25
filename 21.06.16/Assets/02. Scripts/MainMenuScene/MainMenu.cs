using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnclickStart()
    {
        SceneManager.LoadScene("01. Stage");
    }

    public void OnclickOption()
    {
        print("Option");
    }

    public void OnClickExit()
    {
#if UNITY_EDITOR // �����Ϳ����� ����Ǵ� �ڵ� 
        UnityEditor.EditorApplication.isPlaying = false; // �������� �÷��� ��带 �ߴ�
#else // ����� ���ӿ��� ����Ǵ� �ڵ� 
        Application.Quit();
#endif
    }

}
