using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputNumberUI : MonoBehaviour, IPointerClickHandler
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

    Vector3 offset;

    void Start()
    {
        player = GameObject.FindWithTag("PLAYER");
        offset = new Vector3(0, -25f, 0);
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
        transform.position = Input.mousePosition + offset;
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

    public IEnumerator DropItemCoruntine(int num)
    {
        inputUI.SetActive(false);
        activated = false;

        for (int i = 0; i < num; i++)
        {
            Instantiate(DragSlot.instance.dragSlot.item.itemPrefab, player.transform.position, Quaternion.identity);
            
            DragSlot.instance.dragSlot.SetSlotCount(-1);
            
            yield return new WaitForSeconds(0.1f);
        }
        DragSlot.instance.dragSlot = null;
    }

    private bool CheckNumber(string _string)
    {
        char[] Array = _string.ToCharArray();
        bool isNumber = true;

        for (int i = 0; i < Array.Length; i++)
        {
            if (Array[i] >= 48 && Array[i] <= 57)
                continue;
            isNumber = false;
        }
        return isNumber;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }
}
