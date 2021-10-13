using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public int index;
    public Item item;
    public Image itemImage;
    public Image slot_BG;
    public GameObject go_Back_Image;

    public int itemCount = 0;

    [SerializeField]
    Text countText;

    ItemDatabase ItemDB;
    InputNumberUI inputNumber;
    public Inventory inven;
    Tooltip tooltip;
    ShopMessage shopMessage;
    Shop_Test shop;

    public PlayerInfo player;

    [SerializeField]
    RectTransform invenBase;
    [SerializeField]
    RectTransform quickSlotBase;
    [SerializeField]
    RectTransform shopBase;

    GameObject cameraArm;

    const string defalt_EquipmentSlotBG_Path = "UI/Tooltip/TooltipBackground";
    const string defalt_SlotBG_Path = "UI/Inventory/Slot_Frame/itemFrame_alphaFront";
    const string common_SlotBG_Path = "UI/Inventory/Slot_Frame/itemFrame_white";
    const string rare_SlotBG_Path = "UI/Inventory/Slot_Frame/itemFrame_cyan";
    const string unique_SlotBG_Path = "UI/Inventory/Slot_Frame/itemFrame_pink";
    const string epic_SlotBG_Path = "UI/Inventory/Slot_Frame/itemFrame_yellow";
    const string set_SlotBG_Path = "UI/Inventory/Slot_Frame/itemFrame_green";

    private void Awake()
    {
        ItemDB = FindObjectOfType<ItemDatabase>();
        inputNumber = FindObjectOfType<InputNumberUI>();
        tooltip = FindObjectOfType<Tooltip>();
        shopMessage = FindObjectOfType<ShopMessage>();
        shop = FindObjectOfType<Shop_Test>();
        cameraArm = GameObject.Find("CameraArm");
    }

    void SetColorAlpha(float alpha)
    {
        Color color = itemImage.color;
        color.a = alpha;
        itemImage.color = color;
    }

    /// <summary>
    /// 슬롯에 아이템을 추가하고, 아이템에 슬롯 인덱스 정보를 넣어주고, 슬롯에 이미지를 뜨게함.
    /// </summary>
    /// <param name="_item"></param>
    /// <param name="count"></param>
    public void AddItem(Item _item, int count = 1)
    {
        item = _item;
        itemCount = count;
        item.SlotIndex = index;
        item.Count = count;
        itemImage.sprite = Resources.Load<Sprite>(_item.ItemImagePath);
        SetColorAlpha(0.85f);

        if (item.Type == 9 || item.Type == 10)
        {
            countText.gameObject.SetActive(true);
            countText.text = itemCount.ToString();
        }
        else
        {
            countText.text = "0";
            countText.gameObject.SetActive(false);
        }

        switch (_item.Rarity)
        {
            case 0: // 일반
                slot_BG.sprite = Resources.Load<Sprite>(common_SlotBG_Path);
                break;
            case 1: // 레어
                slot_BG.sprite = Resources.Load<Sprite>(rare_SlotBG_Path);
                break;
            case 2: // 유니크
                slot_BG.sprite = Resources.Load<Sprite>(unique_SlotBG_Path);
                break;
            case 3: // 에픽
                slot_BG.sprite = Resources.Load<Sprite>(epic_SlotBG_Path);
                break;
            case 4: // 세트
                slot_BG.sprite = Resources.Load<Sprite>(set_SlotBG_Path);
                break;
        }
    }

    /// <summary>
    /// 해당슬롯의 아이템 카운트를 인수값 만큼 더해줌. 아이템 카운트가 0이되면 ClearSlot 함수 발동.
    /// </summary>
    /// <param name="count"></param>
    public void SetSlotCount(int count)
    {
        itemCount += count;
        item.Count += count;
        countText.text = itemCount.ToString();

        if (itemCount <= 0)
        {
            ClearSlot();
            tooltip.HideTooltip();
        }
    }

    /// <summary>
    /// 해당칸을 비워줌. (item = null, itemCount = 0, 스프라이트 = null, 알파값 0으로)
    /// </summary>
    void ClearSlot()
    {
        item = null;
        itemImage.sprite = null;
        if (gameObject.CompareTag("Equipment"))
        {
            go_Back_Image.SetActive(true);
            slot_BG.sprite = Resources.Load<Sprite>(defalt_EquipmentSlotBG_Path);
        }
        else
        {
            slot_BG.sprite = Resources.Load<Sprite>(defalt_SlotBG_Path);
        }
        SetColorAlpha(0);
        itemCount = 0;

        countText.text = "0";
        countText.gameObject.SetActive(false);
    }

    /// <summary>
    /// 슬롯 우클릭시 슬롯에 있는 아이템 타입을 보고 소모품이면 아이템 사용,
    /// 장비면 스테이터스창의 해당 장비타입칸에 착용.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (itemCount != 0)
            {
                if (gameObject.CompareTag("Inventory"))
                {
                    if (!shopBase.gameObject.activeSelf)
                    {
                        if (item.Type != 9 && item.Type != 10)
                            EquipItem(item);
                        else if (item.Type == 9)
                            UseItem(item);
                        else if (item.Type == 10)
                            Debug.Log("재료탬 우클릭 - 효과없음");
                    }
                }
                else if (gameObject.CompareTag("Equipment"))
                {
                    UnEquipItem(item);
                }
                else if (gameObject.CompareTag("QuickPotionSlot"))
                {
                    UseItem(item);
                }
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (shopBase.gameObject.activeSelf && shop.isSelling)
            {
                SellItem(item);
            }
        }
    }

    /// <summary>
    /// 드레그 시작시 드레그 시작한 슬롯에 있는 아이템 드레그 슬롯에 복제
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (itemCount != 0)
            {
                DragSlot.instance.dragSlot = this;
                DragSlot.instance.DragSetImage(itemImage);
                DragSlot.instance.transform.position = eventData.position;
            }
        }
    }

    /// <summary>
    /// 드레그 하는 동안 포지션 변경
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
            DragSlot.instance.transform.position = eventData.position;
    }

    /// <summary>
    /// 드레그 끝나면 드레그 슬롯 알파값 0으로 바꾸고 비워줌.<br/>
    /// 드레그 끝난 위치가 스텟창이나 인벤창 밖이면 아이템 드롭창 불러줌.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(shopBase, Input.mousePosition) && shopBase.gameObject.activeSelf)
        {
            if (DragSlot.instance.dragSlot != null)
            {
                if (DragSlot.instance.dragSlot.CompareTag("Inventory"))
                {
                    shopMessage.ShowMessageTxt(DragSlot.instance.dragSlot.item, 1);
                }
            }
        }
        else
        {
            if (DragSlot.instance.dragSlot != null)
            {
                DragSlot.instance.SetColorAlpha(0);
                DragSlot.instance.dragSlot = null;
            }
        }

    }

    /// <summary>
    /// 드롭된 슬롯의 종류에따라 기능이발동됨.<br/>
    /// 인벤토리 슬롯은 ChangeSlot 함수 발동. 장비 슬롯은 장착기능 발동.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)
        {
            if (gameObject.CompareTag("Inventory"))
            {
                if (DragSlot.instance.dragSlot.CompareTag("Inventory"))
                {
                    ChangeSlot();
                }
                else if (DragSlot.instance.dragSlot.CompareTag("Equipment"))
                {
                    DragSlot.instance.dragSlot.UnEquipItem(DragSlot.instance.dragSlot.item, this);
                }
                else if (DragSlot.instance.dragSlot.CompareTag("QuickPotionSlot"))
                {
                    if (itemCount != 0)
                    {
                        if (item.Type == 9 && DragSlot.instance.dragSlot.item.Type == 9)
                        {
                            ChangeSlot();
                        }
                        else
                        {
                            inven.GetItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

                            DragSlot.instance.dragSlot.ClearSlot();
                        }
                    }
                    else
                    {
                        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

                        DragSlot.instance.dragSlot.ClearSlot();
                    }
                }
            }
            else if (gameObject.CompareTag("Equipment"))
            {
                if (DragSlot.instance.dragSlot.CompareTag("Inventory") && DragSlot.instance.dragSlot.item.Type != 9 && DragSlot.instance.dragSlot.item.Type != 10)
                {
                    if (inven.Equipment_Slots[DragSlot.instance.dragSlot.item.Type].itemCount != 0)
                    {
                        DragSlot.instance.dragSlot.EquipItem(DragSlot.instance.dragSlot.item);
                    }
                    else
                    {
                        DragSlot.instance.dragSlot.EquipItem(DragSlot.instance.dragSlot.item);
                    }
                }
                else if (DragSlot.instance.dragSlot.CompareTag("Equipment"))
                {
                    // 현재는 같은 장비타입을 끼는 장착 슬롯이 없으므로 무시.
                    // 이후에 생기면 구문 작성 필요.
                }
            }
            else if (gameObject.CompareTag("QuickPotionSlot"))
            {
                if (DragSlot.instance.dragSlot.CompareTag("Inventory"))
                {
                    if (DragSlot.instance.dragSlot.item.Type == 9)
                        ChangeSlot();
                }
                else if (DragSlot.instance.dragSlot.CompareTag("QuickPotionSlot"))
                {
                    ChangeSlot();
                }
            }
        }

        if (itemCount != 0)
            tooltip.ShowTooltip(item);
    }

    /// <summary>
    /// 슬롯 교체할때 아이템 유무에 따라서 옴기기만 할지, 서로 바꿔줄지를 결정해서 처리해줌.
    /// </summary>
    private void ChangeSlot()
    {
        if (this != DragSlot.instance.dragSlot)
        {
            if (itemCount != 0)
            {
                if (item.Name == DragSlot.instance.dragSlot.item.Name)
                {
                    if (item.Type == 9 || item.Type == 10)
                    {
                        SetSlotCount(DragSlot.instance.dragSlot.itemCount);

                        DragSlot.instance.dragSlot.ClearSlot();
                    }
                }
                else
                {
                    Item _item = item;
                    int _itemCount = itemCount;

                    AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

                    DragSlot.instance.dragSlot.AddItem(_item, _itemCount);
                }
            }
            else
            {
                AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

                DragSlot.instance.dragSlot.ClearSlot();
                DragSlot.instance.SetColorAlpha(0);
                DragSlot.instance.dragSlot = null;
            }
        }
    }

    /// <summary>
    /// 아이템 있으면 툴팁출력.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemCount != 0)
            tooltip.ShowTooltip(item);
    }

    /// <summary>
    /// 툴팁 꺼지게함.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }

    /// <summary>
    /// 아이템이 슬롯에서 제거되고, 장비칸에 아이템 타입에 따라서 장착됨.
    /// 장비칸에 이미 아이템이 있을경우 교환해줌.
    /// </summary>
    /// <param name="_item"></param>
    public void EquipItem(Item _item)
    {
        if (player.stats.Level < _item.itemEffect.RequireValueDic[1])
        {
            Debug.Log("레벨이 착용제한에 걸립니다.");
            return;
        }

        SetSlotCount(-1);

        #region 아이템 타입에따라 장비슬롯에 장착하는 내용.
        switch (_item.Type)
        {
            // 0: 한손무기, 1: 두손무기, 2: 헬멧, 3: 갑옷, 4: 벨트, 5:장갑
            // 6: 신발, 7: 목걸이, 8: 반지, 9: 소비, 10: 재료

            case 0:
                if (inven.WeaponSlot.itemCount == 0)
                {
                    inven.WeaponSlot.AddItem(_item);
                    inven.WeaponSlot.go_Back_Image.SetActive(false);
                }
                else
                {
                    inven.WeaponSlot.UnEquipItem(inven.WeaponSlot.item, this);
                    inven.WeaponSlot.AddItem(_item);
                    inven.WeaponSlot.go_Back_Image.SetActive(false);

                    if (itemCount != 0)
                        tooltip.ShowTooltip(item);
                }
                break;
            case 1:
                if (inven.WeaponSlot.itemCount == 0)
                {
                    inven.WeaponSlot.AddItem(_item);
                    inven.WeaponSlot.go_Back_Image.SetActive(false);
                }
                else
                {
                    inven.WeaponSlot.UnEquipItem(inven.WeaponSlot.item, this);
                    inven.WeaponSlot.AddItem(_item);
                    inven.WeaponSlot.go_Back_Image.SetActive(false);

                    if (itemCount != 0)
                        tooltip.ShowTooltip(item);
                }
                break;
            case 2:
                if (inven.HelmetSlot.itemCount == 0)
                {
                    inven.HelmetSlot.AddItem(_item);
                    inven.HelmetSlot.go_Back_Image.SetActive(false);
                }
                else
                {
                    inven.HelmetSlot.UnEquipItem(inven.HelmetSlot.item, this);
                    inven.HelmetSlot.AddItem(_item);
                    inven.HelmetSlot.go_Back_Image.SetActive(false);

                    if (itemCount != 0)
                        tooltip.ShowTooltip(item);
                }
                break;
            case 3:
                if (inven.ArmorSlot.itemCount == 0)
                {
                    inven.ArmorSlot.AddItem(_item);
                    inven.ArmorSlot.go_Back_Image.SetActive(false);
                }
                else
                {
                    inven.ArmorSlot.UnEquipItem(inven.ArmorSlot.item, this);
                    inven.ArmorSlot.AddItem(_item);
                    inven.ArmorSlot.go_Back_Image.SetActive(false);

                    if (itemCount != 0)
                        tooltip.ShowTooltip(item);
                }
                break;
            case 4:
                if (inven.BeltSlot.itemCount == 0)
                {
                    inven.BeltSlot.AddItem(_item);
                    inven.BeltSlot.go_Back_Image.SetActive(false);
                }
                else
                {
                    inven.BeltSlot.UnEquipItem(inven.BeltSlot.item, this);
                    inven.BeltSlot.AddItem(_item);
                    inven.BeltSlot.go_Back_Image.SetActive(false);

                    if (itemCount != 0)
                        tooltip.ShowTooltip(item);
                }
                break;
            case 5:
                if (inven.GlovesSlot.itemCount == 0)
                {
                    inven.GlovesSlot.AddItem(_item);
                    inven.GlovesSlot.go_Back_Image.SetActive(false);
                }
                else
                {
                    inven.GlovesSlot.UnEquipItem(inven.GlovesSlot.item, this);
                    inven.GlovesSlot.AddItem(_item);
                    inven.GlovesSlot.go_Back_Image.SetActive(false);

                    if (itemCount != 0)
                        tooltip.ShowTooltip(item);
                }
                break;
            case 6:
                if (inven.BootsSlot.itemCount == 0)
                {
                    inven.BootsSlot.AddItem(_item);
                    inven.BootsSlot.go_Back_Image.SetActive(false);
                }
                else
                {
                    inven.BootsSlot.UnEquipItem(inven.BootsSlot.item, this);
                    inven.BootsSlot.AddItem(_item);
                    inven.BootsSlot.go_Back_Image.SetActive(false);

                    if (itemCount != 0)
                        tooltip.ShowTooltip(item);
                }
                break;
            case 7:
                if (inven.NecklaceSlot.itemCount == 0)
                {
                    inven.NecklaceSlot.AddItem(_item);
                    inven.NecklaceSlot.go_Back_Image.SetActive(false);
                }
                else
                {
                    inven.NecklaceSlot.UnEquipItem(inven.NecklaceSlot.item, this);
                    inven.NecklaceSlot.AddItem(_item);
                    inven.NecklaceSlot.go_Back_Image.SetActive(false);

                    if (itemCount != 0)
                        tooltip.ShowTooltip(item);
                }
                break;
            case 8:
                if (inven.RingSlot.itemCount == 0)
                {
                    inven.RingSlot.AddItem(_item);
                    inven.RingSlot.go_Back_Image.SetActive(false);
                }
                else
                {
                    inven.RingSlot.UnEquipItem(inven.RingSlot.item, this);
                    inven.RingSlot.AddItem(_item);
                    inven.RingSlot.go_Back_Image.SetActive(false);

                    if (itemCount != 0)
                        tooltip.ShowTooltip(item);
                }
                break;
        }
        #endregion

        #region 아이템 효과를 받아와서 실제 스텟에 적용하는 내용.
        List<int> keys = new List<int>();
        keys.AddRange(_item.itemEffect.ValueDic.Keys);

        for (int i = 0; i < keys.Count; i++)
        {
            // 0: 없음, 1: 현재 Hp, 2: 현재 Mp, 3: 최대 Hp 고정값, 4: 최대 Hp %값, 5: 최대 Mp 고정값, 6: 최대 Mp %값,
            // 7: 물리 공격력 고정값, 8: 물리 공격력 %값, 9: 물리 방어력 고정값, 10: 물리 방어력 %값, 11: 힘 고정값, 12: 힘 %값,
            // 13: 지능 고정값, 14: 지능 %값, 15: 공격시 생명령 회복 고정값, 16: 공격시 데미지의 %만큼 생명력 회복,
            // 17: 마법 공격력 고정값, 18: 마법 공격력 %값, 19: 마법 방어력 고정값, 20: 마법 방어력 %값

            switch (keys[i])
            {
                case 3:
                    player.ItemEffectMaxHp += _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 4:
                    player.ItemEffectMaxHpMultiplier += (_item.itemEffect.ValueDic[keys[i]]) * 0.01f;
                    break;
                case 5:
                    player.ItemEffectMaxMp += _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 6:
                    player.ItemEffectMaxMpMultiplier += (_item.itemEffect.ValueDic[keys[i]]) * 0.01f;
                    break;
                case 7:
                    player.ItemEffectAtk += _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 8:
                    player.ItemEffectAtkMultiplier += (_item.itemEffect.ValueDic[keys[i]]) * 0.01f;
                    break;
                case 9:
                    player.ItemEffectDef += _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 10:
                    player.ItemEffectDefMultiplier += (_item.itemEffect.ValueDic[keys[i]]) * 0.01f;
                    break;
                case 11:
                    player.ItemEffectStr += _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 12:
                    player.ItemEffectStrMultiplier += (_item.itemEffect.ValueDic[keys[i]]) * 0.01f;
                    break;
                case 13:
                    player.ItemEffectInt += _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 14:
                    player.ItemEffectIntMultiplier += (_item.itemEffect.ValueDic[keys[i]]) * 0.01f;
                    break;
                case 15:
                    player.ItemEffectLifeSteal += _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 16:
                    player.ItemEffectLifeStealPercent += _item.itemEffect.ValueDic[keys[i]];
                    break;
                default:
                    break;
            }
        }
        #endregion

        player.RefeshFinalStats();
    }

    /// <summary>
    /// 인벤창에 아이템이 추가되고, 스텟창에 있는 장비슬롯의 아이템 겟수를 -1 (0이되서 ClearSlot함수실행됨)
    /// 슬롯을 인수로 주게되면 해당슬롯에 가게됨.
    /// </summary>
    /// <param name="_item"></param>
    public void UnEquipItem(Item _item, Slot _slot = null)
    {
        SetSlotCount(-1);

        #region 아이템 효과 제거(감소시키는)하는 부분
        List<int> keys = new List<int>();
        keys.AddRange(_item.itemEffect.ValueDic.Keys);

        for (int i = 0; i < keys.Count; i++)
        {
            // 0: 없음, 1: 현재 Hp, 2: 현재 Mp, 3: 최대 Hp 고정값, 4: 최대 Hp %값, 5: 최대 Mp 고정값, 6: 최대 Mp %값,
            // 7: 물리 공격력 고정값, 8: 물리 공격력 %값, 9: 물리 방어력 고정값, 10: 물리 방어력 %값, 11: 힘 고정값, 12: 힘 %값,
            // 13: 지능 고정값, 14: 지능 %값, 15: 공격시 생명령 회복 고정값, 16: 공격시 데미지의 %만큼 생명력 회복,
            // 17: 마법 공격력 고정값, 18: 마법 공격력 %값, 19: 마법 방어력 고정값, 20: 마법 방어력 %값

            switch (keys[i])
            {
                case 3:
                    player.ItemEffectMaxHp -= _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 4:
                    player.ItemEffectMaxHpMultiplier -= (_item.itemEffect.ValueDic[keys[i]]) * 0.01f;
                    break;
                case 5:
                    player.ItemEffectMaxMp -= _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 6:
                    player.ItemEffectMaxMpMultiplier -= (_item.itemEffect.ValueDic[keys[i]]) * 0.01f;
                    break;
                case 7:
                    player.ItemEffectAtk -= _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 8:
                    player.ItemEffectAtkMultiplier -= (_item.itemEffect.ValueDic[keys[i]]) * 0.01f;
                    break;
                case 9:
                    player.ItemEffectDef -= _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 10:
                    player.ItemEffectDefMultiplier -= (_item.itemEffect.ValueDic[keys[i]]) * 0.01f;
                    break;
                case 11:
                    player.ItemEffectStr -= _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 12:
                    player.ItemEffectStrMultiplier -= (_item.itemEffect.ValueDic[keys[i]]) * 0.01f;
                    break;
                case 13:
                    player.ItemEffectInt -= _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 14:
                    player.ItemEffectIntMultiplier -= (_item.itemEffect.ValueDic[keys[i]]) * 0.01f;
                    break;
                case 15:
                    player.ItemEffectLifeSteal -= _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 16:
                    player.ItemEffectLifeStealPercent -= _item.itemEffect.ValueDic[keys[i]] * 0.01f;
                    break;
                default:
                    break;
            }
        }
        #endregion

        player.RefeshFinalStats();

        if (_slot != null)
            _slot.AddItem(_item);
        else
            inven.GetItem(_item);
    }

    public void OnLoadEquipItem(Item _item)
    {
        #region 아이템 타입에따라 장비슬롯에 장착하는 내용.
        switch (_item.Type)
        {
            // 0: 한손무기, 1: 두손무기, 2: 헬멧, 3: 갑옷, 4: 벨트, 5:장갑
            // 6: 신발, 7: 목걸이, 8: 반지, 9: 소비, 10: 재료

            case 0:
                if (inven.WeaponSlot.itemCount == 0)
                {
                    inven.WeaponSlot.AddItem(_item);
                    inven.WeaponSlot.go_Back_Image.SetActive(false);
                }
                else
                {
                    inven.WeaponSlot.UnEquipItem(inven.WeaponSlot.item, this);
                    inven.WeaponSlot.AddItem(_item);
                    inven.WeaponSlot.go_Back_Image.SetActive(false);

                    if (itemCount != 0)
                        tooltip.ShowTooltip(item);
                }
                break;
            case 1:
                if (inven.WeaponSlot.itemCount == 0)
                {
                    inven.WeaponSlot.AddItem(_item);
                    inven.WeaponSlot.go_Back_Image.SetActive(false);
                }
                else
                {
                    inven.WeaponSlot.UnEquipItem(inven.WeaponSlot.item, this);
                    inven.WeaponSlot.AddItem(_item);
                    inven.WeaponSlot.go_Back_Image.SetActive(false);

                    if (itemCount != 0)
                        tooltip.ShowTooltip(item);
                }
                break;
            case 2:
                if (inven.HelmetSlot.itemCount == 0)
                {
                    inven.HelmetSlot.AddItem(_item);
                    inven.HelmetSlot.go_Back_Image.SetActive(false);
                }
                else
                {
                    inven.HelmetSlot.UnEquipItem(inven.HelmetSlot.item, this);
                    inven.HelmetSlot.AddItem(_item);
                    inven.HelmetSlot.go_Back_Image.SetActive(false);

                    if (itemCount != 0)
                        tooltip.ShowTooltip(item);
                }
                break;
            case 3:
                if (inven.ArmorSlot.itemCount == 0)
                {
                    inven.ArmorSlot.AddItem(_item);
                    inven.ArmorSlot.go_Back_Image.SetActive(false);
                }
                else
                {
                    inven.ArmorSlot.UnEquipItem(inven.ArmorSlot.item, this);
                    inven.ArmorSlot.AddItem(_item);
                    inven.ArmorSlot.go_Back_Image.SetActive(false);

                    if (itemCount != 0)
                        tooltip.ShowTooltip(item);
                }
                break;
            case 4:
                if (inven.BeltSlot.itemCount == 0)
                {
                    inven.BeltSlot.AddItem(_item);
                    inven.BeltSlot.go_Back_Image.SetActive(false);
                }
                else
                {
                    inven.BeltSlot.UnEquipItem(inven.BeltSlot.item, this);
                    inven.BeltSlot.AddItem(_item);
                    inven.BeltSlot.go_Back_Image.SetActive(false);

                    if (itemCount != 0)
                        tooltip.ShowTooltip(item);
                }
                break;
            case 5:
                if (inven.GlovesSlot.itemCount == 0)
                {
                    inven.GlovesSlot.AddItem(_item);
                    inven.GlovesSlot.go_Back_Image.SetActive(false);
                }
                else
                {
                    inven.GlovesSlot.UnEquipItem(inven.GlovesSlot.item, this);
                    inven.GlovesSlot.AddItem(_item);
                    inven.GlovesSlot.go_Back_Image.SetActive(false);

                    if (itemCount != 0)
                        tooltip.ShowTooltip(item);
                }
                break;
            case 6:
                if (inven.BootsSlot.itemCount == 0)
                {
                    inven.BootsSlot.AddItem(_item);
                    inven.BootsSlot.go_Back_Image.SetActive(false);
                }
                else
                {
                    inven.BootsSlot.UnEquipItem(inven.BootsSlot.item, this);
                    inven.BootsSlot.AddItem(_item);
                    inven.BootsSlot.go_Back_Image.SetActive(false);

                    if (itemCount != 0)
                        tooltip.ShowTooltip(item);
                }
                break;
            case 7:
                if (inven.NecklaceSlot.itemCount == 0)
                {
                    inven.NecklaceSlot.AddItem(_item);
                    inven.NecklaceSlot.go_Back_Image.SetActive(false);
                }
                else
                {
                    inven.NecklaceSlot.UnEquipItem(inven.NecklaceSlot.item, this);
                    inven.NecklaceSlot.AddItem(_item);
                    inven.NecklaceSlot.go_Back_Image.SetActive(false);

                    if (itemCount != 0)
                        tooltip.ShowTooltip(item);
                }
                break;
            case 8:
                if (inven.RingSlot.itemCount == 0)
                {
                    inven.RingSlot.AddItem(_item);
                    inven.RingSlot.go_Back_Image.SetActive(false);
                }
                else
                {
                    inven.RingSlot.UnEquipItem(inven.RingSlot.item, this);
                    inven.RingSlot.AddItem(_item);
                    inven.RingSlot.go_Back_Image.SetActive(false);

                    if (itemCount != 0)
                        tooltip.ShowTooltip(item);
                }
                break;
        }
        #endregion

        #region 아이템 효과를 받아와서 실제 스텟에 적용하는 내용.
        List<int> keys = new List<int>();
        keys.AddRange(_item.itemEffect.ValueDic.Keys);

        for (int i = 0; i < keys.Count; i++)
        {
            // 0: 없음, 1: 현재 Hp, 2: 현재 Mp, 3: 최대 Hp 고정값, 4: 최대 Hp %값, 5: 최대 Mp 고정값, 6: 최대 Mp %값,
            // 7: 물리 공격력 고정값, 8: 물리 공격력 %값, 9: 물리 방어력 고정값, 10: 물리 방어력 %값, 11: 힘 고정값, 12: 힘 %값,
            // 13: 지능 고정값, 14: 지능 %값, 15: 공격시 생명령 회복 고정값, 16: 공격시 데미지의 %만큼 생명력 회복,
            // 17: 마법 공격력 고정값, 18: 마법 공격력 %값, 19: 마법 방어력 고정값, 20: 마법 방어력 %값

            switch (keys[i])
            {
                case 3:
                    player.ItemEffectMaxHp += _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 4:
                    player.ItemEffectMaxHpMultiplier += (_item.itemEffect.ValueDic[keys[i]]) * 0.01f;
                    break;
                case 5:
                    player.ItemEffectMaxMp += _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 6:
                    player.ItemEffectMaxMpMultiplier += (_item.itemEffect.ValueDic[keys[i]]) * 0.01f;
                    break;
                case 7:
                    player.ItemEffectAtk += _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 8:
                    player.ItemEffectAtkMultiplier += (_item.itemEffect.ValueDic[keys[i]]) * 0.01f;
                    break;
                case 9:
                    player.ItemEffectDef += _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 10:
                    player.ItemEffectDefMultiplier += (_item.itemEffect.ValueDic[keys[i]]) * 0.01f;
                    break;
                case 11:
                    player.ItemEffectStr += _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 12:
                    player.ItemEffectStrMultiplier += (_item.itemEffect.ValueDic[keys[i]]) * 0.01f;
                    break;
                case 13:
                    player.ItemEffectInt += _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 14:
                    player.ItemEffectIntMultiplier += (_item.itemEffect.ValueDic[keys[i]]) * 0.01f;
                    break;
                case 15:
                    player.ItemEffectLifeSteal += _item.itemEffect.ValueDic[keys[i]];
                    break;
                case 16:
                    player.ItemEffectLifeStealPercent += _item.itemEffect.ValueDic[keys[i]];
                    break;
                default:
                    break;
            }
        }
        #endregion

        player.RefeshFinalStats();
    }

    public void UseItem(Item _item)
    {
        List<int> keys = new List<int>();
        keys.AddRange(_item.itemEffect.ValueDic.Keys);

        for (int i = 0; i < keys.Count; i++)
        {
            // 0: 없음, 1: 현재 Hp, 2: 현재 Mp, 3: 최대 Hp 고정값, 4: 최대 Hp %값, 5: 최대 Mp 고정값, 6: 최대 Mp %값,
            // 7: 물리 공격력 고정값, 8: 물리 공격력 %값, 9: 물리 방어력 고정값, 10: 물리 방어력 %값, 11: 힘 고정값, 12: 힘 %값,
            // 13: 지능 고정값, 14: 지능 %값, 15: 공격시 생명령 회복 고정값, 16: 공격시 데미지의 %만큼 생명력 회복,
            // 17: 마법 공격력 고정값, 18: 마법 공격력 %값, 19: 마법 방어력 고정값, 20: 마법 방어력 %값

            switch (keys[i])
            {
                case 1:
                    if (player.curHp < player.finalMaxHp)
                    {
                        player.curHp += _item.itemEffect.ValueDic[keys[i]];

                        if (player.curHp > player.finalMaxHp)
                            player.curHp = player.finalMaxHp;

                        SetSlotCount(-1);

                        var obj = Instantiate(Resources.Load<GameObject>("Effect/HpHealingEffect"));
                        Destroy(obj, 2f);
                    }
                    else
                    {
                        print("플레이어의 hp가 최대치입니다.");
                    }
                    break;
                case 2:
                    if (player.curMp < player.finalMaxMp)
                    {
                        player.curMp += _item.itemEffect.ValueDic[keys[i]];

                        if (player.curMp > player.finalMaxMp)
                            player.curMp = player.finalMaxMp;

                        SetSlotCount(-1);

                        var obj = Instantiate(Resources.Load<GameObject>("Effect/MpHealingEffect"));
                        Destroy(obj, 2f);
                    }
                    else
                    {
                        print("플레이어의 mp가 최대치입니다.");
                    }
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_item"></param>
    public void SellItem(Item _item)
    {
        if (_item.Count == 1)
        {
            if (DragSlot.instance.dragSlot != null)
            {
                DragSlot.instance.dragSlot.SetSlotCount(-1);
                DragSlot.instance.SetColorAlpha(0);
                DragSlot.instance.dragSlot = null;
            }
            else
            {
                SetSlotCount(-1);
            }

            player.stats.Gold += _item.SellCost;
        }
        else
        {
            shopMessage.ShowQuantityTxt(_item, 1, this);
        }
    }
}