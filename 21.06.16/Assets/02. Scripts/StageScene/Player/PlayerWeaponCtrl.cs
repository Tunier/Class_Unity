using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponCtrl : MonoBehaviour
{
    public PlayerCtrl player;
    public GameObject hitmob;
    public List<GameObject> mobList;

    public float weaponDamage = 0;

    enum Elimental
    { 
        None,
        Fire,
        Ice,
        Lightning,
    }

    Elimental eli = Elimental.None;

    void Start()
    {
        mobList = new List<GameObject>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MONSTER")) //���Ͱ� ������
        {
            hitmob = other.gameObject;

            player.hitmob = hitmob.GetComponent<MonsterCtrl>(); // ���� ���� ������ �÷��̾�� ����

            if (!mobList.Contains(other.gameObject)) // ���� ���Ͱ� ����Ʈ�� ������
            {
                mobList.Add(other.gameObject); // ���� ���͸� ����Ʈ�� �����ϰ�

                if (player.CritCal()) // ũ��Ƽ���� ������ ����ؼ� Hit�� ȣ��
                {
                    hitmob.GetComponent<MonsterCtrl>().Hit(player.resultDamage * 1.5f, true);
                }
                else
                {
                    hitmob.GetComponent<MonsterCtrl>().Hit(player.resultDamage, false);
                }
            }
            else { return; }
        }
    }
}
