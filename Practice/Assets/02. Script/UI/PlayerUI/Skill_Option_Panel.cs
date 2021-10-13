using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Skill_Option_Panel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Tooltip tooltip;
    Skill_Tree_UI skillTreeUI;

    private void Awake()
    {
        tooltip = FindObjectOfType<Tooltip>();
        skillTreeUI = FindObjectOfType<Skill_Tree_UI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.ShowTooltip(skillTreeUI.curSlot.skill);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }
}
