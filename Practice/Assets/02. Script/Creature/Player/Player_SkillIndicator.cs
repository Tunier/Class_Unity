using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_SkillIndicator : MonoBehaviour
{
    [Header("원형 범위 스킬")]
    public GameObject circleIndicator;
    public Image circleIndicatorImage;
    [Header("직선형 범위 스킬")]
    public GameObject straightIndicator;
    public Image straightIndicatorImage;

    GameObject player;

    Ray ray;

    Vector3 mousePos;

    private void Awake()
    {
        player = GameObject.Find("Player");

        circleIndicator.SetActive(false);
        straightIndicator.SetActive(false);
    }

    void Update()
    {
        transform.position = player.transform.position;

        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 6);

        var hitPosDir = (hit.point - player.transform.position).normalized;
        float distance = Vector3.Distance(hit.point, player.transform.position);
        distance = Mathf.Min(distance, 20f);

        var newHitPos = player.transform.position + hitPosDir * distance;
        circleIndicatorImage.rectTransform.position = newHitPos + new Vector3(0, 0.2f, 0);

        if (straightIndicator.activeSelf)
            SetStraightIndicatorAndPlayerRotate();
        else
            transform.eulerAngles = Vector3.zero;
    }

    void SetStraightIndicatorAndPlayerRotate()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green); // 레이시각화.

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, 1 << 6))
        {
            mousePos = new Vector3(hit.point.x, player.transform.position.y, hit.point.z) - player.transform.position;
        }

        transform.forward = mousePos.normalized;
        player.transform.forward = mousePos.normalized;
    }
}
