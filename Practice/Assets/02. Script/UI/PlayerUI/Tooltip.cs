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

        #region 아이템 툴팁 체크 (디버그용)
        //Item _testItem = ItemDatabase.instance.newItem("0000008");
        //#region 아이템 딕셔너리 키 체크 (디버그용)
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
                //Offset.y = -(Input.mousePosition.y - tooltip_Rect.rect.height * 0.5f); // 아래쪽으로 딱 맞게
                Offset.y = tooltip_Rect.rect.height * 0.5f; // 위쪽으로 전환
                //Offset.y = 0; // 중간으로 뜨게
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
    /// 아이템의 정보를 받아서 툴팁이 바뀌고, 툴팁 UI를 보여줌.
    /// </summary>
    /// <param name="_item"></param>
    public void ShowTooltip(Item _item)
    {
        // 아이템 타입
        // 0: 물리공격무기, 1: 두손무기, 2: 헬멧, 3: 갑옷 , 4: 벨트, 5:장갑
        // 6: 신발, 7: 목걸이, 8: 반지, 9: 소비, 10: 재료

        // 아이템 이펙트 타입
        // 0: 없음, 1: 현재 Hp, 2: 현재 Mp, 3: 최대 Hp 고정값, 4: 최대 Hp %값, 5: 최대 Mp 고정값, 6: 최대 Mp %값,
        // 7: 물리 공격력 고정값, 8: 물리 공격력 %값, 9: 물리 방어력 고정값, 10: 물리 방어력 %값, 11: 힘 고정값, 12: 힘 %값,
        // 13: 지능 고정값, 14: 지능 %값, 15: 공격시 생명령 회복 고정값, 16: 공격시 데미지의 %만큼 생명력 회복,
        // 17: 마법 공격력 고정값, 18: 마법 공격력 %값, 19: 마법 방어력 고정값, 20: 마법 방어력 %값

        LevelText.gameObject.SetActive(false);
        SkillCostText.gameObject.SetActive(false);

        #region 아이템 이름
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

        #region 아이템 타입
        switch (_item.Type)
        {
            case 0:
                TypeText.text = "한손무기";
                break;
            case 1:
                TypeText.text = "두손무기";
                break;
            case 2:
                TypeText.text = "갑옷";
                break;
            case 3:
                TypeText.text = "헬멧";
                break;
            case 4:
                TypeText.text = "벨트";
                break;
            case 5:
                TypeText.text = "장갑";
                break;
            case 6:
                TypeText.text = "신발";
                break;
            case 7:
                TypeText.text = "목걸이";
                break;
            case 8:
                TypeText.text = "반지";
                break;
            case 9:
                TypeText.text = "소비";
                break;
            case 10:
                TypeText.text = "재료";
                break;
            default:
                TypeText.text = "아이템 타입 오류";
                break;
                #region 영문명
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
                //    TypeText.text = "아이템 타입 오류";
                //    break;
                #endregion
        }
        #endregion

        #region 아이템 베이스 값 텍스트
        switch (_item.Type)
        {
            case 0:
                BaseValueText.text = string.Format("물리 공격력 : <color=#ABA3FF><b>{0}</b></color>", _item.itemEffect.ValueDic[7]);
                BaseValueText.gameObject.SetActive(true);
                break;
            case 1:
                BaseValueText.text = string.Format("마법 공격력 : <color=#ABA3FF><b>{0}</b></color>", _item.itemEffect.ValueDic[7]);
                BaseValueText.gameObject.SetActive(true);
                break;
            case 2:
                BaseValueText.text = string.Format("방어력 : <color=#ABA3FF><b>{0}</b></color>", _item.itemEffect.ValueDic[9]);
                BaseValueText.gameObject.SetActive(true);
                break;
            case 3:
                BaseValueText.text = string.Format("방어력 : <color=#ABA3FF><b>{0}</b></color>", _item.itemEffect.ValueDic[9]);
                BaseValueText.gameObject.SetActive(true);
                break;
            case 4:
                BaseValueText.text = string.Format("방어력 : <color=#ABA3FF><b>{0}</b></color>", _item.itemEffect.ValueDic[9]);
                BaseValueText.gameObject.SetActive(true);
                break;
            case 5:
                BaseValueText.text = string.Format("방어력 : <color=#ABA3FF><b>{0}</b></color>", _item.itemEffect.ValueDic[9]);
                BaseValueText.gameObject.SetActive(true);
                break;
            case 6:
                BaseValueText.text = string.Format("방어력 : <color=#ABA3FF><b>{0}</b></color>", _item.itemEffect.ValueDic[9]);
                BaseValueText.gameObject.SetActive(true);
                break;
            case 7:
                BaseValueText.text = string.Format("최대 생명력 <color=#ABA3FF><b>{0}</b></color> 증가", _item.itemEffect.ValueDic[3]);
                BaseValueText.gameObject.SetActive(true);
                break;
            case 8:
                BaseValueText.text = string.Format("최대 마나 <color=#ABA3FF><b>{0}</b></color> 증가", _item.itemEffect.ValueDic[5]);
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

        #region 아이템 필요조건
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
                    str[i] = string.Format("레벨 <color><b>{0}</b></color>", _item.itemEffect.RequireValueDic[1]);
                    break;
                case 2:
                    str[i] = string.Format("힘 <color><b>{0}</b></color>", _item.itemEffect.RequireValueDic[2]);
                    break;
                case 3:
                    str[i] = string.Format("민첩 <color><b>{0}</b></color>", _item.itemEffect.RequireValueDic[3]);
                    break;
                case 4:
                    str[i] = string.Format("지능 <color><b>{0}</b></color>", _item.itemEffect.RequireValueDic[4]);
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
                RequireText.text = "요구 " + str[i];
        }
        #endregion

        #region 아이템 효과
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
                    str2[i] = string.Format("Hp <color><b>{0}</b></color> 회복", _item.itemEffect.ValueDic[1]);
                    break;
                case 2:
                    str2[i] = string.Format("Mp <color><b>{0}</b></color> 회복", _item.itemEffect.ValueDic[2]);
                    break;
                case 3:
                    str2[i] = string.Format("Hp최대치 <color><b>{0}</b></color> 증가", _item.itemEffect.ValueDic[3]);
                    break;
                case 4:
                    str2[i] = string.Format("Hp최대치 <color><b>{0}%</b></color> 증가", _item.itemEffect.ValueDic[4]);
                    break;
                case 5:
                    str2[i] = string.Format("Mp최대치 <color><b>{0}</b></color> 증가", _item.itemEffect.ValueDic[5]);
                    break;
                case 6:
                    str2[i] = string.Format("Mp최대치 <color><b>{0}%</b></color> 증가", _item.itemEffect.ValueDic[6]);
                    break;
                case 7:
                    str2[i] = string.Format("공격력 <color><b>{0}</b></color> 증가", _item.itemEffect.ValueDic[7]);
                    break;
                case 8:
                    str2[i] = string.Format("공격력 <color><b>{0}%</b></color> 증가", _item.itemEffect.ValueDic[8]);
                    break;
                case 9:
                    str2[i] = string.Format("방어력 <color><b>{0}</b></color> 증가", _item.itemEffect.ValueDic[9]);
                    break;
                case 10:
                    str2[i] = string.Format("방어력 <color><b>{0}%</b></color> 증가", _item.itemEffect.ValueDic[10]);
                    break;
                case 11:
                    str2[i] = string.Format("힘 <color><b>{0}</b></color> 증가", _item.itemEffect.ValueDic[11]);
                    break;
                case 12:
                    str2[i] = string.Format("힘 <color><b>{0}</b>%</color> 증가", _item.itemEffect.ValueDic[12]);
                    break;
                case 13:
                    str2[i] = string.Format("지능 <color><b>{0}</b></color> 증가", _item.itemEffect.ValueDic[13]);
                    break;
                case 14:
                    str2[i] = string.Format("지능 <color><b>{0}%</b></color> 증가", _item.itemEffect.ValueDic[14]);
                    break;
                case 15:
                    str2[i] = string.Format("공격시 생명력 <color><b>{0}</b></color> Hp회복", _item.itemEffect.ValueDic[15]);
                    break;
                case 16:
                    str2[i] = string.Format("공격시 데미지의 <color><b>{0}%</b></color> Hp회복", _item.itemEffect.ValueDic[16]);
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

        #region 아이템 가격
        if (RectTransformUtility.RectangleContainsScreenPoint(shopRect, Input.mousePosition) && shopRect.gameObject.activeSelf)
            CostText.text = "구매가격 : <color>" + _item.BuyCost + " Gold</color>";
        else
            CostText.text = "판매가격 : <color>" + _item.SellCost + " Gold</color>";

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
    /// 스킬의 정보를 받아서 툴팁이 바뀌고, 툴팁 UI를 보여줌.
    /// </summary>
    /// <param name="_skill"></param>
    public void ShowTooltip(Skill _skill)
    {
        // 스킬 타입
        // 0: 패시브, 1: 논타겟팅, 2: 타겟팅, 3: 버프, 4: 즉시시전

        // 코스트 타입
        // 0: 노코스트, 1: Hp, 2: Mp

        foreach (GameObject obj in Divider)
            obj.SetActive(true);

        NameText.text = _skill.Name;

        LevelText.gameObject.SetActive(true);

        LevelText.text = "Lv : " + player.player_Skill_Dic[_skill.UIDCODE] + " / 최대 Lv : " + SkillDatabase.instance.AllSkillDic[_skill.UIDCODE].MaxSkillLv;

        if (_skill.CostType != 0)
            SkillCostText.gameObject.SetActive(true);
        else
            SkillCostText.gameObject.SetActive(false);

        switch (_skill.CostType)
        {
            case 1:
                SkillCostText.text = string.Format("Hp소모 : <color><b>{0}</b></color>", _skill.Cost);
                break;
            case 2:
                SkillCostText.text = string.Format("Mp소모 : <color><b>{0}</b></color>", _skill.Cost);
                break;
        }

        switch (_skill.Type)
        {
            case 0:
                TypeText.text = "패시브";
                break;
            case 1:
                TypeText.text = "논타겟팅";
                break;
            case 2:
                TypeText.text = "타겟팅";
                break;
            case 3:
                TypeText.text = "버프";
                break;
            case 4:
                TypeText.text = "즉시시전";
                break;
        }

        BaseValueText.gameObject.SetActive(false);

        RequireText.gameObject.SetActive(true);
        RequireText.text = string.Format("요구 레벨 <color><b>{0}</b></color>", _skill.NeedLv + player.player_Skill_Dic[_skill.UIDCODE]);

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
