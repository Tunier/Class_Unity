using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputNumberUI : MonoBehaviour
{
    bool activated = false;

    [SerializeField]
    Text previewText;
    [SerializeField]
    Text inputText;
    [SerializeField]
    InputField IFtext;

    [SerializeField]
    GameObject inputUI;

    GameObject player;


    void Start()
    {
        player = GameObject.FindWithTag("PLAYER");
    }

    void Update()
    {
        if (activated)
        {
            if (Input.GetKeyDown(KeyCode.Return))
                OK();
            else if (Input.GetKeyDown(KeyCode.Escape))
                Cancel();
        }
    }

    public void Call()
    {
        inputUI.SetActive(true);
        activated = true;
        IFtext.text = DragSlot.instance.dragSlot.itemCount.ToString();
    }

    public void Cancel()
    {
        activated = false;
        DragSlot.instance.SetColorAlpha(0);
        inputUI.SetActive(false);
        DragSlot.instance.dragSlot = null;
    }

    public void OK()
    {
        DragSlot.instance.SetColorAlpha(0);

        int num;
        if (inputText.text != "")
        {
            if (CheckNumber(inputText.text))
            {
                num = int.Parse(inputText.text); // 문자열을 부호있는 숫자로 변환.

                if (num > DragSlot.instance.dragSlot.itemCount)
                    num = DragSlot.instance.dragSlot.itemCount;
            }
            else
                num = 0;
        }
        else
            num = int.Parse(previewText.text);

        StartCoroutine(DropItemCoruntine(num));
    }

    IEnumerator DropItemCoruntine(int num)
    {
        for (int i = 0; i < num; i++)
        {
            var obj = Instantiate(DragSlot.instance.dragSlot.item.itemPrefab, player.transform.position, Quaternion.identity);
            DragSlot.instance.dragSlot.SetSlotCount(-1);
            yield return new WaitForSeconds(0.1f);
        }
        DragSlot.instance.dragSlot = null;
        inputUI.SetActive(false);
        activated = false;
    }

    private bool CheckNumber(string _string)
    {
        char[] _charArray = _string.ToCharArray();
        bool isNumber = true;

        for (int i = 0; i < _charArray.Length; i++)
        {
            if (_charArray[i] >= 48 && _charArray[i] <= 57)
                continue;
            isNumber = false;
        }
        return isNumber;
    }
}
