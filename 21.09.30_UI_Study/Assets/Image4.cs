using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Image4 : MonoBehaviour
{
    public Image Background;
    public Image curser;

    public bool rotateMark = true;

    void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, -360 * Background.fillAmount);

        if (rotateMark)
            curser.transform.localEulerAngles = Vector3.zero;
        else
            curser.transform.localEulerAngles = -transform.localEulerAngles;
    }
}