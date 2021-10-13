using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    public enum DialogButton
    {
        Left,
        Right,
    }

    public enum ButtonType
    {
        NextBack,
        AcceptRefuse,
        QuestClear,
        conversation,
    }

    public Text leftButtonText;
    public Text rightButtonText;

    public List<string> dialogTextList = new List<string>();
    public Text dialogText;

    public string questUIDCODE; // 스크립트 꽃고 직접 작성.

    int index = 0;
    public ButtonType buttonType = ButtonType.NextBack;

    PlayerInfo player;
    Inventory inven;

    private void Awake()
    {
        player = FindObjectOfType<PlayerInfo>();
        inven = FindObjectOfType<Inventory>();
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        index = 0;
    }

    private void Update()
    {
        if (gameObject.activeSelf && questUIDCODE != "")
        {
            if (QuestManager.Instance.QuestDic[questUIDCODE].State != 2)
            {
                if (dialogTextList.Count != 0)
                    ShowDialogText(index);

                if (index == dialogTextList.Count - 1)
                {
                    SetButtonTextAcceptRefuseType();
                }
            }
            else
            {
                dialogText.text = "퀘스트를 완료했구나!";
                switch (questUIDCODE)
                {
                    case "001":
                        dialogText.text += " 보상은 골드 500, 경험치 250이야";
                        break;
                    case "002":
                        dialogText.text += " 보상은 골드 1000, 경험치 500이야";
                        break;
                    case "003":
                        dialogText.text = "자네는 포폴왕국의 영웅이야! 이 검을 받게";
                        break;
                }
                SetButtonTextQuestClearType();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
            questUIDCODE = "";
            ClearTextList();
        }
    }

    public void SetButtonTextAcceptRefuseType()
    {
        SetButtonText(DialogButton.Left, "거절하기");
        SetButtonText(DialogButton.Right, "수락하기");
        buttonType = ButtonType.AcceptRefuse;
    }

    public void SetButtonTextNextBackType()
    {
        SetButtonText(DialogButton.Left, "이전으로");
        SetButtonText(DialogButton.Right, "다음으로");
        buttonType = ButtonType.NextBack;
    }

    public void SetButtonTextCoversationType()
    {
        SetButtonText(DialogButton.Left, "예");
        SetButtonText(DialogButton.Right, "종료");
        buttonType = ButtonType.conversation;
    }

    public void SetButtonTextQuestClearType()
    {
        SetButtonText(DialogButton.Left, "대화종료");
        SetButtonText(DialogButton.Right, "보상받기");
        buttonType = ButtonType.QuestClear;
    }

    void SetButtonText(DialogButton _button, string _text)
    {
        switch (_button)
        {
            case DialogButton.Left:
                leftButtonText.text = _text;
                break;
            case DialogButton.Right:
                rightButtonText.text = _text;
                break;
            default:
                break;
        }
    }

    public void AddDialogText(string _text)
    {
        dialogTextList.Add(_text);
    }

    void ShowDialogText(int _index)
    {
        dialogText.text = dialogTextList[_index];
    }

    public void ClearTextList()
    {
        dialogTextList.Clear();
    }

    public void LeftButtonClick()
    {
        switch (buttonType)
        {
            case ButtonType.NextBack:
                if (index > 0)
                    index--;
                break;
            case ButtonType.AcceptRefuse:
                gameObject.SetActive(false);
                questUIDCODE = "";
                ClearTextList();
                break;
            case ButtonType.conversation:
                gameObject.SetActive(false);
                questUIDCODE = "";
                ClearTextList();
                break;
            case ButtonType.QuestClear:
                gameObject.SetActive(false);
                questUIDCODE = "";
                ClearTextList();
                break;
        }
    }

    public void RightButtonClick()
    {
        switch (buttonType)
        {
            case ButtonType.NextBack:
                if (index < dialogTextList.Count - 1)
                    index++;
                break;
            case ButtonType.AcceptRefuse:
                if (QuestManager.Instance.QuestDic[questUIDCODE].State == 0)
                {
                    QuestManager.Instance.GetQuest(questUIDCODE);
                    gameObject.SetActive(false);
                }
                else
                {
                    SetButtonTextCoversationType();
                    dialogText.text = "넌이미 퀘스트를 진행중이거나 완료했어.";
                }
                questUIDCODE = "";
                ClearTextList();
                break;
            case ButtonType.conversation:
                gameObject.SetActive(false);
                questUIDCODE = "";
                ClearTextList();
                break;
            case ButtonType.QuestClear:
                QuestManager.Instance.QuestDic[questUIDCODE].State = 3;
                print($"{QuestManager.Instance.QuestDic[questUIDCODE].UIDCODE} : {QuestManager.Instance.QuestDic[questUIDCODE].State}");
                switch (questUIDCODE)
                {
                    case "001":
                        player.GetExp(250);
                        player.GetGold(500);
                        break;
                    case "002":
                        player.GetExp(500);
                        player.GetGold(1000);
                        break;
                    case "003":
                        Item item = ItemDatabase.instance.newItem("0000003");
                        inven.GetItem(item);
                        SystemText_ScrollView_Ctrl.Instance.PrintText(item.Name + " 을 획득했습니다.");
                        break;
                }
                gameObject.SetActive(false);
                questUIDCODE = "";
                ClearTextList();
                break;
        }
    }
}
