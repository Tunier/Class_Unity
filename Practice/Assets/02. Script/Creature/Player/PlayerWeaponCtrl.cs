using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponCtrl : MonoBehaviour
{
    public PlayerInfo player;
    PlayerActionCtrl playerAC;

    Collider col;

    public List<GameObject> mobList = new List<GameObject>();

    void Awake()
    {
        col = GetComponent<Collider>();
        col.enabled = false;
        playerAC = FindObjectOfType<PlayerActionCtrl>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster")) //���Ͱ� ������
        {
            var curHitMob = other.gameObject;
            player.targetMonster = curHitMob;

            if (!mobList.Contains(curHitMob)) // ���� ���Ͱ� ����Ʈ�� ������
            {
                mobList.Add(curHitMob); // ���� ���͸� ����Ʈ�� �����ϰ�

                if (CritcalCalculate()) // ũ��Ƽ���� ������ ����ؼ� Hit�� ȣ��
                {
                    curHitMob.GetComponent<MonsterBase>().Hit(player.finalNormalAtk * 1.5f);
                    // ������ UI ����ϴ� ���� �ۼ��ؾ���. ũ��Ƽ���� �߸� �ش� UI Text�� �÷��� �ٲ��ִ� ��ɵ� �߰��ؾ���.
                    UIManager.Instance.ShowDamageText(player.finalNormalAtk * 1.5f, true);

                    player.curHp += player.finalLifeStealPercent * player.finalNormalAtk * 1.5f * 0.01f;
                }
                else
                {
                    curHitMob.GetComponent<MonsterBase>().Hit(player.finalNormalAtk);

                    UIManager.Instance.ShowDamageText(player.finalNormalAtk);

                    player.curHp += player.finalLifeStealPercent * player.finalNormalAtk * 0.01f;
                }
            }
            else { return; }
        }
    }

    public bool CritcalCalculate()
    {
        bool isCrit = false;
        int crit;

        crit = Random.Range(0, 10000);

        if (player.finalCriticalChance >= crit)
            isCrit = true;

        return isCrit;
    }
}
