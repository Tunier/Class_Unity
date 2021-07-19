using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    Camera uiCam;
    Canvas canvas;
    RectTransform rectParent;
    RectTransform rectHp;

    public Vector3 offset = Vector3.zero;
    public Transform targetTr;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        uiCam = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = gameObject.GetComponent<RectTransform>();
    }

    void Update()
    {
        var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset);

        // 카메라의 뒷쪽 영역에 위치할 때 좌표값 보정
        if (screenPos.z < 0f)
        {
            screenPos *= -1f;
        }
        var localPos = Vector2.zero;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCam, out localPos);
        
        rectHp.localPosition = localPos;
    }
}
