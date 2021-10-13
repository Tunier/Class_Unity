using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingletone<UIManager>
{
    public PlayerInfo player;
    public CharacterController characterController;
    public GameObject go_DamageText;

    public GameObject QuestUI;

    public Canvas BackCanvas;

    public GameObject hotKeyGuid;
    public GameObject hotKeyGuidTarget;
    public Image fadeImg;
    public ButtonCtrl buttonCtrl;

    public List<GameObject> wayPoints = new List<GameObject>();
    public List<GameObject> merchants = new List<GameObject>();

    [SerializeField]
    Text explainTxt;
    [SerializeField]
    GameObject wayPointUI;
    [SerializeField]
    GameObject shopUI;

    public float recognitionRange = 5.5f;

    private void Awake()
    {
        hotKeyGuid.SetActive(false);
    }

    private void Start()
    {
        wayPoints.AddRange(GameObject.FindGameObjectsWithTag("WayPoint"));
        merchants.AddRange(GameObject.FindGameObjectsWithTag("Merchant"));

    }

    private void Update()
    {
        ShowExplainTxt();
        CheckDistance();
        ExitDistance();
    }

    public void ShowDamageText(float _Damage, bool _critical = false)
    {
        var go_damageText = Instantiate(go_DamageText, BackCanvas.transform);

        DamageTextUI DamageText = go_damageText.GetComponent<DamageTextUI>();

        DamageText.mob = player.targetMonster;

        DamageText.SetDamageText(_Damage);

        if (_critical)
        {
            DamageText.SetTextColor(Color.red);
            DamageText.SetTextSize(65);
        }
    }

    public void ShowExplainTxt()
    {
        if (hotKeyGuid.activeSelf)
        {
            if (hotKeyGuidTarget.CompareTag("WayPoint"))
            {
                explainTxt.text = "작동시키기";
            }
            else if (hotKeyGuidTarget.CompareTag("Merchant")) //태그가 마을사람,상점이면 다르게 글자가 나오게
            {
                explainTxt.text = "상점창열기";
            }
            else if (hotKeyGuidTarget.CompareTag("Npc"))
            {
                explainTxt.text = "대화하기";
            }
        }
    }

    private void CheckDistance()
    {
        foreach (GameObject _wayPoint in wayPoints) //웨이포인트 작용 키 활성화
        {
            if (Vector3.Distance(player.transform.position, _wayPoint.transform.position) <= recognitionRange && !wayPointUI.activeSelf)
            {
                hotKeyGuid.SetActive(true);
                hotKeyGuidTarget = _wayPoint;
                return;
            }
            else if (wayPointUI.activeSelf)
            {
                hotKeyGuid.SetActive(false);
                return;
            }
        }

        foreach (GameObject _merchant in merchants) //상점 or NPC 상호작용 키 활성화
        {
            if (Vector3.Distance(player.transform.position, _merchant.transform.position) <= recognitionRange && !shopUI.activeSelf)
            {
                hotKeyGuid.SetActive(true);
                hotKeyGuidTarget = _merchant;
                return;
            }
            else if (shopUI.activeSelf)
            {
                hotKeyGuid.SetActive(false);
                return;
            }
        }

        if (hotKeyGuidTarget != null)
        {
            if (!hotKeyGuidTarget.CompareTag("Npc"))
                hotKeyGuid.SetActive(false);
        }
    }

    /// <summary>
    /// 거리가 멀어지면 UI꺼지게
    /// </summary>
    public void ExitDistance()
    {
        if (wayPointUI.activeSelf)
        {
            if (Vector3.Distance(player.transform.position, hotKeyGuidTarget.transform.position) > recognitionRange
                || hotKeyGuidTarget == null)
            {
                wayPointUI.SetActive(false);
            }
        }
        else if (shopUI.activeSelf)
        {
            if (Vector3.Distance(player.transform.position, hotKeyGuidTarget.transform.position) > recognitionRange
                || hotKeyGuidTarget == null)
            {
                shopUI.SetActive(false);
            }
        }
    }

    /// <summary>
    /// fadein fadeout 코루틴
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeCoroutine(float _darkTime, int _waynumber)
    {
        characterController.enabled = false;
        float fadeCount = 0;
        while (fadeCount < 1.0f)
        {
            fadeCount += 0.03f;
            yield return new WaitForSeconds(0.03f);
            fadeImg.color = new Color(0, 0, 0, fadeCount);
        }
        buttonCtrl.moveWaypoint(_waynumber);
        yield return new WaitForSeconds(_darkTime);
        while (fadeCount > 0)
        {
            fadeCount -= 0.03f;
            yield return new WaitForSeconds(0.03f);
            fadeImg.color = new Color(0, 0, 0, fadeCount);
        }
        characterController.enabled = true;
    }

    public void OnClickQuestUIButton()
    {
        if (QuestUI.GetComponent<RectTransform>().localPosition.x != 750)
            QuestUI.GetComponent<RectTransform>().localPosition = new Vector2(750, 85);
        else
            QuestUI.GetComponent<RectTransform>().localPosition = new Vector2(1100, 85);
    }
}