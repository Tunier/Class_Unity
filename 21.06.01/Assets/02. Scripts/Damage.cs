using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    const string bulletTag = "BULLET";
    float iniHp = 100f;
    public float currHp;

    // ��������Ʈ ����.
    public delegate void PlayerDieHandler();
    // ��������Ʈ�� Ȱ���� �̺�Ʈ ����.
    public static PlayerDieHandler OnPlayerDieEvent;
    [SerializeField]
    Image bloodScreen;
    [SerializeField]
    Image hpBar;
    readonly Color initColor = new Vector4(0, 1f, 0f, 1f);
    Color currColor;

    bool isOnBloodScreen = false;

    void Start()
    {
        currHp = iniHp;
        hpBar.color = initColor;
        currColor = initColor;
    }

    // �浹�� �ƴ϶� ������ ��쿡 ����ϴ� �Լ�.
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == bulletTag)
        {
            Destroy(other.gameObject);
            currHp -= 5; // hp 5 ����
            //print("���� ü�� : " + currHp);
            // Debug.Log(currHp); <-- ������ ���.
            //if (!isOnBloodScreen)
                //StartCoroutine(BloodScreen());

            if (currHp <= 0f)
            {
                // �÷��̾� ��� �Լ� ȣ��
                PlayerDie();
            }
        }
    }

    void PlayerDie()
    {
        OnPlayerDieEvent();
        //print("�÷��̾� ���");
        //GameObject[] enemies = GameObject.FindGameObjectsWithTag("ENEMY");

        //for (int i = 0; i < enemies.Length; i++)
        //{
        //    // �Լ� ȣ���ϴ� �����
        //    // ���� ȣ��
        //    // enemies[i].GetComponent<EnemyAI>().OnPlayerDie();
        //    // SendMessage�� ȣ��
        //    enemies[i].SendMessage("OnPlayerDie",SendMessageOptions.DontRequireReceiver); 
        //    // ��������Ʈ�� ȣ��
        //}
    }

    void Update()
    {
        DisplayHpbar();
    }

    IEnumerator BloodScreen()
    {
        isOnBloodScreen = true;
        for (int i = 1; i < 3; i++)
        {
            Color color = bloodScreen.color;
            color.a = i % 2;
            bloodScreen.color = color;
            yield return new WaitForSeconds(0.25f);
        }
        isOnBloodScreen = false;
    }

    void DisplayHpbar()
    {
        if ((currHp / iniHp) > 0.5f)
            currColor.r = (1 - (currHp / iniHp)) * 2.0f;
        else
            currColor.g = (currHp / iniHp) * 2f;

        hpBar.color = currColor;

        hpBar.fillAmount = currHp / iniHp;
    }
}
