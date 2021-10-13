using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

[System.Serializable]
public class Skill
{
    public string UIDCODE;
    public string Name;
    public int Type;
    public int CostType;
    public int Cost;
    public int Value;
    public float ValueFactor;
    public float CoolTime;
    public int MaxSkillLv;
    public int NeedLv;

    public int slotIndex;

    [TextArea]
    public string SkillDescription;
    public string SkillImagePath;
}

public class SkillDatabase : MonoBehaviour
{
    public static SkillDatabase instance;

    public List<Skill> AllSkillList = new List<Skill>();
    public Dictionary<string, Skill> AllSkillDic = new Dictionary<string, Skill>();

    Player_SkillIndicator player_SkillIndicator;
    PlayerInfo player;
    PlayerActionCtrl playerAC;

    GameObject skill2HitArea;
    GameObject cameraArm;

    const string skillDataPath = "/Resources/Data/All_Skill_Data.text";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
                Destroy(gameObject);
        }

        player_SkillIndicator = FindObjectOfType<Player_SkillIndicator>();
        cameraArm = GameObject.Find("CameraArm");

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
            AllSkillDic.Add(AllSkillList[i].UIDCODE, AllSkillList[i]);
        }

        player = FindObjectOfType<PlayerInfo>();
        playerAC = FindObjectOfType<PlayerActionCtrl>();
        skill2HitArea = GameObject.Find("Skill2HitBox");

        player.player_Skill_Dic.Add(AllSkillDic["0300000"].UIDCODE, 1); // 임시로 플레이어의 스킬리스트에 스킬을 넣어줌.
        player.player_Skill_Dic.Add(AllSkillDic["0300001"].UIDCODE, 0);
        player.player_Skill_Dic.Add(AllSkillDic["0300002"].UIDCODE, 0);
        //player.player_Skill_Dic.Add(AllSkillDic["0300003"].UIDCODE, 0);
        //player.player_Skill_Dic.Add(AllSkillDic["0300004"].UIDCODE, 0);
        player.player_Skill_Dic.Add(AllSkillDic["0300005"].UIDCODE, 0);

        skill2HitArea.SetActive(false);
    }

    public Skill NewSkill(string _UIDCODE)
    {
        //최종 데미지 공식 : Mathf.RoundToInt(AllSkillDic[i].Value + (AllSkillDic[i].Skill_Level - 1) * AllSkillDic[i].ValueFactor);

        var skill = new Skill();

        skill.UIDCODE = AllSkillDic[_UIDCODE].UIDCODE;
        skill.Type = AllSkillDic[_UIDCODE].Type;
        skill.Name = AllSkillDic[_UIDCODE].Name;
        skill.CostType = AllSkillDic[_UIDCODE].CostType;
        skill.Cost = AllSkillDic[_UIDCODE].Cost;
        skill.Value = AllSkillDic[_UIDCODE].Value;
        skill.ValueFactor = AllSkillDic[_UIDCODE].ValueFactor;
        skill.SkillDescription = AllSkillDic[_UIDCODE].SkillDescription;
        skill.SkillImagePath = AllSkillDic[_UIDCODE].SkillImagePath;

        return skill;
    }

    /// <summary>
    /// 스킬, 스킬사용자, 스킬타겟(기본은 null)를 받아서 스킬 타입에따라 스킬이 사용되게함.
    /// </summary>
    /// <param name="_skill"></param>
    /// <param name="_user"></param>
    /// <param name="_target"></param>
    public void UseSkill(Skill _skill, GameObject _user, GameObject _target = null, SkillSlot _skillslot = null)
    {
        // 스킬타입
        // 0: 패시브, 1: 논타겟팅, 2: 타겟팅, 3: 버프, 4: 즉시시전

        PlayerInfo player;

        if (_user.CompareTag("Player"))
        {
            player = _user.GetComponent<PlayerInfo>();

            if (player.player_Skill_Dic[_skill.UIDCODE] == 0)
            {
                Debug.Log("아직 배우지 않은 스킬입니다.");
                return;
            }

            switch (_skill.Type)
            {
                case 0: // 패시브
                    switch (_skill.UIDCODE)
                    {
                        case "0300001": //"Hp증가"
                            if (player.player_Skill_Dic[_skill.UIDCODE] > 1)
                            {
                                player.SkillEffectMaxHp += _skill.ValueFactor;
                                player.curHp += _skill.ValueFactor;
                                player.RefeshFinalStats();
                                //Debug.Log(_skill.Name + " (패시브)스킬 레벨업");
                            }
                            else if (player.player_Skill_Dic[_skill.UIDCODE] == 1)
                            {
                                player.SkillEffectMaxHp += _skill.Value;
                                player.curHp += _skill.Value;
                                player.RefeshFinalStats();
                                //Debug.Log(_skill.Name + " (패시브)스킬 습득");
                            }
                            break;
                    }
                    break;
                case 1: // 논타겟팅
                    switch (_skill.UIDCODE)
                    {
                        case "0300000":
                            player.curMp -= _skill.Cost;
                            playerAC.curSkillCooltime[_skillslot.skill.UIDCODE] += _skill.CoolTime;
                            Vector3 skillPos = player.transform.position + player.transform.forward * 2 + new Vector3(0, 1.9f, 0);
                            var obj = Instantiate(Resources.Load<GameObject>("Skill/Prefebs/FireBall"), skillPos, Quaternion.identity);
                            // 나중에 오브젝트 풀링해서 미리 생성해놓은 오브젝트 활성화해서 사용하게 변경해야함
                            obj.transform.forward = player.transform.forward;
                            obj.GetComponent<FireBall>().player = player;
                            break;
                    }
                    break;
                case 2: // 타겟팅
                        //switch (_skill.UIDCODE)
                        //{

                    //}
                    break;
                case 3: // 버프
                    //switch (_skill.UIDCODE)
                    //{

                    //}
                    break;
                case 4:
                    switch (_skill.UIDCODE)
                    {
                        case "0300002":
                            player.curMp -= _skill.Cost;
                            Vector3 camArmRot = new Vector3(0, cameraArm.transform.eulerAngles.y, 0);
                            player.transform.rotation = Quaternion.Euler(camArmRot);
                            playerAC.curSkillCooltime[_skillslot.skill.UIDCODE] += _skill.CoolTime;
                            playerAC.isSwordSkill2 = true;
                            StartCoroutine(UseSkill2());
                            break;
                        case "0300005":
                            player.curMp -= _skill.Cost;
                            playerAC.curSkillCooltime[_skillslot.skill.UIDCODE] += _skill.CoolTime;
                            playerAC.isWhirlwind = true;
                            StartCoroutine(WhirlWindEffectInst());
                            break;
                    }

                    break;
                default:
                    Debug.LogError("스킬타입에 없는 스킬입니다.");
                    break;
            }
        }


    }

    public void UsePassiveSkillOnLoad(Skill _skill, int _skillLv, GameObject _user)
    {
        if (_user.CompareTag("Player"))
        {
            var player = _user.GetComponent<PlayerInfo>();
            if (_skill.Type == 0)
            {
                switch (_skill.UIDCODE)
                {
                    case "0300001"://"Hp증가"
                        if (_skillLv > 0)
                        {
                            player.SkillEffectMaxHp += _skill.Value + (_skillLv - 1) * _skill.ValueFactor;
                            player.RefeshFinalStats();
                            player.curHp = player.finalMaxHp;
                            //Debug.Log(_skill.Name + " (패시브)스킬 효과 발동");
                        }
                        else
                        {
                            //Debug.Log("아직 배우지 않은 스킬이라 효과발동안됨.");
                        }
                        break;
                }
            }
        }
        //else if()
        //{
        //  나중에 플레이어 이외 대상이 패시브 스킬을 사용할떄 상황.
        //}
    }

    public void PassiveSkillLvDown(Skill _skill, GameObject _user)
    {
        if (_user.CompareTag("Player"))
        {
            var player = _user.GetComponent<PlayerInfo>();
            if (_skill.Type == 0)
            {
                switch (_skill.UIDCODE)
                {
                    case "0300001"://"Hp증가"
                        if (player.player_Skill_Dic[_skill.UIDCODE] > 0)
                        {
                            player.SkillEffectMaxHp -= _skill.ValueFactor;
                            player.curHp -= _skill.ValueFactor;
                            player.RefeshFinalStats();
                        }
                        else if (player.player_Skill_Dic[_skill.UIDCODE] <= 0)
                        {
                            player.SkillEffectMaxHp -= _skill.Value;
                            player.curHp -= _skill.Value;
                            player.RefeshFinalStats();
                        }
                        break;
                }
            }
        }
    }

    IEnumerator WhirlWindEffectInst()
    {
        yield return new WaitForSeconds(0.3f);

        var obj = Instantiate(Resources.Load<GameObject>("Skill/Prefebs/WhirlWind_Skill_Effect"), player.transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
    }

    IEnumerator UseSkill2()
    {
        var skill2 = skill2HitArea.GetComponent<Skill2>();

        yield return new WaitForSeconds(0.45f);

        skill2HitArea.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        skill2HitArea.SetActive(false);
        skill2.mobList.Clear();
    }
}
