using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public Transform[] points;
    public GameObject enemy;

    public float creatTime = 2f;
    public int maxEnemy = 10;
    public bool isGameOver = false;

    [Header("오브젝트풀 정보")]
    public GameObject bulletPrefeb;
    int maxPool = 10;
    public List<GameObject> bulletpool = new List<GameObject>();

    [Header("UI관련")]
    public Image weaponButtonImg;
    bool isPaused;

    public CanvasGroup inventoryCG;

    [Header("데이터 저장")]
    public Text killConutTxt;
    int killCount;

    void LoadGameData()
    {
        killCount = PlayerPrefs.GetInt("KILL_COUNT", 0);
        killConutTxt.text = "KILL " + killCount.ToString("0000");
    }

    public void OnPauseClick()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        var PlayerObj = GameObject.FindGameObjectWithTag("PLAYER");
        var scripts = PlayerObj.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            script.enabled = !isPaused;
        }

        var canvasGroup = GameObject.Find("Panel-Weapon").GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = !isPaused;
    }

    public void OnInventoryOpen(bool _isOpened)
    {
        inventoryCG.alpha = _isOpened ? 1f : 0f;
        inventoryCG.interactable = _isOpened;
        inventoryCG.blocksRaycasts = _isOpened;
    }

    public void OnCloseButtonClick()
    {
        inventoryCG.alpha = 0;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
                Destroy(gameObject);
        }

        LoadGameData();

        CreatePooling();
    }

    public void IncKillCount()
    {
        killCount++;
        killConutTxt.text = "KILL " + killCount.ToString("0000");
        PlayerPrefs.SetInt("KILL_COUNT", killCount);
        PlayerPrefs.Save();
    }

    void Start()
    {
        points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();

        if (points.Length > 0)
        {
            StartCoroutine(CreateEnemy());
        }
    }

    public GameObject GetBullet()
    {
        foreach (GameObject bullet in bulletpool)
        {
            if (bullet.activeSelf == false)
            {
                return bullet;
            }
        }
        return null;
    }

    public void CreatePooling()
    {
        GameObject ObjectPools = new GameObject("ObjectPools");

        for (int i = 0; i < maxPool; i++)
        {
            var obj = Instantiate<GameObject>(bulletPrefeb, ObjectPools.transform);

            obj.name = "Bullet_" + i.ToString("00");
            obj.SetActive(false);
            bulletpool.Add(obj);
        }
    }

    IEnumerator CreateEnemy()
    {
        while (!isGameOver)
        {
            int enemyCount = (int)GameObject.FindGameObjectsWithTag("ENEMY").Length;

            if (enemyCount < maxEnemy)
            {
                yield return new WaitForSeconds(creatTime);

                int idx = Random.Range(1, points.Length);
                Instantiate(enemy, points[idx].position, points[idx].rotation);
            }
        }
    }
}
