using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    const string bulletTag = "BULLET";
    float iniHp = 100f;
    public float currHp;
    
    // 델리게이트 선언.
    public delegate void PlayerDieHandler();
    // 델리게이트를 활용한 이벤트 선언.
    public static PlayerDieHandler OnPlayerDieEvent;
    void Start()
    {
        currHp = iniHp;
    }
    
    // 충돌이 아니라 관통일 경우에 사용하는 함수.
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == bulletTag)
        {
            Destroy(other.gameObject);
            currHp -= 5; // hp 5 감소
            print("현재 체력 : " + currHp);
            // Debug.Log(currHp); <-- 동일한 방법.
            if (currHp <= 0f)
            {
                // 플레이어 사망 함수 호출
                PlayerDie();
            }
        }
    }    

    void PlayerDie()
    {
        OnPlayerDieEvent();
        //print("플레이어 사망");
        //GameObject[] enemies = GameObject.FindGameObjectsWithTag("ENEMY");

        //for (int i = 0; i < enemies.Length; i++)
        //{
        //    // 함수 호출하는 방법들
        //    // 직접 호출
        //    // enemies[i].GetComponent<EnemyAI>().OnPlayerDie();
        //    // SendMessage로 호출
        //    enemies[i].SendMessage("OnPlayerDie",SendMessageOptions.DontRequireReceiver); 
        //    // 델리게이트로 호출
        //}
    }

    void Update()
    {
        
    }


}
