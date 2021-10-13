using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpCanvas : MonoBehaviour
{
    GameObject CameraArm;
    MonsterBase monster;

    [SerializeField]
    Image recognize;
    [SerializeField]
    Image hpFill;
    [SerializeField]
    Text monsterName;


    private void Awake()
    {
        CameraArm = GameObject.Find("CameraArm");
        monster = GetComponentInParent<MonsterBase>();
        recognize.enabled = false;
    }

    private void Start()
    {
        monsterName.text = monster.stats.s_Name;
    }

    void LateUpdate()
    {
        Vector3 Rot = new Vector3(0, CameraArm.transform.eulerAngles.y, 0);
        transform.eulerAngles = Rot;

        hpFill.fillAmount = monster.curHp / monster.finalMaxHp;

        Recognize();
    }

    void Recognize()
    {
        if (monster.state == STATE.Attacking || monster.state == STATE.Chase || monster.state == STATE.Backing)
        {
            recognize.enabled = true;
        }
        else
        {
            recognize.enabled = false;
        }
    }
}
