using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnerCtrl : MonoBehaviour
{
    public List<GameObject> mobList = new List<GameObject>();

    public GameObject mob;
    public PlayerCtrl player;

    float spawnDelay;
    float spawnAfterTime;

    int spawnMobLevel;

    Vector3 mobPos; // x -13 ~ -3 / z 6 ~ 17 //���Ŀ� ����Ʈ �����ؼ� ��ȯ���� �����Ұ�

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerCtrl>();
        spawnAfterTime = 7f;
        spawnDelay = 8f;
        spawnMobLevel = 1;
    }

    void Update()
    {
        OnOffMonsterSpawn();

        mobPos.x = Random.Range(-13f, -3f);
        mobPos.y = 0f;
        mobPos.z = Random.Range(6f, 17f);
    }

    /// <summary>
    /// ��ü �ʿ���(���߿� ������ �����ϴ°����� �����Ұ�) �����ϴ� ���͸� ����Ʈ�� �޾Ƽ�<br/>
    /// 4���� ���ϸ� ���� ������ �����ؼ� ���� ��ġ �ȿ� ������������ ��ȯ��.
    /// </summary>
    public void OnOffMonsterSpawn()
    {
        if (mobList.Count <= 4)
        {
            if (player.level > spawnMobLevel + 1)
            {
                spawnMobLevel += 1;
            }

            spawnAfterTime += Time.deltaTime;
            if (spawnAfterTime >= spawnDelay)
            {
                spawnAfterTime = 0;
                GameObject obj = Instantiate(mob, mobPos, Quaternion.Euler(new Vector3(0, 180, 0)));

                MonsterCtrl MobCtrl = obj.GetComponent<MonsterCtrl>();
                MobCtrl.level = spawnMobLevel;
                MobCtrl.hpMax = (MobCtrl.level - 1) * 25f + 30;
                MobCtrl.hp = MobCtrl.hpMax;

                mobList.Add(obj);
            }
        }
        else
        {
            return;
        }
    }
}
