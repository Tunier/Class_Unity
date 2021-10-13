using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

interface iPlayerMustHaveFuc
{
    public abstract void LevelUp();

    public abstract void RefeshFinalStats();

    public abstract void SavePlayerInfo();

    public abstract void LoadPlayerInfo();
}

public class PlayerInfo : Creature, iPlayerMustHaveFuc
{
    public Dictionary<string, int> player_Skill_Dic = new Dictionary<string, int>();

    [HideInInspector]
    public GameObject targetMonster;

    public GameObject cameraArm;
    public Inventory inven;
    public GameObject levelUpEffect;

    public float ItemEffectMaxHp;
    public float SkillEffectMaxHp;
    public float ItemEffectMaxHpMultiplier;
    public float SkillEffectMaxHpMultiplier;

    public float ItemEffectHpRegen;
    public float SkillEffectHpRegen;
    public float ItemEffectHpRegenMultiplier;
    public float SkillEffectHpRegenMultiplier;

    public float finalMaxMp { get; protected set; }
    public float curMp;
    public float ItemEffectMaxMp;
    public float SkillEffectMaxMp;
    public float ItemEffectMaxMpMultiplier;
    public float SkillEffectMaxMpMultiplier;

    public float finalMpRegen { get; protected set; }
    public float ItemEffectMpRegen;
    public float SkillEffectMpRegen;
    public float ItemEffectMpRegenMultiplier;
    public float SkillEffectMpRegenMultiplier;

    public float finalStr { get; protected set; }
    public float ItemEffectStr;
    public float SkillEffectStr;
    public float ItemEffectStrMultiplier;
    public float SkillEffectStrMultiplier;

    public float finalInt { get; protected set; }
    public float ItemEffectInt;
    public float SkillEffectInt;
    public float ItemEffectIntMultiplier;
    public float SkillEffectIntMultiplier;

    public float ItemEffectAtk;
    public float SkillEffectAtk;
    public float ItemEffectAtkMultiplier;
    public float SkillEffectAtkMultiplier;

    public float ItemEffectDef;
    public float SkillEffectDef;
    public float ItemEffectDefMultiplier;
    public float SkillEffectDefMultiplier;

    public float finalLifeSteal { get; protected set; }
    public float ItemEffectLifeSteal;
    public float SkillEffectLifeSteal;

    public float finalLifeStealPercent { get; protected set; }
    public float ItemEffectLifeStealPercent;
    public float SkillEffectLifeStealPercent;

    public int finalCriticalChance { get; protected set; }
    public int ItemEffectCriticalChance;
    public int SkillEffectCriticalChace;

    public float finalCriticalDamageMuliplie { get; protected set; }
    public float ItemEffectCriticalDamageMultiple;
    public float SkillEffectCriticalDamageMultiple;

    SkillDatabase skillDB;

    const string PlayerDataPath = "/Resources/Data/PlayerData.text";
    const string PlayerSkillDataPath = "/Resources/Data/PlayerSkillData.text";

    CharacterController cController;
    Animator ani;

    [HideInInspector]
    public bool debugMode = false;
    [HideInInspector]
    public bool infinityMana = false;

    void Awake()
    {
        skillDB = FindObjectOfType<SkillDatabase>();
        cController = GetComponent<CharacterController>();
        ani = GetComponent<Animator>();

        LoadPlayerInfo();

        RefeshFinalStats();
    }

    private void Start()
    {
        List<string> keys = new List<string>();
        keys.AddRange(player_Skill_Dic.Keys);
        //foreach (string key in keys)
        //{
        //    Debug.Log(key);
        //}

        foreach (string skill_UID in keys)
        {
            skillDB.UsePassiveSkillOnLoad(skillDB.AllSkillDic[skill_UID], player_Skill_Dic[skill_UID], gameObject);
        }

        // �����Ҷ� Ǯ��, Ǯ������ �������.
        curHp = finalMaxHp;
        curMp = finalMaxMp;
    }

    private void Update()
    {
        if (state != STATE.Die)
        {
            if (curHp < finalMaxHp)
                curHp += finalHpRegen * Time.deltaTime;
            else
                curHp = finalMaxHp;

            if (curMp < finalMaxMp)
                curMp += finalMpRegen * Time.deltaTime;
            else
                curMp = finalMaxMp;
        }
        else if (state == STATE.Die)
        {
            finalLifeSteal = 0;
            finalLifeStealPercent = 0;
        }

        if (curHp < 0)
            curHp = 0;

        if (curMp < 0)
            curMp = 0;

        //if (stats.CurExp >= stats.MaxExp)
        //LevelUp();
    }

