using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    // 셰이크 효과를 줘서 흔들리게될 카메라의 트랜스폼.
    public Transform shakeCamera;
    public bool shakeRotate = false;
    Vector3 originPos; // 원래 위치.
    Quaternion originRot; // 원래 회전값.

    void Start()
    {
        originPos = shakeCamera.localPosition;
        originRot = shakeCamera.localRotation;
    }

    public IEnumerator ShakeCamera(float duration = 0.05f, float mPos = 0.03f, float mRot = 0.1f)
    {
        // 경과 시간 저장용 변수.
        float passTime = 0f;
        // 설정한 진동 시간동안만 흔들리도록 설정
        while (passTime < duration)
        {
            // 반지름이 1인 가상의 구체 모야에서 좌표를 추출
            // (x, y, z)가 최소 (-1, -1, -1~) ~ (1 ,1 ,1) 사이의 값을 추출.
            // 흔들림할때 많이씀.
            Vector3 shakePos = Random.insideUnitSphere;
            // 위에서 추출한 위치 값을 토대로 카메라의 위치를 변경해줌
            shakeCamera.localPosition = shakePos * mPos;

            // 카메라를 회전 시킬경우.
            if (shakeRotate)
            {
                // PerlinNoise : 무규칙적인 노이즈를 어느정도 보정하여 일관성이 있는 가진 노이즈를 발생.
                // 따라서 규칙성이 있도록 보임. 지형이나 사물을 배치할때 많이 사용되며 예로
                // 오픈 필드에 있는 나무나 풀 등을 심을 때 많이 사용됨.                
                float noise = Mathf.PerlinNoise(Time.time * mRot, 0f);
                Vector3 shakeRot = new Vector3(0, 0, noise);
                // 위에서 구한 회전값을 카메레에 설정.
                shakeCamera.localRotation = Quaternion.Euler(shakeRot);

            }
            passTime += Time.deltaTime;
            yield return null;
        }
        // 진동 후에 카메라의 위치와 회전값을 초깃값으로 다시 설정.
        shakeCamera.localPosition = originPos;
        shakeCamera.localRotation = originRot;
    }

    void Update()
    {

    }
}
