using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target; // 카메라가 추적할 대상
    public float moveDamping = 15f; // 이동속도 계수
    public float rotateDamping = 10f; // 회전속도 계수
    public float distance = 5f; // 추적 대상과의 거리
    public float height = 4f; // 추적 대상과의 높이
    public float targetOffset = 2f; // 추적 좌표의 오프셋

    Transform tr;
    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
    }

    // 콜백함수 - 호출을 따로 하지 않아도 알아서 작동하는 함수.
    // 이벤트 트리거등 여러가지 사용법이 있음.
    void LateUpdate()
    {
        var camPos = target.position - (target.forward * distance) + (target.up * height);

        tr.position = Vector3.Slerp(tr.position, camPos, Time.deltaTime * moveDamping);
        // 보간함수검색. (점과 점의 연걸을 부드럽게 깎아줌)

        tr.rotation = Quaternion.Slerp(tr.rotation, target.rotation, Time.deltaTime * rotateDamping);
        // 유니티에서 사용하는 각도, 축들의 교착상태를 푸는데 사용

        tr.LookAt(target.position + (target.up * targetOffset));
        // 카메라를 꺾어서 타겟을 보게함.(캐릭터의 발을 향해 보고있는것을 정수리를 향하게 꺾어줌)
    }
}
