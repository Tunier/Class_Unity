using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Skill skill;
    public float skillCooltime;
    public float currentSkillCoolTime;
    public Image skillImage;

    [SerializeField]
    GameObject cooldownImage;
    [SerializeField]
    Text cooltimeText;

    [SerializeField]
    RectTransform QuickSkillSlotBase;

    [SerializeField]
    SkillSlotToolTip toolTip;

    void SetColorAlpha(float alpha)
    {
        Color color = skillImage.color;
        color.a = alpha;
        skillImage.color = color;
    }

    public void AddSkillIcon(Skill _skill)
    {
        skill = _skill;
        skillImage.sprite = _skill.skillImage;

        // 해당 스킬이 쿨타임 이라면 쿨타임 이미지를 active 시키고
        // 쿨타임 표시 되게 하는 기능 넣어야함.

        SetColorAlpha(1);
    }

    void ClearSlot()
    {
        skill = null;
        skillCooltime = 0;
        skillImage.sprite = null;
        SetColorAlpha(0);

        cooltimeText.text = "0";
        cooldownImage.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("클릭");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (skill != null)
        {
            DragSlot.instance.dragSkillSlot = this;
            DragSlot.instance.DragSetImage(skillImage);
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (skill != null)
            DragSlot.instance.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlot.instance.SetColorAlpha(0);
        DragSlot.instance.dragSkillSlot = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSkillSlot != null)
            ChangeSlot();
    }

    private void ChangeSlot()
    {
        Skill _skill = skill;

        AddSkillIcon(DragSlot.instance.dragSkillSlot.skill);

        if (_skill != null)
            DragSlot.instance.dragSkillSlot.AddSkillIcon(_skill);
        else
            DragSlot.instance.dragSkillSlot.ClearSlot();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (skill != null)
            toolTip.ShowToolTip(skill);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTip.HideToolTip();
    }
}
