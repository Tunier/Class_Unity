using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingletone<GameManager>
{
    public GameObject wayPointUI;
    public GameObject inventoryUI;
    public GameObject statusUI;
    public GameObject skilltreeUI;
    public GameObject shopUI;
    public GameObject worldMapUI;
    public GameObject dialogUI;
    public GameObject PauseCanvas;

    public GameObject pauseButtonUI;
    public GameObject settingButtonUI;

    //public GameObject dieText;

    GameObject cameraArm;
    PlayerInfo player;
    PlayerActionCtrl playerAC;
    CharacterController playerCc;
    Inventory inventory;
    Tooltip tooltip;

    public bool isPause = false;
    bool isGamequit = false;

    //public Texture2D[] cursorImg;


    private void Awake()
    {
        cameraArm = GameObject.Find("CameraArm");
        player = GameObject.FindWithTag("Player").GetComponent<PlayerInfo>();
        playerAC = FindObjectOfType<PlayerActionCtrl>();
        playerCc = player.GetComponent<CharacterController>();
        inventory = FindObjectOfType<Inventory>();
        tooltip = FindObjectOfType<Tooltip>();

        isPause = false;
        isGamequit = false;

        //Vector2 mousePos = new Vector2(-2f, 0);
    }
    private void Start()
    {
        //#if UNITY_EDITOR

        //#else
        //        Cursor.SetCursor(cursorImg[0], mousePos, CursorMode.ForceSoftware);
        //#endif
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (wayPointUI.activeSelf || inventoryUI.activeSelf || statusUI.activeSelf || skilltreeUI.activeSelf || shopUI.activeSelf || worldMapUI.activeSelf || dialogUI.activeSelf)
            {
                wayPointUI.SetActive(false);
                inventoryUI.SetActive(false);
                skilltreeUI.SetActive(false);
                statusUI.SetActive(false);
                shopUI.SetActive(false);
                worldMapUI.SetActive(false);

                tooltip.HideTooltip();

                DragSlot.instance.SetColorAlpha(0);
                DragSlot.instance.dragSlot = null;
                DragSlot.instance.dragSkillSlot = null;
            }
            else if (!wayPointUI.activeSelf && !inventoryUI.activeSelf && !statusUI.activeSelf && !skilltreeUI.activeSelf && !shopUI.activeSelf && !worldMapUI.activeSelf)
                isPause = !isPause;
        }

        if (isPause && !isGamequit)
        {
            PauseCanvas.SetActive(true);
            if (!settingButtonUI.activeSelf)
                pauseButtonUI.SetActive(true);

            Time.timeScale = 0;
        }
        else if (!isPause || isGamequit)
        {
            PauseCanvas.SetActive(false);
            settingButtonUI.SetActive(false);

            Time.timeScale = 1;
        }

        //if (player.state == PlayerCtrl.State.DIE)
        //{
        //    dieText.SetActive(true);
        //    if (Input.GetKeyDown(KeyCode.R))
        //    {
        //        SceneManager.LoadScene("01. Stage");
        //    }
        //}
        //else
        //{
        //    dieText.SetActive(false);
        //}

        //#if UNITY_EDITOR

        //#else
        //        if (shop.isBuying)
        //        {
        //            Cursor.SetCursor(GameManager.instance.cursorImg[1], Vector2.zero, CursorMode.ForceSoftware);
        //        }
        //        else if (shop.isSelling)
        //        {
        //            Cursor.SetCursor(GameManager.instance.cursorImg[2], Vector2.zero, CursorMode.ForceSoftware);
        //        }
        //        else
        //        {
        //            Cursor.SetCursor(GameManager.instance.cursorImg[0], Vector2.zero, CursorMode.ForceSoftware);
        //        }
        //#endif
    }

    public void OnPauseClick()
    {
        isPause = !isPause;
    }

    public void OnSettingButtonClick()
    {
        pauseButtonUI.SetActive(false);
        settingButtonUI.SetActive(true);
    }

    public void OnSettingQuitButtonClick()
    {
        pauseButtonUI.SetActive(true);
        settingButtonUI.SetActive(false);
    }

    public void OnPlayerResetPosButtonClick()
    {
        playerCc.enabled = false;
        player.gameObject.transform.position = new Vector3(138.8f, 0, 191.2f);
        player.gameObject.transform.eulerAngles = new Vector3(0, 270, 0);
        cameraArm.transform.eulerAngles = new Vector3(0, -90, 0);
        playerCc.enabled = true;
    }

    public void OnStatusButtonClick()
    {
        statusUI.SetActive(!statusUI.activeSelf);
    }

    public void OnInventoryButtonClick()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
    }

    public void OnSkillTreeButtonClick()
    {
        skilltreeUI.SetActive(!skilltreeUI.activeSelf);
    }

    public void OnClickSaveButton()
    {
        player.SavePlayerInfo();
        playerAC.SaveSkillQuickSlot();
        inventory.SaveInven();
    }
    public void OnExitButtonClick()
    {
        player.SavePlayerInfo();
        playerAC.SaveSkillQuickSlot();
        inventory.SaveInven();
        isGamequit = true;
        LoadingSceneController.Instance.LoadScene("Game_Title_Scene");
    }
}
