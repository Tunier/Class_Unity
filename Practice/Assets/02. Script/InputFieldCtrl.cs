using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldCtrl : MonoBehaviour
{
    public InputField inputField;

    private void OnEnable()
    {
        inputField.placeholder.GetComponent<Text>().text = "1";
    }
    private void OnDisable()
    {
        inputField.text = "1";
    }
}


