using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;

[System.Serializable]
public class Skill
{
    public enum SkillType
    {
        Passive,
        AoEAttack,
        TargetingAttack,
        Buff,
        Debuff,
        Heal,
    }

    public enum CostType
    {
        None,
        Hp,
        Mp,
    }

    public Skill(int _Index, SkillType _skillType, string _Name, CostType _costType, int _Cost, int _Value, float _ValueFactor, string _SkillDescription, string _SkillImagePath)
    {
        Index = _Index;
        skillType = _skillType;
        Name = _Name;
        costType = _costType;
        Cost = _Cost;
        Value = _Value;
        ValueFactor = _ValueFactor;
        SkillDescription = _SkillDescription;
        SkillImagePath = _SkillImagePath;
    }

    public int Index;
    [JsonConverter(typeof(StringEnumConverter))]
    public SkillType skillType;
    public string Name;
    [JsonConverter(typeof(StringEnumConverter))]
    public CostType costType;
    public int Cost;
    public int Value;
    public float ValueFactor;
    public int SkillLv;
    public int MaxSkillLv;

    [TextArea]
    public string SkillDescription;
    public string SkillImagePath;
}

public class SkillDatabase : MonoBehaviour
{
    public static SkillDatabase instance;

    public List<Skill> AllSkillList = new List<Skill>();
    public Dictionary<int, Skill> AllSkillDic = new Dictionary<int, Skill>();

    const string skillDataPath = "/Resources/Data/All_Skill_Data.text";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
                Destroy(gameObject);
        }

        if (File.Exists(Application.dataPath + skillDataPath))
        {
            string Jdata = File.ReadAllText(Application.dataPath + skillDataPath);
            AllSkillList = JsonConvert.DeserializeObject<List<Skill>>(Jdata);
            Debug.Log("스킬 데이터 로드성공.");
        }
        else
            Debug.LogWarning("스킬 데이터 파일이 없습니다.");

        for (int i = 0; i < AllSkillList.Count; i++)
        {
            AllSkillDic.Add(AllSkillList[i].Index, AllSkillList[i]);
        }
    }

    public Skill newSkill(int i)
    {
        //벨류 공식 : Mathf.RoundToInt(AllSkillDic[i].Value + (AllSkillDic[i].Skill_Level - 1) * AllSkillDic[i].ValueFactor);

        var skill = new Skill(AllSkillDic[i].Index,
                              AllSkillDic[i].skillType,
                              AllSkillDic[i].Name,
                              AllSkillDic[i].costType,
                              AllSkillDic[i].Cost,
                              AllSkillDic[i].Value,
                              AllSkillDic[i].ValueFactor,
                              AllSkillDic[i].SkillDescription,
                              AllSkillDic[i].SkillImagePath);
        return skill;
    }

    /// <summary>
    /// 스킬, 스킬타겟, 스킬사용자(기본은 null)를 받아서 스킬 타입에따라 스킬이 사용되게함.
    /// </summary>
    /// <param name="_skill"></param>
    /// <param name="_user"></param>
    /// <param name="_target"></param>
    public void UseSkill(Skill _skill, GameObject _user, GameObject _target = null)
    {
        if (_skill.SkillLv == 0)
        {
            Debug.Log("아직 배우지 않은 스킬입니다.");
            return;
        }

        switch (_skill.skillType)
        {
            case Skill.SkillType.Passive:
                if (_user.CompareTag("Player"))
                {
                    var player = _user.GetComponent<PlayerTest>();
                    switch (_skill.Index)
                    {
                        case 1://"Hp증가"
                            if (_skill.SkillLv > 1)
                            {
                                player.SkillEffectMaxHp += _skill.ValueFactor;
                                player.RefeshFinalStats();
                                Debug.Log(_skill.Name + " (패시브)스킬 레벨업");
                            }
                            else if (_skill.SkillLv == 1)
                            {
                                player.SkillEffectMaxHp += _skill.Value;
                                player.RefeshFinalStats();
                                Debug.Log(_skill.Name + " (패시브)스킬 습득");
                            }
                            break;
                    }
                }
                // 나중에 사용자가 플레이어 이외일때 작성.
                //else if (_user.GetComponent<>)
                //{ 
                //}
                break;
            case Skill.SkillType.TargetingAttack:

                break;
            case Skill.SkillType.AoEAttack:

                break;
            case Skill.SkillType.Buff:

                break;
            case Skill.SkillType.Debuff:

                break;
            case Skill.SkillType.Heal:

                break;
            default:
                Debug.LogError("스킬타입에 없는 스킬입니다.");
                break;
        }
    }

    public void UsePassiveSkillOnLoad(Skill _skill, GameObject _user)
    {
        if (_user.CompareTag("Player"))
        {
            var player = _user.GetComponent<PlayerTest>();
            switch (_skill.Index)
            {
                case 1://"Hp증가"
                    if (_skill.SkillLv != 0)
                    {
                        player.SkillEffectMaxHp += _skill.Value + (_skill.SkillLv - 1) * _skill.ValueFactor;
                        player.RefeshFinalStats();
                        //Debug.Log(_skill.Name + " (패시브)스킬 효과 발동");
                    }
                    else if (_skill.SkillLv == 0)
                    {
                        //Debug.Log("아직 배우지 않은 스킬이라 효과발동안됨.");
                    }
                    break;
            }
        }
        //else if()
        //{
        //  나중에 플레이어 이외 대상이 패시브 스킬을 사용할떄 상황.
        //}
    }
}
