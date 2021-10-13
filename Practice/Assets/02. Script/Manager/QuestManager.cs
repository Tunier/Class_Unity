using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public string UIDCODE;
    public string Title;
    public string Desc;
    public int State; //(0:미수락, 1:진행중, 2:조건완료, 3:퀘스트완료)
}

public class QuestManager : MonoSingletone<QuestManager>
{
    public GameObject questPanel;
    public GameObject go_QuestText;

    public Dictionary<string, Quest> QuestDic = new Dictionary<string, Quest>(); // 퀘스트UID, 퀘스트

    Quest quest1 = new Quest();
    Quest quest2 = new Quest();
    Quest quest3 = new Quest();

    public int quest1_Count = 0;
    public int quest2_Count = 0;
    public int quest3_Count = 0;


    private void Awake()
    {
        quest1.UIDCODE = "001";
        quest1.Title = "돼지고기 구하기";
        quest1.Desc = "리틀보어 10마리 잡기.";

        quest2.UIDCODE = "002";
        quest2.Title = "고블린 혼내주기";
        quest2.Desc = "고블린 궁수 10마리 잡기.";

        quest3.UIDCODE = "003";
        quest3.Title = "고블린 킹 죽이기";
        quest3.Desc = "고블린 킹 1마리 잡기.";

        QuestDic.Add(quest1.UIDCODE, quest1);
        QuestDic.Add(quest2.UIDCODE, quest2);
        QuestDic.Add(quest3.UIDCODE, quest3);
    }

    private void Update()
    {
        if (QuestDic["001"].State == 1)
        {
            if (quest1_Count >= 10)
                QuestDic["001"].State = 2;
        }

        if (QuestDic["002"].State == 1)
        {
            if (quest2_Count >= 10)
                QuestDic["002"].State = 2;
        }

        if (QuestDic["003"].State == 1)
        {
            if (quest3_Count >= 1)
                QuestDic["003"].State = 2;
        }
    }

    public void AddQuestInPanel(Quest _quest)
    {
        var obj = Instantiate(go_QuestText, questPanel.transform);
        obj.name = _quest.Title + " QuestText";

        //obj.transform.SetParent(questPanel.transform);

        var questText = obj.GetComponent<QuestTextUI>();

        questText.UIDCODE = _quest.UIDCODE;
        questText.SetTitleText(_quest.Title);
        questText.SetDescText(_quest.Desc);
    }

    /// <summary>
    /// 퀘스트의 UIDCODE를 넣어주면 퀘스트 패널에 추가되고, 퀘스트의 상태를 진행중으로 바꿔준다.<br/>
    /// 또한 퀘스트 패널을 열어준다.
    /// </summary>
    /// <param name="_UIDCODE"></param>
    public void GetQuest(string _UIDCODE)
    {
        AddQuestInPanel(QuestDic[_UIDCODE]);
        QuestDic[_UIDCODE].State = 1;

        //Debug.Log(QuestDic[_UIDCODE].Title + " 퀘스트의 상태가 " + QuestDic[_UIDCODE].State + "가 되었습니다.");

        UIManager.Instance.QuestUI.GetComponent<RectTransform>().localPosition = new Vector2(750, 85);
    }
}