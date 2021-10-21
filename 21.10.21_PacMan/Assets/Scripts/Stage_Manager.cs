using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_Manager : MonoSingletone<Stage_Manager>
{
    Player player;

    List<Enemy> enemies = new List<Enemy>();
    public List<GameObject> items = new List<GameObject>();

    public int highScore;
    public int PlayerLife = 3;
    public bool isClear = false;

    void Start()
    {
        player = FindObjectOfType<Player>();
        highScore = PlayerPrefs.HasKey("highScore") ? PlayerPrefs.GetInt("highScore") : 0;

        enemies.AddRange(FindObjectsOfType<Enemy>());
        items.AddRange(GameObject.FindGameObjectsWithTag("Item"));
    }

    void Update()
    {
        isClear = items.Count == 0;

        Time.timeScale = isClear || player.isDie ? 0 : 1;

        if (isClear)
        {
            if (PlayerPrefs.HasKey("highScore"))
            {
                if (player.score > PlayerPrefs.GetInt("highScore"))
                    PlayerPrefs.SetInt("highScore", player.score);
            }
            else
                PlayerPrefs.SetInt("highScore", player.score);
        }
    }

    public void ChageAllEnemyState(Enemy.eState _state)
    {
        foreach (var enemy in enemies)
        {
            if (enemy.state != Enemy.eState.Die)
                enemy.state = _state;
        }
    }
}
