using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public string UIDCODE;
    public string Title;
    public string Desc;
    public int State; //(0:�̼���, 1:������, 2:���ǿϷ�, 3:����Ʈ�Ϸ�)
}

public class QuestManager : MonoSingletone<QuestManager>
{
    public GameObject questPanel;
    public GameObject go_QuestText;

    public Dictionary<string, Quest> QuestDic = new Dictionary<string, Quest>(); // ����ƮUID, ����Ʈ

    Quest quest1 = new Quest();
    Quest quest2 = new Quest();
    Quest quest3 = new Quest();

    public int quest1_Count = 0;
    public int quest2_Count = 0;
    public int quest3_Count = 0;


    private void Awake()
    {
        quest1.UIDCODE = "001";
        quest1.Title = "������� ���ϱ�";
        quest1.Desc = "��Ʋ���� 10���� ���.";

        quest2.UIDCODE = "002";
        quest2.Title = "��� ȥ���ֱ�";
        quest2.Desc = "��� �ü� 10���� ���.";

        quest3.UIDCODE = "003";
        quest3.Title = "��� ŷ ���̱�";
        quest3.Desc = "��� ŷ 1���� ���.";

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
    /// ����Ʈ�� UIDCODE�� �־��ָ� ����Ʈ �гο� �߰��ǰ�, ����Ʈ�� ���¸� ���������� �ٲ��ش�.<br/>
    /// ���� ����Ʈ �г��� �����ش�.
    /// </summary>
    /// <param name="_UIDCODE"></param>
    public void GetQuest(string _UIDCODE)
    {
        AddQuestInPanel(QuestDic[_UIDCODE]);
        QuestDic[_UIDCODE].State = 1;

        //Debug.Log(QuestDic[_UIDCODE].Title + " ����Ʈ�� ���°� " + QuestDic[_UIDCODE].State + "�� �Ǿ����ϴ�.");

        UIManager.Instance.QuestUI.GetComponent<RectTransform>().localPosition = new Vector2(750, 85);
    }
}