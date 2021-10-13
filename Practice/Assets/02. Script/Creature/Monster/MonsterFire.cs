using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���Ͱ� ���Ÿ� ������ ��� �ִϸ��̼� �̺�Ʈ���� ȣ��
/// </summary>
public class MonsterFire : MonoBehaviour
{
    public Transform shotPos;   //ȭ�� �߻� ��ġ

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
        //�׽�Ʈ instantiate
        //var _arrow = Instantiate(arrow,transform.position, Quaternion.identity);

        //�� ��ƼŬ ���
        var _arrow = ObjPoolingManager.Instance.GetObjAtPool(ObjPoolingManager.Obj.GoblinHunterArrow);
        _arrow.GetComponent<ArrowCtrl>().hunter = GetComponent<MonsterHunter>();
        _arrow.transform.position = shotPos.position;
        _arrow.transform.rotation = shotPos.rotation;
        _arrow.SetActive(true);
    }
}