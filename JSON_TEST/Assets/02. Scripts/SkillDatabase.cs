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
    public int Skill_Level = 1;

    [TextArea]
    public string SkillDescription;
    public string SkillImagePath;
}

public class SkillDatabase : MonoBehaviour
{
    SkillDatabase instance;

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

    void Start()
    {

    }

    void Update()
    {

    }

    public Skill newSkill(int i)
    {
        //int _value;

        //_value = Mathf.RoundToInt(AllSkillDic[i].Value + (AllSkillDic[i].Skill_Level - 1) * AllSkillDic[i].ValueFactor);

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
}
