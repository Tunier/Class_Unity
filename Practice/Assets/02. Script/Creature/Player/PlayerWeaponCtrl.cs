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
        if (other.CompareTag("Monster")) //몬스터가 맞으면
        {
            var curHitMob = other.gameObject;
            player.targetMonster = curHitMob;

            if (!mobList.Contains(curHitMob)) // 맞은 몬스터가 리스트에 없으면
            {
                mobList.Add(curHitMob); // 맞은 몬스터를 리스트에 저장하고

                if (CritcalCalculate()) // 크리티컬이 떴는지 계산해서 Hit를 호출
                {
                    curHitMob.GetComponent<MonsterBase>().Hit(player.finalNormalAtk * 1.5f);
                    // 데미지 UI 출력하는 구문 작성해야함. 크리티컬이 뜨면 해당 UI Text의 컬러를 바꿔주는 기능도 추가해야함.
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
