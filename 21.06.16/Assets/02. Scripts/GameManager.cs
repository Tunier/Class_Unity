using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameObject pauseCanvas;
    public GameObject statusUI;
    public GameObject inventoryUI;
    public GameObject shopUI;
    public GameObject wayPointUI;

    ItemEffectDatebase database;
    Shop shop;

    [SerializeField]
    RectTransform invenBase;
    [SerializeField]
    RectTransform statusBase;

    [SerializeField]
    SlotToolTip toolTip;

    [SerializeField]
    GameObject quickSlotParent;
    [SerializeField]
    Slot[] quickSlots;

    public GameObject dieText;
    public PlayerCtrl player;

    public bool isPause = false;

    public Texture2D[] cursorImg;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
                Destroy(gameObject);
        }
    }

    private void Start()
    {
        player = GameObject.FindWithTag("PLAYER").GetComponent<PlayerCtrl>();

        database = FindObjectOfType<ItemEffectDatebase>();
        shop = FindObjectOfType<Shop>();
        quickSlots = quickSlotParent.GetComponentsInChildren<Slot>();

        isPause = false;

        inventoryUI.SetActive(false);
        statusUI.SetActive(false);

#if UNITY_EDITOR

#else
        Cursor.SetCursor(cursorImg[0], Vector2.zero, CursorMode.ForceSoftware);
#endif
    }

    private void Update()
    {
        PlayerPrefs.Save();

        UIHotKey();

        if (isPause)
        {
            Time.timeScale = 0f;
            pauseCanvas.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            pauseCanvas.SetActive(false);
        }

        if (player.state == PlayerCtrl.State.DIE)
        {
            dieText.SetActive(true);
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("01. Stage");
            }
        }
        else
        {
            dieText.SetActive(false);
        }

#if UNITY_EDITOR

#else
        if (shop.isBuying)
        {
            Cursor.SetCursor(GameManager.instance.cursorImg[1], Vector2.zero, CursorMode.ForceSoftware);
        }
        else if (shop.isSelling)
        {
            Cursor.SetCursor(GameManager.instance.cursorImg[2], Vector2.zero, CursorMode.ForceSoftware);
        }
        else
        {
            Cursor.SetCursor(GameManager.instance.cursorImg[0], Vector2.zero, CursorMode.ForceSoftware);
        }
#endif
    }

    /// <summary>
    /// 인벤토리, 스텟창, 퀵슬롯 아이템 사용 등의 키보드 입력을 처리함.
    /// </summary>
    void UIHotKey()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);

            if (!inventoryUI.activeSelf && RectTransformUtility.RectangleContainsScreenPoint(invenBase, Input.mousePosition))
                toolTip.HideToolTip();
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            statusUI.SetActive(!statusUI.activeSelf);

            if (!statusUI.activeSelf && RectTransformUtility.RectangleContainsScreenPoint(statusBase, Input.mousePosition))
                toolTip.HideToolTip();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!inventoryUI.activeSelf && !statusUI.activeSelf && !shopUI.activeSelf && !wayPointUI.activeSelf)
            {
                isPause = !isPause;
            }
            else
            {
                inventoryUI.SetActive(false);
                statusUI.SetActive(false);
                shopUI.SetActive(false);
                wayPointUI.SetActive(false);

                toolTip.HideToolTip();
                DragSlot.instance.SetColorAlpha(0);
                DragSlot.instance.dragSlot = null;
            }
        }
        else if (Input.GetKeyDown(KeyCode.F10))
        {
            isPause = !isPause;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            QuickSlotUseItem(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            QuickSlotUseItem(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            QuickSlotUseItem(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            QuickSlotUseItem(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            QuickSlotUseItem(4);
        }
    }

    public void OnPauseClick()
    {
        isPause = !isPause;
    }

    public void OnStatusBottonClick()
    {
        statusUI.SetActive(!statusUI.activeSelf);
    }

    public void OnInventoryBottonClick()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
    }

    void QuickSlotUseItem(int i)
    {
        if (quickSlots[i].item != null)
        {
            if (quickSlots[i].item.itemType == Item.ItemType.Equipment)
            {
                quickSlots[i].EquipItem(quickSlots[0].item);
                return;
            }

            database.UseItem(quickSlots[i].item);

            if (quickSlots[i].item.itemType == Item.ItemType.Used)
                quickSlots[i].SetSlotCount(-1);
        }
    }
}
