using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Skill skill;
    public int slotIndex;
    public Image skillImage;
    public Text skillTreeLvText = null;
    public float curCooltime;
    public GameObject cooldownImage;
    public Text cooldownText;
    public bool haveSkill = false;

    Tooltip tooltip;
    Skill_Tree_UI skillTreeUI;

    PlayerInfo player;
    PlayerActionCtrl playerActl;

    private void Awake()
    {
        tooltip = FindObjectOfType<Tooltip>();
        skillTreeUI = FindObjectOfType<Skill_Tree_UI>();

        player = FindObjectOfType<PlayerInfo>();
        playerActl = FindObjectOfType<PlayerActionCtrl>();
    }

    private void Start()
    {
        skillTreeUI.skillOptionPanel.SetActive(false);
    }

    private void Update()
    {
        if (CompareTag("QuickSkillSlot"))
        {
            if (skill != null)
            {
                if (curCooltime > 0)
                {
                    cooldownImage.SetActive(true);
                    cooldownImage.GetComponent<Image>().fillAmount = curCooltime / skill.CoolTime;
                    cooldownText.text = (Mathf.FloorToInt(curCooltime)).ToString();
                }
                else if (curCooltime <= 0)
                {
                    if (cooldownImage.activeSelf)
                    {
                        cooldownImage.SetActive(false);
                        cooldownText.text = "0";
                    }
                }
            }
            else
            {
                if (cooldownImage.activeSelf)
                {
                    cooldownImage.SetActive(false);
                    curCooltime = 0;
                    cooldownText.text = "0";
                }
            }
        }

        if (gameObject.CompareTag("QuickSkillSlot") && haveSkill)
            curCooltime = playerActl.curSkillCooltime[skill.UIDCODE];

        if (gameObject.CompareTag("SkillTreeSlot"))
        { 
            skillTreeLvText.text = player.player_Skill_Dic[skill.UIDCODE].ToString();
            if (player.player_Skill_Dic[skill.UIDCODE] == 0)
            { 

            }
        }
    }

    void SetColorAlpha(float alpha)
    {
        Color color = skillImage.color;
        color.a = alpha;
        skillImage.color = color;
    }

    /// <summary>
    /// 슬롯에 스킬을 추가하고, 스킬 이미지를 뜨게함.
    /// </summary>
    /// <param name="_item"></param>
    /// <param name="count"></param>
    public void AddSkill(Skill _skill)
    {
        skill = _skill;
        skill.slotIndex = slotIndex;
        skillImage.sprite = Resources.Load<Sprite>(_skill.SkillImagePath);
        SetColorAlpha(1);
        haveSkill = true;

        if (gameObject.CompareTag("QuickSkillSlot"))
            curCooltime = playerActl.curSkillCooltime[_skill.UIDCODE];
    }

    private void ClearSlot()
    {
        skill = null;
        skillImage.sprite = null;
        haveSkill = false;
        curCooltime = 0;
        SetColorAlpha(0);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (gameObject.CompareTag("SkillTreeSlot"))
            {
                skillTreeUI.skillOptionPanel.transform.localPosition = new Vector3(transform.localPosition.x + 61.4f, transform.localPosition.y - 44, 0);
                skillTreeUI.skillOptionPanel.SetActive(true);
                skillTreeUI.curSlot = this;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (skill.Type != 0)
            {
                DragSlot.instance.dragSkillSlot = this;
                DragSlot.instance.DragSetImage(skillImage);
                DragSlot.instance.transform.position = eventData.position;
            }
            else
            {
                DragSlot.instance.SetColorAlpha(0);
                DragSlot.instance.dragSkillSlot = null;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        if (haveSkill == true)
            DragSlot.instance.transform.position = eventData.position;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!EventSystem.current.IsPointerOverGameObject() && gameObject.CompareTag("QuickSkillSlot"))
        {
            if (DragSlot.instance.dragSkillSlot != null)
            {
                DragSlot.instance.SetColorAlpha(0);
                DragSlot.instance.dragSkillSlot = null;
            }
            ClearSlot();
        }

        DragSlot.instance.SetColorAlpha(0);
        DragSlot.instance.dragSkillSlot = null;
    }

    /// <summary>
    /// 스킬 있으면 툴팁출력.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (haveSkill == true)
            tooltip.ShowTooltip(skill);
    }

    /// <summary>
    /// 툴팁 꺼지게함.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (gameObject.CompareTag("QuickSkillSlot"))
        {
            if (DragSlot.instance.dragSkillSlot != null)
            {
                if (DragSlot.instance.dragSkillSlot.CompareTag("SkillTreeSlot"))
                {
                    for (int i = 0; i < playerActl.skillSlot.Count; i++)
                    {
                        if (playerActl.skillSlot[i].haveSkill)
                        {
                            if (playerActl.skillSlot[i].skill.UIDCODE == DragSlot.instance.dragSkillSlot.skill.UIDCODE)
                            {
                                AddSkill(playerActl.skillSlot[i].skill);
                                curCooltime = playerActl.skillSlot[i].curCooltime;

                                playerActl.skillSlot[i].ClearSlot();
                                playerActl.skillSlot[i].curCooltime = 0;

                                tooltip.ShowTooltip(skill);

                                return;
                            }
                        }
                    }

                    AddSkill(DragSlot.instance.dragSkillSlot.skill);

                    tooltip.ShowTooltip(skill);
                }
                else if (DragSlot.instance.dragSkillSlot.CompareTag("QuickSkillSlot"))
                {
                    ChangeSlot();
                }
            }
        }
    }

    private void ChangeSlot()
    {
        if (!haveSkill)
        {
            if (DragSlot.instance.dragSkillSlot.haveSkill)
            {
                AddSkill(DragSlot.instance.dragSkillSlot.skill);
                curCooltime = DragSlot.instance.dragSkillSlot.curCooltime;
                DragSlot.instance.dragSkillSlot.ClearSlot();
                DragSlot.instance.dragSkillSlot = null;
            }
        }
        else
        {
            Skill _skill = skill;
            float _curCooltime = curCooltime;

            AddSkill(DragSlot.instance.dragSkillSlot.skill);
            curCooltime = DragSlot.instance.dragSkillSlot.curCooltime;

            DragSlot.instance.dragSkillSlot.AddSkill(_skill);
            DragSlot.instance.dragSkillSlot.curCooltime = _curCooltime;
        }

        tooltip.ShowTooltip(skill);
    }
}
