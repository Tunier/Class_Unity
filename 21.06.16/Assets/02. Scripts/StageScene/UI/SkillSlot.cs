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
        throw new System.NotImplementedException();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnDrop(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
