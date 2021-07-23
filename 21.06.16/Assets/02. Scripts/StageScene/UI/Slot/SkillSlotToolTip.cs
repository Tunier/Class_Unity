using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotToolTip : MonoBehaviour
{
    public GameObject baseImage;

    [SerializeField]
    Text skillNameAndLvText;
    [SerializeField]
    Text coolTimeText;
    [SerializeField]
    Text skillDesc;
    [SerializeField]
    Text mpCostText;

    [SerializeField]
    RectTransform QuickSkillSlot;

    Vector3 RD_Offset;
    Vector3 RU_Offset;

    private void Awake()
    {
        RD_Offset = new Vector3(baseImage.GetComponent<RectTransform>().rect.width * 0.5f, -baseImage.GetComponent<RectTransform>().rect.height * 0.5f, 0); // ������ �Ʒ��� ���� ������
        RU_Offset = new Vector3(baseImage.GetComponent<RectTransform>().rect.width * 0.5f, baseImage.GetComponent<RectTransform>().rect.height * 0.5f, 0); // ������ ���� ���� ������
    }

    void Start()
    {

    }

    void Update()
    {
        if (baseImage.activeSelf)
        {
            if (Input.mousePosition.y >= baseImage.GetComponent<RectTransform>().rect.height)
                baseImage.transform.position = Input.mousePosition + RD_Offset;
            else
                baseImage.transform.position = Input.mousePosition + RU_Offset;
        }
    }

    public void ShowToolTip(Skill _skill)
    {
        baseImage.SetActive(true);

        skillNameAndLvText.text = _skill.skillName + "(1����)";
        coolTimeText.text = "��Ÿ�� : " + _skill.coolTime;
        skillDesc.text = _skill.skillDescription;
        mpCostText.text = "���� : " + _skill.mpCost;
    }

    public void HideToolTip()
    {
        baseImage.SetActive(false);
    }
}
