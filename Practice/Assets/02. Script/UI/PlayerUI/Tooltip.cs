using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public GameObject go_Tooltip;
    public RectTransform invenRect;
    public RectTransform shopRect;

    [SerializeField]
    Text NameText;
    [SerializeField]
    Text TypeText;
    [SerializeField]
    Text LevelText;
    [SerializeField]
    Text SkillCostText;
    [SerializeField]
    Text BaseValueText;
    [SerializeField]
    Text RequireText;
    [SerializeField]
    Text EffectText;
    [SerializeField]
    Text CostText;
    [SerializeField]
    GameObject[] Divider;

    Vector3 Offset;

    [SerializeField]
    RectTransform tooltip_Rect;

    bool wantActive = false;
    float activeOverTime = 0f;

    PlayerInfo player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerInfo>();
    }

    void Start()
    {
        go_Tooltip.SetActive(false);
        //CostText.gameObject.SetActive(false);

        #region ������ ���� üũ (����׿�)
        //Item _testItem = ItemDatabase.instance.newItem("0000008");
        //#region ������ ��ųʸ� Ű üũ (����׿�)
        ////List<int> Keys = new List<int>();
        ////Keys.AddRange(_testItem.itemEffect.ValueDic.Keys);
        ////Debug.Log(Keys[0]);
        //#endregion
        //ShowTooltip(_testItem);
        #endregion
    }

    void Update()
    {
        if (wantActive)
        {
            go_Tooltip.SetActive(true);

            if (Input.mousePosition.x + tooltip_Rect.rect.width >= 1920)
            {
                Offset.x = 1920 - Input.mousePosition.x - tooltip_Rect.rect.width * 0.5f;
            }
            else
            {
                Offset.x = tooltip_Rect.rect.width * 0.5f;
            }

            if (Input.mousePosition.y - tooltip_Rect.rect.height <= 0)
            {
                //Offset.y = -(Input.mousePosition.y - tooltip_Rect.rect.height * 0.5f); // �Ʒ������� �� �°�
                Offset.y = tooltip_Rect.rect.height * 0.5f; // �������� ��ȯ
                //Offset.y = 0; // �߰����� �߰�
            }
            else
            {
                Offset.y = -tooltip_Rect.rect.height * 0.5f;
            }

            activeOverTime += Time.deltaTime;

            if (activeOverTime >= 0.15f)
                go_Tooltip.transform.position = Input.mousePosition + Offset;
        }
    }

    /// <summary>
    /// �������� ������ �޾Ƽ� ������ �ٲ��, ���� UI�� ������.
    /// </summary>
    /// <param name="_item"></param>
    public void ShowTooltip(Item _item)
    {
        // ������ Ÿ��
        // 0: �������ݹ���, 1: �μչ���, 2: ���, 3: ���� , 4: ��Ʈ, 5:�尩
        // 6: �Ź�, 7: �����, 8: ����, 9: �Һ�, 10: ���

        // ������ ����Ʈ Ÿ��
        // 0: ����, 1: ���� Hp, 2: ���� Mp, 3: �ִ� Hp ������, 4: �ִ� Hp %��, 5: �ִ� Mp ������, 6: �ִ� Mp %��,
        // 7: ���� ���ݷ� ������, 8: ���� ���ݷ� %��, 9: ���� ���� ������, 10: ���� ���� %��, 11: �� ������, 12: �� %��,
        // 13: ���� ������, 14: ���� %��, 15: ���ݽ� ����� ȸ�� ������, 16: ���ݽ� �������� %��ŭ ����� ȸ��,
        // 17: ���� ���ݷ� ������, 18: ���� ���ݷ� %��, 19: ���� ���� ������, 20: ���� ���� %��

        LevelText.gameObject.SetActive(false);
        SkillCostText.gameObject.SetActive(false);

        #region ������ �̸�
        NameText.text = _item.Name;

        switch (_item.Rarity)
        {
            case 0:
                NameText.color = Color.white;
                break;
            case 1:
                ColorUtility.TryParseHtmlString("#ABA3FF", out Color color_Rare);
                NameText.color = color_Rare;
                break;
            case 2:
                ColorUtility.TryParseHtmlString("#DDD04A", out Color color_Unique);
                NameText.color = color_Unique;
                break;
            case 3:
                ColorUtility.TryParseHtmlString("#C5804F", out Color color_Epic);
                NameText.color = color_Epic;
                break;
            case 4:
                ColorUtility.TryParseHtmlString("#3BC1AF", out Color color_Set);
                NameText.color = color_Set;
                break;
            default:
                //NameText.color = 
                break;
        }
        #endregion

        #region ������ Ÿ��
        switch (_item.Type)
        {
            case 0:
                TypeText.text = "�Ѽչ���";
                break;
            case 1:
                TypeText.text = "�μչ���";
                break;
            case 2:
                TypeText.text = "����";
                break;
            case 3:
                TypeText.text = "���";
                break;
            case 4:
                TypeText.text = "��Ʈ";
                break;
            case 5:
                TypeText.text = "�尩";
                break;
            case 6:
                TypeText.text = "�Ź�";
                break;
            case 7:
                TypeText.text = "�����";
                break;
            case 8:
                TypeText.text = "����";
                break;
            case 9:
                TypeText.text = "�Һ�";
                break;
            case 10:
                TypeText.text = "���";
                break;
            default:
                TypeText.text = "������ Ÿ�� ����";
                break;
                #region ������
                //case 0:
                //    TypeText.text = "OneHandWeapon";
                //    break;
                //case 1:
                //    TypeText.text = "TwoHandWeapon";
                //    break;
                //case 2:
                //    TypeText.text = "Armor";
                //    break;
                //case 3:
                //    TypeText.text = "Helmet";
                //    break;
                //case 4:
                //    TypeText.text = "Belt";
                //    break;
                //case 5:
                //    TypeText.text = "Gloves";
                //    break;
                //case 6:
                //    TypeText.text = "Boots";
                //    break;
                //case 7:
                //    TypeText.text = "Necklace";
                //    break;
                //case 8:
                //    TypeText.text = "Ring";
                //    break;
                //case 9:
                //    TypeText.text = "Used";
                //    break;
                //case 10:
                //    TypeText.text = "Material";
                //    break;
                //default:
                //    TypeText.text = "������ Ÿ�� ����";
                //    break;
                #endregion
        }
        #endregion

        #region ������ ���̽� �� �ؽ�Ʈ
        switch (_item.Type)
        {
            case 0:
                BaseValueText.text = string.Format("���� ���ݷ� : <color=#ABA3FF><b>{0}</b></color>", _item.itemEffect.ValueDic[7]);
                BaseValueText.gameObject.SetActive(true);
                break;
            case 1:
                BaseValueText.text = string.Format("���� ���ݷ� : <color=#ABA3FF><b>{0}</b></color>", _item.itemEffect.ValueDic[7]);
                BaseValueText.gameObject.SetActive(true);
                break;
            case 2:
                BaseValueText.text = string.Format("���� : <color=#ABA3FF><b>{0}</b></color>", _item.itemEffect.ValueDic[9]);
                BaseValueText.gameObject.SetActive(true);
                break;
            case 3:
                BaseValueText.text = string.Format("���� : <color=#ABA3FF><b>{0}</b></color>", _item.itemEffect.ValueDic[9]);
                BaseValueText.gameObject.SetActive(true);
                break;
            case 4:
                BaseValueText.text = string.Format("���� : <color=#ABA3FF><b>{0}</b></color>", _item.itemEffect.ValueDic[9]);
                BaseValueText.gameObject.SetActive(true);
                break;
            case 5:
                BaseValueText.text = string.Format("���� : <color=#ABA3FF><b>{0}</b></color>", _item.itemEffect.ValueDic[9]);
                BaseValueText.gameObject.SetActive(true);
                break;
            case 6:
                BaseValueText.text = string.Format("���� : <color=#ABA3FF><b>{0}</b></color>", _item.itemEffect.ValueDic[9]);
                BaseValueText.gameObject.SetActive(true);
                break;
            case 7:
                BaseValueText.text = string.Format("�ִ� ����� <color=#ABA3FF><b>{0}</b></color> ����", _item.itemEffect.ValueDic[3]);
                BaseValueText.gameObject.SetActive(true);
                break;
            case 8:
                BaseValueText.text = string.Format("�ִ� ���� <color=#ABA3FF><b>{0}</b></color> ����", _item.itemEffect.ValueDic[5]);
                BaseValueText.gameObject.SetActive(true);
                break;
            case 9:
                BaseValueText.gameObject.SetActive(false);
                break;
            case 10:
                BaseValueText.gameObject.SetActive(false);
                break;
        }
        #endregion

        #region ������ �ʿ�����
        string[] str = new string[64];
        List<int> keys = new List<int>();
        keys.AddRange(_item.itemEffect.RequireValueDic.Keys);

        if (keys.Count == 1 && keys[0] == 0)
        {
            RequireText.gameObject.SetActive(false);
            Divider[1].SetActive(false);
            //foreach (GameObject obj in Divider)
            //    obj.SetActive(false);
        }
        else
        {
            RequireText.gameObject.SetActive(true);
            foreach (GameObject obj in Divider)
                obj.SetActive(true);
        }

        for (int i = 0; i < _item.itemEffect.RequireValueDic.Count; i++)
        {
            switch (keys[i])
            {
                case 1:
                    str[i] = string.Format("���� <color><b>{0}</b></color>", _item.itemEffect.RequireValueDic[1]);
                    break;
                case 2:
                    str[i] = string.Format("�� <color><b>{0}</b></color>", _item.itemEffect.RequireValueDic[2]);
                    break;
                case 3:
                    str[i] = string.Format("��ø <color><b>{0}</b></color>", _item.itemEffect.RequireValueDic[3]);
                    break;
                case 4:
                    str[i] = string.Format("���� <color><b>{0}</b></color>", _item.itemEffect.RequireValueDic[4]);
                    break;
                case 5:
                    str[i] = string.Format("HpMax <color><b>{0}</b></color>", _item.itemEffect.RequireValueDic[5]);
                    break;
                case 6:
                    str[i] = string.Format("MpMax <color><b>{0}</b></color>", _item.itemEffect.RequireValueDic[6]);
                    break;
                default:
                    break;
            }

            if (i != 0)
                RequireText.text += ", " + str[i];
            else
                RequireText.text = "�䱸 " + str[i];
        }
        #endregion

        #region ������ ȿ��
        string[] str2 = new string[64];
        List<int> keys2 = new List<int>();
        keys2.AddRange(_item.itemEffect.ValueDic.Keys);
        int mainStart = 1;

        if (_item.Type == 9 || _item.Type == 10)
        {
            mainStart = 0;
            EffectText.gameObject.SetActive(true);
            //Divider[1].SetActive(false);
        }
        else
        {
            if (_item.itemEffect.ValueDic.Count == 1)
            {
                EffectText.gameObject.SetActive(false);
                Divider[1].SetActive(false);
            }
            else
            {
                EffectText.gameObject.SetActive(true);
                Divider[1].SetActive(true);
            }
        }

        for (int i = mainStart; i < _item.itemEffect.ValueDic.Count; i++)
        {
            switch (keys2[i])
            {
                case 1:
                    str2[i] = string.Format("Hp <color><b>{0}</b></color> ȸ��", _item.itemEffect.ValueDic[1]);
                    break;
                case 2:
                    str2[i] = string.Format("Mp <color><b>{0}</b></color> ȸ��", _item.itemEffect.ValueDic[2]);
                    break;
                case 3:
                    str2[i] = string.Format("Hp�ִ�ġ <color><b>{0}</b></color> ����", _item.itemEffect.ValueDic[3]);
                    break;
                case 4:
                    str2[i] = string.Format("Hp�ִ�ġ <color><b>{0}%</b></color> ����", _item.itemEffect.ValueDic[4]);
                    break;
                case 5:
                    str2[i] = string.Format("Mp�ִ�ġ <color><b>{0}</b></color> ����", _item.itemEffect.ValueDic[5]);
                    break;
                case 6:
                    str2[i] = string.Format("Mp�ִ�ġ <color><b>{0}%</b></color> ����", _item.itemEffect.ValueDic[6]);
                    break;
                case 7:
                    str2[i] = string.Format("���ݷ� <color><b>{0}</b></color> ����", _item.itemEffect.ValueDic[7]);
                    break;
                case 8:
                    str2[i] = string.Format("���ݷ� <color><b>{0}%</b></color> ����", _item.itemEffect.ValueDic[8]);
                    break;
                case 9:
                    str2[i] = string.Format("���� <color><b>{0}</b></color> ����", _item.itemEffect.ValueDic[9]);
                    break;
                case 10:
                    str2[i] = string.Format("���� <color><b>{0}%</b></color> ����", _item.itemEffect.ValueDic[10]);
                    break;
                case 11:
                    str2[i] = string.Format("�� <color><b>{0}</b></color> ����", _item.itemEffect.ValueDic[11]);
                    break;
                case 12:
                    str2[i] = string.Format("�� <color><b>{0}</b>%</color> ����", _item.itemEffect.ValueDic[12]);
                    break;
                case 13:
                    str2[i] = string.Format("���� <color><b>{0}</b></color> ����", _item.itemEffect.ValueDic[13]);
                    break;
                case 14:
                    str2[i] = string.Format("���� <color><b>{0}%</b></color> ����", _item.itemEffect.ValueDic[14]);
                    break;
                case 15:
                    str2[i] = string.Format("���ݽ� ����� <color><b>{0}</b></color> Hpȸ��", _item.itemEffect.ValueDic[15]);
                    break;
                case 16:
                    str2[i] = string.Format("���ݽ� �������� <color><b>{0}%</b></color> Hpȸ��", _item.itemEffect.ValueDic[16]);
                    break;
                default:
                    break;
            }

            if (i != mainStart)
                EffectText.text += "\n" + str2[i];
            else
                EffectText.text = str2[i];
        }
        #endregion

        #region ������ ����
        if (RectTransformUtility.RectangleContainsScreenPoint(shopRect, Input.mousePosition) && shopRect.gameObject.activeSelf)
            CostText.text = "���Ű��� : <color>" + _item.BuyCost + " Gold</color>";
        else
            CostText.text = "�ǸŰ��� : <color>" + _item.SellCost + " Gold</color>";

        CostText.gameObject.SetActive(true);

        //if (shopBase.activeSelf)
        //{
        //    CostText.gameObject.SetActive(true);
        //}
        //else
        //{
        //    CostText.gameObject.SetActive(false);
        //}
        #endregion

        wantActive = true;
    }

    /// <summary>
    /// ��ų�� ������ �޾Ƽ� ������ �ٲ��, ���� UI�� ������.
    /// </summary>
    /// <param name="_skill"></param>
    public void ShowTooltip(Skill _skill)
    {
        // ��ų Ÿ��
        // 0: �нú�, 1: ��Ÿ����, 2: Ÿ����, 3: ����, 4: ��ý���

        // �ڽ�Ʈ Ÿ��
        // 0: ���ڽ�Ʈ, 1: Hp, 2: Mp

        foreach (GameObject obj in Divider)
            obj.SetActive(true);

        NameText.text = _skill.Name;

        LevelText.gameObject.SetActive(true);

        LevelText.text = "Lv : " + player.player_Skill_Dic[_skill.UIDCODE] + " / �ִ� Lv : " + SkillDatabase.instance.AllSkillDic[_skill.UIDCODE].MaxSkillLv;

        if (_skill.CostType != 0)
            SkillCostText.gameObject.SetActive(true);
        else
            SkillCostText.gameObject.SetActive(false);

        switch (_skill.CostType)
        {
            case 1:
                SkillCostText.text = string.Format("Hp�Ҹ� : <color><b>{0}</b></color>", _skill.Cost);
                break;
            case 2:
                SkillCostText.text = string.Format("Mp�Ҹ� : <color><b>{0}</b></color>", _skill.Cost);
                break;
        }

        switch (_skill.Type)
        {
            case 0:
                TypeText.text = "�нú�";
                break;
            case 1:
                TypeText.text = "��Ÿ����";
                break;
            case 2:
                TypeText.text = "Ÿ����";
                break;
            case 3:
                TypeText.text = "����";
                break;
            case 4:
                TypeText.text = "��ý���";
                break;
        }

        BaseValueText.gameObject.SetActive(false);

        RequireText.gameObject.SetActive(true);
        RequireText.text = string.Format("�䱸 ���� <color><b>{0}</b></color>", _skill.NeedLv + player.player_Skill_Dic[_skill.UIDCODE]);

        EffectText.gameObject.SetActive(true);

        if (player.player_Skill_Dic[_skill.UIDCODE] > 0)
            switch (_skill.UIDCODE)
            {
                default:
                    EffectText.text = string.Format(_skill.SkillDescription, _skill.Value + (player.player_Skill_Dic[_skill.UIDCODE] - 1) * _skill.ValueFactor);
                    break;
            }

        else
            EffectText.text = string.Format(_skill.SkillDescription, _skill.Value);

        CostText.gameObject.SetActive(false);

        wantActive = true;
    }

    public void HideTooltip()
    {
        wantActive = false;
        activeOverTime = 0f;

        go_Tooltip.SetActive(false);
        tooltip_Rect.localPosition = new Vector3(1195, 388.75f);
    }
}
