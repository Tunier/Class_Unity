using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingletone<GameManager>
{
    [HideInInspector]
    public float limitTime = 30f;

    bool isGameover = false;

    [HideInInspector]
    public bool isClear = false;
    [HideInInspector]
    public bool isPause = false;

    void Update()
    {
        limitTime = limitTime > 0 ? limitTime -= Time.deltaTime : 0;

        if (limitTime <= 0)
            isGameover = true;

        Time.timeScale = isPause ? 0 : 1;
    }
}