    /// <summary>
    /// ���������� ����� ���ݵ� ������.
    /// </summary>
    public virtual void RefeshFinalStats()
    {
        finalMaxHp = (stats.MaxHp + (ItemEffectMaxHp + SkillEffectMaxHp)) * (1 + ItemEffectMaxHpMultiplier + SkillEffectMaxHpMultiplier);
        finalMaxMp = (stats.MaxMp + (ItemEffectMaxMp + SkillEffectMaxMp)) * (1 + ItemEffectMaxMpMultiplier + SkillEffectMaxMpMultiplier);
        finalNormalAtk = (stats.Str + (ItemEffectAtk + SkillEffectAtk)) * (1 + ItemEffectAtkMultiplier + SkillEffectAtkMultiplier);
        finalNormalDef = (ItemEffectDef + SkillEffectDef) * (1 + ItemEffectDefMultiplier + SkillEffectDefMultiplier);
        finalStr = (stats.Str + ItemEffectStr + SkillEffectStr) * (1 + ItemEffectStrMultiplier + SkillEffectStrMultiplier);
        finalInt = (stats.Int + ItemEffectInt + SkillEffectInt) * (1 + ItemEffectIntMultiplier + SkillEffectIntMultiplier);

        finalCriticalChance = 2000 + ItemEffectCriticalChance + SkillEffectCriticalChace;
        if (finalCriticalChance >= 10000)
            finalCriticalChance = 10000;

        finalCriticalDamageMuliplie = 1.5f + ItemEffectCriticalDamageMultiple + SkillEffectCriticalDamageMultiple;

        finalLifeSteal = ItemEffectLifeSteal + SkillEffectLifeSteal;
        finalLifeStealPercent = ItemEffectLifeStealPercent + SkillEffectLifeStealPercent;

        finalHpRegen = 0.4f + finalStr * 0.1f + ItemEffectHpRegen + SkillEffectHpRegen;
        finalMpRegen = 0.4f + finalInt * 0.1f + ItemEffectMpRegen + SkillEffectMpRegen;

        if (curHp >= finalMaxHp)
            curHp = finalMaxHp;

        if (curMp >= finalMaxMp)
            curMp = finalMaxMp;
    }

    public virtual void LevelUp()
    {
        float ExpFactor = 1f;

        stats.Level++;
        stats.CurExp -= stats.MaxExp;

        if (stats.Level == 1)
            stats.MaxExp = 100f;
        else
        {
            for (int i = 1; i < stats.Level; i++)
            {
                ExpFactor *= 1.1f;
            }
            stats.MaxExp = Mathf.RoundToInt(100f * (stats.Level - 1) + (100 * ExpFactor));
        }

        stats.MaxHp = 100f + (stats.Level - 1) * 10;
        stats.MaxMp = 20f + (stats.Level - 1) * 2f;
        stats.Str = 5f + (stats.Level - 1);
        stats.Int = 5f + (stats.Level - 1);

        stats.Skill_Point++;

        RefeshFinalStats();

        curHp = finalMaxHp;
        curMp = finalMaxMp;

        levelUpEffect.SetActive(true);
    }

    /// <summary>
    /// �÷��̾ �ش� ��ų�� �������ִ��� �˻�����, �ش罺ų�� ������ _i(�⺻�� 1)�ø���.
    /// </summary>
    /// <param name="_skill"></param>
    public void SetSkillLv(Skill _skill, int _i = 1)
    {
        if (player_Skill_Dic.ContainsKey(_skill.UIDCODE))
        {
            if (_i > 0)
            {
                if (stats.Skill_Point < _i)
                {
                    Debug.Log("��ų����Ʈ�� ���ڶ��ϴ�.");
                    return;
                }
            }

            player_Skill_Dic[_skill.UIDCODE] += _i;
            stats.Skill_Point -= _i;

            //Debug.Log(_skill.Name + " ��ų�� ��ų������ " + player_Skill_Dic[_skill.UIDCODE] + "�� �Ǿ����ϴ�.");

            if (_skill.Type == 0)//�нú꽺ų�̸�
            {
                if (_i == 1)
                    skillDB.UseSkill(_skill, gameObject);
                else
                    skillDB.PassiveSkillLvDown(_skill, gameObject);
            }
        }
        else
        {
            Debug.Log("���� ��ų�� ���� �����Ϸ��� �ϰ� �ֽ��ϴ�.");
        }
    }

    public override void Hit(float _damage)
    {
        curHp -= _damage - finalNormalDef;

        if (curHp <= 0)
        {
            state = STATE.Die;
            Die();
        }
    }

    public override void Die()
    {
        state = STATE.Die;

        print("���");

        StartCoroutine(DieCo());
    }

    public void GetExp(float _exp)
    {
        stats.CurExp += _exp;

        if (stats.CurExp >= stats.MaxExp)
            LevelUp();

        SystemText_ScrollView_Ctrl.Instance.PrintText("����ġ�� " + _exp + " ȹ���߽��ϴ�.");
    }

