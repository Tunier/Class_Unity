using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Editor 클래스는 인스펙터나 윈도우 화면 등을
// 자유롭게 구성하거나 확장할 수 있록 하기 위한 클래스
// 쉽게 말해 사용자가 커스텀한 제작툴 제작에 사용됨

// EnemyFOV 스크립트를 보조하는 커스텀 에디터다 라고 명시
[CustomEditor(typeof(EnemyFOV))]
public class FOVEditor : Editor
{
    private void OnSceneGUI()
    {
        // 에디터가 보조할 대상을 지정
        // EnemyFov 클래스 참조
        EnemyFOV fov = (EnemyFOV)target;

        // 원주 위에서 시작점의 좌표를 계산(시야각의 1/2)
        Vector3 fromAnglePos = fov.CirclePoint(-fov.viewAngle * 0.5f);

        // 원주의 색상을 흰색으로 지정
        Handles.color = Color.white;

        // 외곽선만 있는 원을 그림
        Handles.DrawWireDisc(fov.transform.position, Vector3.up, fov.viewRange); // 원점좌표, 노멀벡터, 원의 반지름

        // 부채꼴(시야각을 표현)
        Handles.color = new Color(1, 0, 0, 0.2f);
        // 원점 좌표, 노멀벡터, 부채꼴의 시작 각도, 그리는 각도, 부채꼴의 반지름
        Handles.DrawSolidArc(fov.transform.position, Vector3.up, fromAnglePos, fov.viewAngle, fov.viewRange);
        // 시야각 라벨링
        Handles.Label(fov.transform.position + fov.transform.forward * 2f, fov.viewAngle.ToString());
    }
}
