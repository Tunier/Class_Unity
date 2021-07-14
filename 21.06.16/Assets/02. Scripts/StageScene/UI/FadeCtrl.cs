using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeCtrl : MonoBehaviour
{
    [SerializeField]
    GameObject fadeObj;

    [SerializeField]
    Image fadeImage;
    
    Color alpha;

    public bool endFadeOut;
    public bool endFadeIn;


    void Start()
    {
        endFadeOut = false;
        endFadeIn = false;
    }

    void Update()
    {

    }

    public void StartFade()
    {
        if (!fadeObj.activeSelf)
        {
            alpha.a = 0f;
            fadeImage.color = alpha;
            fadeObj.SetActive(true);
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut()
    {
        while (!endFadeOut)
        {
            alpha.a += 0.01f;
            fadeImage.color = alpha;

            if (alpha.a >= 1f)
            {
                endFadeOut = true;
            }

            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator FadeIn()
    {
        while (!endFadeIn)
        {
            alpha.a -= 0.01f;
            fadeImage.color = alpha;

            if (alpha.a <= 0f)
            {
                endFadeIn = true;
            }

            yield return new WaitForSeconds(0.01f);
        }
    }

    void SetActiveFalse()
    {
        fadeObj.SetActive(false);
    }
}
