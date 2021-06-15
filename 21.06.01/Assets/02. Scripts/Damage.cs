using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    const string bulletTag = "BULLET";
    float iniHp = 100f;
    public float currHp;
    
    // ��������Ʈ ����.
    public delegate void PlayerDieHandler();
    // ��������Ʈ�� Ȱ���� �̺�Ʈ ����.
    public static PlayerDieHandler OnPlayerDieEvent;
    void Start()
    {
        currHp = iniHp;
    }
    
    // �浹�� �ƴ϶� ������ ��쿡 ����ϴ� �Լ�.
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == bulletTag)
        {
            Destroy(other.gameObject);
            currHp -= 5; // hp 5 ����
            print("���� ü�� : " + currHp);
            // Debug.Log(currHp); <-- ������ ���.
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
        
    }


}
