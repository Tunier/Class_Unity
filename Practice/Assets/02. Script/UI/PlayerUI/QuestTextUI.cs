using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestTextUI : MonoBehaviour
{
    public string UIDCODE;
    public Text titleText;
    public Text descText;
    public Text stateText;

    public void SetTitleText(string _text)
    {
        titleText.text = _text;
    }

    public void SetDescText(string _text)
    {
        descText.text = _text;
    }

    private void Update()
    {
        switch (UIDCODE)
        {
            case "001":
                stateText.text = "리틀보어 " + QuestManager.Instance.quest1_Count + " / 10";
                break;
            case "002":
                stateText.text = "고블린 궁수 " + QuestManager.Instance.quest2_Count + " / 10";
                break;
            case "003":
                stateText.text = $"고블린 킹 {QuestManager.Instance.quest3_Count} / 1";
                break;
            default:
                stateText.text = "";
                break;
        }

        if (QuestManager.Instance.QuestDic[UIDCODE].State == 3 || QuestManager.Instance.QuestDic[UIDCODE].State == 0)
        {
            Destroy(gameObject);
        }
    }
}
