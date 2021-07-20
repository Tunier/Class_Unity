using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
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

    /// <summary>
    /// ��ų ���Կ� ��ų�� �־��ְ�, �̹����� ��ų �̹����� �ٲ��ְ�, �ش� ��ų��Ÿ�Ӱ� ���� ��Ÿ���� �Ѱ���.
    /// </summary>
    /// <param name="_skill"></param>
    /// <param name="curCooltime"></param>
    public void AddSkillIcon(Skill _skill, float curCooltime)
    {
        skill = _skill;
        skillImage.sprite = _skill.skillImage;
        skillCooltime = _skill.coolTime;
        currentSkillCoolTime = curCooltime;

        // �ش� ��ų�� ��Ÿ�� �̶�� ��Ÿ�� �̹����� active ��Ű��
        // ��Ÿ�� ǥ�� �ǰ� �ϴ� ��� �־����.

        SetColorAlpha(1);
    }

    void ClearSlot()
    {
        skill = null;
        skillCooltime = 0f;
        currentSkillCoolTime = 0f;
        skillImage.sprite = null;
        SetColorAlpha(0);

        cooltimeText.text = "0";
        cooldownImage.SetActive(false);
    }

    private void Update()
    {
        cooldownImage.SetActive(currentSkillCoolTime > 0f ? true : false);

        if (currentSkillCoolTime > 0f)
            currentSkillCoolTime -= Time.deltaTime;
        else if (currentSkillCoolTime < 0f)
            currentSkillCoolTime = 0f;

        cooltimeText.text = currentSkillCoolTime.ToString("F0");

        cooldownImage.GetComponent<Image>().fillAmount = currentSkillCoolTime / skillCooltime;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (skill != null)
            {
                DragSlot.instance.dragSkillSlot = this;
                DragSlot.instance.DragSetImage(skillImage);
                DragSlot.instance.transform.position = eventData.position;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (skill != null)
            DragSlot.instance.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!RectTransformUtility.RectangleContainsScreenPoint(QuickSkillSlotBase, Input.mousePosition))
        {
            ClearSlot();
        }

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
        float curCoolTime = currentSkillCoolTime;

        AddSkillIcon(DragSlot.instance.dragSkillSlot.skill, DragSlot.instance.dragSkillSlot.currentSkillCoolTime);

        if (_skill != null)
            DragSlot.instance.dragSkillSlot.AddSkillIcon(_skill, curCoolTime);
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
