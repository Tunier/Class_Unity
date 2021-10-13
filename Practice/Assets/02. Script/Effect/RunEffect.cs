using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunEffect : MonoBehaviour
{
    float effectOverTime = 0;

    private void OnEnable()
    {
        effectOverTime = 0;
    }

    private void Update()
    {
        effectOverTime += Time.deltaTime;

        if (effectOverTime >= 1f)
        {
            gameObject.SetActive(false);
        }
    }
}
