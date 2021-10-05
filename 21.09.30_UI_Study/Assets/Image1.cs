using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Image1 : MonoBehaviour
{
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.fillClockwise = true;
        image.fillAmount = 0;
    }

    void Update()
    {
        if (image.fillAmount <= 1)
            image.fillAmount += 0.1f * Time.deltaTime;
    }
}
