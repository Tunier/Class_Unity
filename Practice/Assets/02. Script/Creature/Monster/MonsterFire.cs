using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 몬스터가 원거리 공격일 경우 애니메이션 이벤트에서 호출
/// </summary>
public class MonsterFire : MonoBehaviour
{
    public Transform shotPos;   //화살 발사 위치

    private void Awake()
    {
    }

    public void SlashAttack()
    {
        var _slash = ObjPoolingManager.Instance.GetObjAtPool(ObjPoolingManager.Obj.GoblinKingSlah);
        _slash.GetComponent<SpecialAttackCtrl>().goblinKing = GetComponent<MonsterGoblinKing>();
        _slash.transform.position = shotPos.position;
        //_slash.transform.rotation = shotPos.rotation;
        _slash.transform.eulerAngles = new Vector3(90, gameObject.transform.eulerAngles.y, 0);
        _slash.SetActive(true);
    }

    public void ArrowShot()
    {
        //테스트 instantiate
        //var _arrow = Instantiate(arrow,transform.position, Quaternion.identity);

        //쏠때 파티클 제어문
        var _arrow = ObjPoolingManager.Instance.GetObjAtPool(ObjPoolingManager.Obj.GoblinHunterArrow);
        _arrow.GetComponent<ArrowCtrl>().hunter = GetComponent<MonsterHunter>();
        _arrow.transform.position = shotPos.position;
        _arrow.transform.rotation = shotPos.rotation;
        _arrow.SetActive(true);
    }
}