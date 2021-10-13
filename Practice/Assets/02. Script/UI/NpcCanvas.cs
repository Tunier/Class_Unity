using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcCanvas : MonoBehaviour
{
    GameObject cameraArm;
    Npc npc;

    [SerializeField]
    Image upperImage;
    [SerializeField]
    Text nameText;

    Sprite bangMark;
    Sprite questionMark;

    void Awake()
    {
        cameraArm = GameObject.Find("CameraArm");
        npc = GetComponentInParent<Npc>();
        bangMark = Resources.Load<Sprite>("UI/57");
        questionMark = Resources.Load<Sprite>("UI/64");

        nameText.text = npc.npcName;
    }

    void Update()
    {
        transform.eulerAngles = new Vector3(0, cameraArm.transform.eulerAngles.y, 0);

        if (npc.questUIDCODE != "")
        {
            if (npc.questUIDCODE == "003")
            {
                if (QuestManager.Instance.QuestDic["001"].State == 3)
                {
                    if (QuestManager.Instance.QuestDic["003"].State == 0)
                    {
                        if (!upperImage.gameObject.activeSelf)
                            upperImage.gameObject.SetActive(true);

                        if (upperImage.sprite != bangMark)
                            upperImage.sprite = bangMark;
                    }
                    else if (QuestManager.Instance.QuestDic["003"].State == 2)
                    {
                        if (!upperImage.gameObject.activeSelf)
                            upperImage.gameObject.SetActive(true);

                        if (upperImage.sprite != questionMark)
                            upperImage.sprite = questionMark;
                    }
                    else
                    {
                        if (upperImage.gameObject.activeSelf)
                            upperImage.gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (upperImage.gameObject.activeSelf)
                        upperImage.gameObject.SetActive(false);
                }
                
                return;
            }

            if (QuestManager.Instance.QuestDic[npc.questUIDCODE].State == 0)
            {
                if (!upperImage.gameObject.activeSelf)
                    upperImage.gameObject.SetActive(true);

                if (upperImage.sprite != bangMark)
                    upperImage.sprite = bangMark;
            }
            else if (QuestManager.Instance.QuestDic[npc.questUIDCODE].State == 2)
            {
                if (!upperImage.gameObject.activeSelf)
                    upperImage.gameObject.SetActive(true);

                if (upperImage.sprite != questionMark)
                    upperImage.sprite = questionMark;
            }
            else
            {
                if (upperImage.gameObject.activeSelf)
                    upperImage.gameObject.SetActive(false);
            }
        }
        else
        {
            if (upperImage.gameObject.activeSelf)
                upperImage.gameObject.SetActive(false);
        }
    }
}
