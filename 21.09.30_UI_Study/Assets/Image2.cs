using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Image2 : MonoBehaviour
{
    Image image;

    bool isCharge = true;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.fillClockwise = true;
        image.fillAmount = 0;
    }

    void Update()
    {
        if (image.fillAmount < 1 && isCharge)
            image.fillAmount += 0.1f * Time.deltaTime;
        else if (image.fillAmount >= 1)
        {
            isCharge = false;
            image.fillAmount -= 0.1f * Time.deltaTime;
        }
        else if (image.fillAmount > 0)
        {
            image.fillAmount -= 0.1f * Time.deltaTime;
        }
        else if (image.fillAmount <= 0)
        {
            isCharge = true;
            image.fillAmount += 0.1f * Time.deltaTime;
        }
    }
}