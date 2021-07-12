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
    public GameObject inventoryUI;

    [SerializeField]
    RectTransform invenBase;
    [SerializeField]
    RectTransform statusBase;

    [SerializeField]
    SlotToolTip toolTip;

    public GameObject dieText;

    public PlayerCtrl player;

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

        player = GameObject.FindWithTag("PLAYER").GetComponent<PlayerCtrl>();
    }
    private void Start()
    {
        isPause = false;
        mobList = new List<GameObject>();
    }

    private void Update()
    {
        PlayerPrefs.Save();

        UIHotKey();
        OnOffMonsterSpawn();

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

        if (player.state == PlayerCtrl.State.DIE)
        {
            dieText.SetActive(true);
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("Stage");
            }
        }
        else
        {
            dieText.SetActive(false);
        }
    }

    void UIHotKey()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);

            if (!inventoryUI.activeSelf && RectTransformUtility.RectangleContainsScreenPoint(invenBase, toolTip.baseImage.transform.position - toolTip.offset))
                toolTip.HideToolTip();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            statusUI.SetActive(!statusUI.activeSelf);

            if (!statusUI.activeSelf && RectTransformUtility.RectangleContainsScreenPoint(statusBase, toolTip.baseImage.transform.position - toolTip.offset))
                toolTip.HideToolTip();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = !isPause;
        }

        if (Input.GetKeyDown(KeyCode.F12))
        {
            isPause = !isPause;
        }
    }

    public void OnOffMonsterSpawn()
    {
        if (mobList.Count >= 4)
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

    public void OnStatusBottonClick()
    {
        statusUI.SetActive(!statusUI.activeSelf);
    }

    public void OnInventoryBottonClick()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
    }
}
