using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public List<GameObject> mobList;
    public GameObject mobSpawn;

    public GameObject pauseCanvas;
    public GameObject statusUI;

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
                Destroy(gameObject);
        }
    }
    private void Start()
    {
        isPause = false;
        mobList = new List<GameObject>();
    }

    private void Update()
    {
        PlayerPrefs.Save();

        PauseGame();
        ControlMonsterSpawn();

        if (Input.GetKeyDown(KeyCode.N))
        {
            statusUI.SetActive(!statusUI.activeSelf);
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

    public void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = !isPause;
            //UnityEditor.EditorApplication.isPlaying = false;
        }
    }

    public void ControlMonsterSpawn()
    {
        if (mobList.Count >= 3)
        {
            mobSpawn.SetActive(false);
        }
        else
        {
            mobSpawn.SetActive(true);
        }
    }

    public void OnPauseClick()
    {
        isPause = !isPause;
    }
}
