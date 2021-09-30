using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScr : MonoBehaviour
{
    Text text;

    void Awake()
    {
        text = GetComponent<Text>();
        text.text = "Abc";
        text.text += "<color=#000000>def</color>";
    }

    void Update()
    {

    }
}
