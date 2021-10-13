using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpEffect : MonoBehaviour
{
    float effectOverTime = 0;

    private void OnEnable()
    {
        effectOverTime = 0;
    }

    private void Update()
    {
        effectOverTime += Time.deltaTime;

        if (effectOverTime >= 1.5f)
        {
            gameObject.SetActive(false);
        }
    }
}
