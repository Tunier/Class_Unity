using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public List<GameObject> mobList;
    public GameObject mobSpawn;

    public GameObject pauseCanvas;
    public GameObject statusUI;
    public GameObject inventoryUI;
    FadeCtrl fadectrl;

    ItemEffectDatebase database;
    Status status;

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
        fadectrl = FindObjectOfType<FadeCtrl>();

        mobList = new List<GameObject>();
        status = FindObjectOfType<Status>();
        database = FindObjectOfType<ItemEffectDatebase>();
        quickSlots = quickSlotParent.GetComponentsInChildren<Slot>();

        isPause = false;
    }

    private void Update()
    {
        PlayerPrefs.Save();

        UIHotKey();
        OnOffMonsterSpawn();

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
                SceneManager.LoadScene("Stage");
            }
        }
        else
        {
            dieText.SetActive(false);
        }
    }

    void UIHotKey()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);

            if (Input.mousePosition.y >= toolTip.baseImage.GetComponent<RectTransform>().rect.height)
            {
                if (!inventoryUI.activeSelf && RectTransformUtility.RectangleContainsScreenPoint(invenBase, toolTip.baseImage.transform.position - toolTip.RD_Offset))
                    toolTip.HideToolTip();
            }
            else
            {
                if (!inventoryUI.activeSelf && RectTransformUtility.RectangleContainsScreenPoint(statusBase, toolTip.baseImage.transform.position - toolTip.RU_Offset))
                    toolTip.HideToolTip();
            }

        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            statusUI.SetActive(!statusUI.activeSelf);

            if (Input.mousePosition.y >= toolTip.baseImage.GetComponent<RectTransform>().rect.height)
            {
                if (!inventoryUI.activeSelf && RectTransformUtility.RectangleContainsScreenPoint(invenBase, toolTip.baseImage.transform.position - toolTip.RD_Offset))
                    toolTip.HideToolTip();
            }
            else
            {
                if (!inventoryUI.activeSelf && RectTransformUtility.RectangleContainsScreenPoint(statusBase, toolTip.baseImage.transform.position - toolTip.RU_Offset))
                    toolTip.HideToolTip();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = !isPause;
        }
        if (Input.GetKeyDown(KeyCode.F10))
        {
            isPause = !isPause;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            QuickSlotUseItem(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            QuickSlotUseItem(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            QuickSlotUseItem(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            QuickSlotUseItem(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            QuickSlotUseItem(4);
        }
    }

    public void OnOffMonsterSpawn()
    {
        if (mobList.Count >= 4)
        {
            mobSpawn.SetActive(false);
        }
        else
        {
            mobSpawn.SetActive(true);
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
