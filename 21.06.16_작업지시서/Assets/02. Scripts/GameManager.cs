using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public MonsterCtrl mob;
    public GameObject pauseCanvas;

    public bool isPause;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        isPause = false;
    }

    private void Update()
    {
        PlayerPrefs.Save();
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = true;
            //UnityEditor.EditorApplication.isPlaying = false;
        }
        
        if (isPause)
        {
            Time.timeScale = 0f;
            pauseCanvas.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            pauseCanvas.SetActive(false);
        }
    }
}
