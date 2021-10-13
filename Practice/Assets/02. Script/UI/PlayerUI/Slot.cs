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
    /// ���Կ� �������� �߰��ϰ�, �����ۿ� ���� �ε��� ������ �־��ְ�, ���Կ� �̹����� �߰���.
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
            case 0: // �Ϲ�
                slot_BG.sprite = Resources.Load<Sprite>(common_SlotBG_Path);
                break;
            case 1: // ����
                slot_BG.sprite = Resources.Load<Sprite>(rare_SlotBG_Path);
                break;
            case 2: // ����ũ
                slot_BG.sprite = Resources.Load<Sprite>(unique_SlotBG_Path);
                break;
            case 3: // ����
                slot_BG.sprite = Resources.Load<Sprite>(epic_SlotBG_Path);
                break;
            case 4: // ��Ʈ
                slot_BG.sprite = Resources.Load<Sprite>(set_SlotBG_Path);
                break;
        }
    }

    /// <summary>
    /// �ش罽���� ������ ī��Ʈ�� �μ��� ��ŭ ������. ������ ī��Ʈ�� 0�̵Ǹ� ClearSlot �Լ� �ߵ�.
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
    /// �ش�ĭ�� �����. (item = null, itemCount = 0, ��������Ʈ = null, ���İ� 0����)
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
    /// ���� ��Ŭ���� ���Կ� �ִ� ������ Ÿ���� ���� �Ҹ�ǰ�̸� ������ ���,
    /// ���� �������ͽ�â�� �ش� ���Ÿ��ĭ�� ����.
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
                            Debug.Log("����� ��Ŭ�� - ȿ������");
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
    /// �巹�� ���۽� �巹�� ������ ���Կ� �ִ� ������ �巹�� ���Կ� ����
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
    /// �巹�� �ϴ� ���� ������ ����
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
            DragSlot.instance.transform.position = eventData.position;
    }

    /// <summary>
    /// �巹�� ������ �巹�� ���� ���İ� 0���� �ٲٰ� �����.<br/>
    /// �巹�� ���� ��ġ�� ����â�̳� �κ�â ���̸� ������ ���â �ҷ���.
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
    /// ��ӵ� ������ ���������� ����̹ߵ���.<br/>
    /// �κ��丮 ������ ChangeSlot �Լ� �ߵ�. ��� ������ ������� �ߵ�.
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
                    // ����� ���� ���Ÿ���� ���� ���� ������ �����Ƿ� ����.
                    // ���Ŀ� ����� ���� �ۼ� �ʿ�.
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
    /// ���� ��ü�Ҷ� ������ ������ ���� �ȱ�⸸ ����, ���� �ٲ������� �����ؼ� ó������.
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
    /// ������ ������ �������.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemCount != 0)
            tooltip.ShowTooltip(item);
    }

    /// <summary>
    /// ���� ��������.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }

    /// <summary>
    /// �������� ���Կ��� ���ŵǰ�, ���ĭ�� ������ Ÿ�Կ� ���� ������.
    /// ���ĭ�� �̹� �������� ������� ��ȯ����.
    /// </summary>
    /// <param name="_item"></param>
    public void EquipItem(Item _item)
    {
        if (player.stats.Level < _item.itemEffect.RequireValueDic[1])
        {
            Debug.Log("������ �������ѿ� �ɸ��ϴ�.");
            return;
        }

        SetSlotCount(-1);

        #region ������ Ÿ�Կ����� ��񽽷Կ� �����ϴ� ����.
        switch (_item.Type)
        {
            // 0: �Ѽչ���, 1: �μչ���, 2: ���, 3: ����, 4: ��Ʈ, 5:�尩
            // 6: �Ź�, 7: �����, 8: ����, 9: �Һ�, 10: ���

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

        #region ������ ȿ���� �޾ƿͼ� ���� ���ݿ� �����ϴ� ����.
        List<int> keys = new List<int>();
        keys.AddRange(_item.itemEffect.ValueDic.Keys);

        for (int i = 0; i < keys.Count; i++)
        {
            // 0: ����, 1: ���� Hp, 2: ���� Mp, 3: �ִ� Hp ������, 4: �ִ� Hp %��, 5: �ִ� Mp ������, 6: �ִ� Mp %��,
            // 7: ���� ���ݷ� ������, 8: ���� ���ݷ� %��, 9: ���� ���� ������, 10: ���� ���� %��, 11: �� ������, 12: �� %��,
            // 13: ���� ������, 14: ���� %��, 15: ���ݽ� ����� ȸ�� ������, 16: ���ݽ� �������� %��ŭ ����� ȸ��,
            // 17: ���� ���ݷ� ������, 18: ���� ���ݷ� %��, 19: ���� ���� ������, 20: ���� ���� %��

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
    /// �κ�â�� �������� �߰��ǰ�, ����â�� �ִ� ��񽽷��� ������ �ټ��� -1 (0�̵Ǽ� ClearSlot�Լ������)
    /// ������ �μ��� �ְԵǸ� �ش罽�Կ� ���Ե�.
    /// </summary>
    /// <param name="_item"></param>
    public void UnEquipItem(Item _item, Slot _slot = null)
    {
        SetSlotCount(-1);

        #region ������ ȿ�� ����(���ҽ�Ű��)�ϴ� �κ�
        List<int> keys = new List<int>();
        keys.AddRange(_item.itemEffect.ValueDic.Keys);

        for (int i = 0; i < keys.Count; i++)
        {
            // 0: ����, 1: ���� Hp, 2: ���� Mp, 3: �ִ� Hp ������, 4: �ִ� Hp %��, 5: �ִ� Mp ������, 6: �ִ� Mp %��,
            // 7: ���� ���ݷ� ������, 8: ���� ���ݷ� %��, 9: ���� ���� ������, 10: ���� ���� %��, 11: �� ������, 12: �� %��,
            // 13: ���� ������, 14: ���� %��, 15: ���ݽ� ����� ȸ�� ������, 16: ���ݽ� �������� %��ŭ ����� ȸ��,
            // 17: ���� ���ݷ� ������, 18: ���� ���ݷ� %��, 19: ���� ���� ������, 20: ���� ���� %��

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
        #region ������ Ÿ�Կ����� ��񽽷Կ� �����ϴ� ����.
        switch (_item.Type)
        {
            // 0: �Ѽչ���, 1: �μչ���, 2: ���, 3: ����, 4: ��Ʈ, 5:�尩
            // 6: �Ź�, 7: �����, 8: ����, 9: �Һ�, 10: ���

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

        #region ������ ȿ���� �޾ƿͼ� ���� ���ݿ� �����ϴ� ����.
        List<int> keys = new List<int>();
        keys.AddRange(_item.itemEffect.ValueDic.Keys);

        for (int i = 0; i < keys.Count; i++)
        {
            // 0: ����, 1: ���� Hp, 2: ���� Mp, 3: �ִ� Hp ������, 4: �ִ� Hp %��, 5: �ִ� Mp ������, 6: �ִ� Mp %��,
            // 7: ���� ���ݷ� ������, 8: ���� ���ݷ� %��, 9: ���� ���� ������, 10: ���� ���� %��, 11: �� ������, 12: �� %��,
            // 13: ���� ������, 14: ���� %��, 15: ���ݽ� ����� ȸ�� ������, 16: ���ݽ� �������� %��ŭ ����� ȸ��,
            // 17: ���� ���ݷ� ������, 18: ���� ���ݷ� %��, 19: ���� ���� ������, 20: ���� ���� %��

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
            // 0: ����, 1: ���� Hp, 2: ���� Mp, 3: �ִ� Hp ������, 4: �ִ� Hp %��, 5: �ִ� Mp ������, 6: �ִ� Mp %��,
            // 7: ���� ���ݷ� ������, 8: ���� ���ݷ� %��, 9: ���� ���� ������, 10: ���� ���� %��, 11: �� ������, 12: �� %��,
            // 13: ���� ������, 14: ���� %��, 15: ���ݽ� ����� ȸ�� ������, 16: ���ݽ� �������� %��ŭ ����� ȸ��,
            // 17: ���� ���ݷ� ������, 18: ���� ���ݷ� %��, 19: ���� ���� ������, 20: ���� ���� %��

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
                        print("�÷��̾��� hp�� �ִ�ġ�Դϴ�.");
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
                        print("�÷��̾��� mp�� �ִ�ġ�Դϴ�.");
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