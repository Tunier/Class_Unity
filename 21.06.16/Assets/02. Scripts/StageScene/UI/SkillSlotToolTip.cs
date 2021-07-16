using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotToolTip : MonoBehaviour
{
    public GameObject baseImage;

    [SerializeField]
    Text skillName;
    [SerializeField]
    Text skillLv;
    [SerializeField]
    Text skillDesc;

    [SerializeField]
    RectTransform QuickSkillSlot;

    Vector3 RD_Offset;
    Vector3 RU_Offset;

    private void Awake()
    {
        RD_Offset = new Vector3(baseImage.GetComponent<RectTransform>().rect.width * 0.5f, -baseImage.GetComponent<RectTransform>().rect.height * 0.5f, 0); // 오른쪽 아래로 띄우는 오프셋
        RU_Offset = new Vector3(baseImage.GetComponent<RectTransform>().rect.width * 0.5f, baseImage.GetComponent<RectTransform>().rect.height * 0.5f, 0); // 오른쪽 위로 띄우는 오프셋
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

        skillName.text = _skill.skillName;
        skillLv.text = "Lv : 0";
        skillDesc.text = _skill.skillDescription;
    }

    public void HideToolTip()
    {
        baseImage.SetActive(false);
    }

    public void SetitemNameColor(Color _color)
    {
        skillName.color = _color;
    }

    public void SetitemDescColor(Color _color)
    {
        skillDesc.color = _color;
    }
}
