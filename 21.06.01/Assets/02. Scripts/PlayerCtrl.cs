using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 내부클래스
// 클래스 직렬화
// 클래스의 경우 변수와 달리 직렬화 해줘야 인스펙터에 표시됨.
[System.Serializable]
public class PlayerAnim
{
    public AnimationClip idle;
    public AnimationClip runF;
    public AnimationClip runB;
    public AnimationClip runL;
    public AnimationClip runR;

}

public class PlayerCtrl : MonoBehaviour
{
    // 접근지시자
    // Private는 작성된 스크립트 내부에서만 사용됨.
    // 유니티 기본 형태는 Private임
    // 추가로 Protected 라는 접근지시자가 있음.
    float h = 0f;
    float v = 0f;
    float r = 0f; // 마우스 값 받아서 회전 시키기 위한 변수.

    Transform tr; // 트랜스폼 컴포넌트 접근 변수
    // Public으로 선언된 변수는 Inspector 창에 노출됨.    
    public float moveSpeed = 8f;
    public float rotSpeed = 400f;

    public PlayerAnim playerAnim;
    public Animation anim;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        anim = GetComponent<Animation>();
        anim.clip = playerAnim.idle;
        anim.Play();
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal"); // 인풋.힘을받아옴(가속도를 가진)(수평)
        v = Input.GetAxis("Vertical"); // 인풋.힘을받아옴(가속도를 가진)(수직)
        r = Input.GetAxis("Mouse X"); // 인풋.힘을받아옴(마우스 좌우로 돌리면 캐릭터가 돌아감)
        // print("H값 " + h + " V값 " + v);

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        // 벡터 정규화
        // 대각선이동이 일반적인 이동보다 크기가 커서 빨리 움직임.
        // 이를 해결하고자 벡터의 크기를 일정하게 1로 정규화 시킴.
        moveDir = moveDir.normalized;

        tr.Translate(moveDir * moveSpeed * Time.deltaTime, Space.Self);
        tr.Rotate(Vector3.up * rotSpeed * Time.deltaTime * r);

        // print(Vector3.Magnitude(Vector3.forward + Vector3.right));
        // print(Vector3.Magnitude((Vector3.forward + Vector3.right).normalized));

        // 애니메이션 동작구분
        if (v >= 0.1f) // 전진
            // CrossFade(애니메이션 이름, 전환시간)
            anim.CrossFade(playerAnim.runF.name, 0.2f);
        else if (v <= -0.1f) // 후진
            anim.CrossFade(playerAnim.runB.name, 0.2f);
        else if (h >= 0.1f) // 오른쪽
            anim.CrossFade(playerAnim.runR.name, 0.2f);
        else if (h <= -0.1f) // 왼쪽
            anim.CrossFade(playerAnim.runL.name, 0.2f);
        else // 정지시 idle 상태로 전환.
            anim.CrossFade(playerAnim.idle.name, 0.1f);
    }
}
