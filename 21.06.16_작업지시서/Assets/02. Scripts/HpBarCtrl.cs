using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarCtrl : MonoBehaviour
{
    PlayerCtrl player;
    GameObject target;
    
    Canvas hpCanvas;
    Camera hpCamera;
    RectTransform rectTrCanvas;
    RectTransform rectHpTr;

    public Slider hpBar;

    Vector3 screenPos;
    Vector3 offset;

    void Start()
    {
        hpCanvas = GameObject.Find("HpCanvas").GetComponent<Canvas>();
        hpCamera = hpCanvas.worldCamera;
        rectTrCanvas = hpCanvas.GetComponent<RectTransform>();
        rectHpTr = GetComponent<RectTransform>();

        player = GameObject.FindWithTag("PLAYER").GetComponent<PlayerCtrl>();

        offset = new Vector3(0, 1f, 0);
    }

    void Update()
    {
        UpdateHpSlider();
    }

    private void LateUpdate()
    {
        var screenPos = Camera.main.WorldToScreenPoint(GameObject.Find("Golem").GetComponent<Transform>().position + offset);

        if (screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }

        var localpos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTrCanvas, screenPos, hpCamera, out localpos);

        rectHpTr.localPosition = localpos;
    }

    void UpdateHpSlider()
    {
        hpBar.value = Mathf.Lerp(hpBar.value, player.hp / player.hpMax, Time.deltaTime * 10);
    }
}