    public void GetGold(int _gold)
    {
        stats.Gold += _gold;

        SystemText_ScrollView_Ctrl.Instance.PrintText("��带 " + _gold + " ȹ���߽��ϴ�.");
    }

    IEnumerator DieCo()
    {
        yield return new WaitForSeconds(3f);

        GameManager.Instance.isPause = true;
    }

    public virtual void SavePlayerInfo()
    {
        stats.Pos_x = transform.position.x;
        stats.Pos_y = transform.position.y;
        stats.Pos_z = transform.position.z;
        stats.Rot_y = transform.eulerAngles.y;

        string Jdata = JsonConvert.SerializeObject(stats, Formatting.Indented);
        File.WriteAllText(Application.dataPath + PlayerDataPath, Jdata);

        string Jdata2 = JsonConvert.SerializeObject(player_Skill_Dic, Formatting.Indented);
        File.WriteAllText(Application.dataPath + PlayerSkillDataPath, Jdata2);

        Debug.Log("�÷��̾���� ���̺� �Ϸ�");
    }

    public virtual void LoadPlayerInfo()
    {
        if (File.Exists(Application.dataPath + PlayerDataPath))
        {
            string Jdata = File.ReadAllText(Application.dataPath + PlayerDataPath);
            stats = JsonConvert.DeserializeObject<Stats>(Jdata);

            cController.enabled = false;
            transform.position = new Vector3(stats.Pos_x, stats.Pos_y, stats.Pos_z);
            transform.eulerAngles = new Vector3(0, stats.Rot_y, 0);
            cameraArm.transform.eulerAngles = new Vector3(0, stats.Rot_y, 0);
            cController.enabled = true;

            Debug.Log("�÷��̾���� �ε强��.");
        }
        else
        {
            Debug.LogWarning("�÷��̾���������� �����ϴ�.\n�÷��̾����� ���� �ʱ�ȭ �����մϴ�.");

            #region �÷��̾� ���� ���� �ʱ�ȭ
            stats.Level = 14;
            stats.CurExp = 1610;
            float ExpFactor = 1f;
            if (stats.Level == 1)
                stats.MaxExp = 100f;
            else
            {
                for (int i = 1; i < stats.Level; i++)
                {
                    ExpFactor *= 1.1f;
                }
                stats.MaxExp = Mathf.RoundToInt(100f * (stats.Level - 1) + (100 * ExpFactor));
            }
            stats.MaxHp = 100f + (stats.Level - 1) * 20;
            stats.MaxMp = 30f + (stats.Level - 1) * 5;
            stats.Str = 5f + (stats.Level - 1);
            stats.Int = 5f + (stats.Level - 1);

            stats.Skill_Point = stats.Level - 1;

            //GetItem(ItemDatabase.instance.newItem("0000003"));
            inven.GetItem(ItemDatabase.instance.newItem("0000000"));
            inven.GetItem(ItemDatabase.instance.newItem("0000004"));
            //inven.GetItem(ItemDatabase.instance.newItem("0000007"));
            inven.GetItem(ItemDatabase.instance.newItem("0000008"), 10);

            for (int i = 0; i < 3; i++)
                inven.inventory_Slots[i].item.SlotIndex = i;
            //GetItem(ItemDatabase.instance.newItem("0000009"), 10);

            stats.Gold = 100;

            #endregion
        }

        if (File.Exists(Application.dataPath + PlayerSkillDataPath))
        {
            string Jdata = File.ReadAllText(Application.dataPath + PlayerSkillDataPath);
            player_Skill_Dic = JsonConvert.DeserializeObject<Dictionary<string, int>>(Jdata);

            Debug.Log("�÷��̾� ��ų������ �ε� ����");
        }
        else
        {
            Debug.Log("�÷��̾� ��ų ������������ �����ϴ�.");
        }
    }

    #region ����׽� ����ϴ� �Լ��� ����
    public void DebugSkillLvUp()
    {
        string Skill_UID = "0300001";

        if (player_Skill_Dic.ContainsKey(Skill_UID))
        {
            player_Skill_Dic[Skill_UID]++;
            Debug.Log(skillDB.AllSkillDic[Skill_UID].Name + " ��ų�� ��ų������ " + player_Skill_Dic[Skill_UID] + "�� �Ǿ����ϴ�.");

            if (skillDB.AllSkillDic[Skill_UID].Type == 0)//�нú꽺ų�̸�
            {
                skillDB.UseSkill(skillDB.AllSkillDic[Skill_UID], gameObject);
                Debug.Log("�нú� ��ų ����");
            }
        }
    }

    [ContextMenu("hp����")]
    void ResetHp()
    {
        curHp = finalMaxHp;
    }
    #endregion
}
