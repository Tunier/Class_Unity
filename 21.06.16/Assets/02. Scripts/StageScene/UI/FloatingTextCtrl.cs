using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextCtrl : MonoBehaviour
{
    public GameObject mob = null;

    PlayerCtrl player;

    float moveSpeed;
    float alphaSpeed;
    float destroyTime;
    Vector3 offset;
    float v;

    Text floatingText;
    Color alpha = Color.black;

    public float damage;

    private void Awake()
    {
        floatingText = GetComponent<Text>();
        player = FindObjectOfType<PlayerCtrl>();

        moveSpeed = 45f;
        alphaSpeed = 1.2f;
        destroyTime = 2f;
        offset = new Vector3(0, 60f, 0);
        v = 0f;
    }

    void Start()
    {
        if (player.hitmob != null)
            mob = player.hitmob.gameObject;

        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        Vector3 mobPos = Camera.main.WorldToScreenPoint(mob.transform.position);

        Vector3 printPos = mobPos;

        v += moveSpeed * Time.deltaTime;

        printPos = printPos + new Vector3(0, v, 0) + offset;

        transform.position = printPos;

        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        floatingText.color = alpha;
    }

    /// <summary>
    /// 플롯값을 받아서 반올림해서 텍스트를 변경해줌.
    /// </summary>
    /// <param name="_damage"></param>
    public void SetDamageText(float _damage)
    {
        floatingText.text = Mathf.RoundToInt(_damage) + "";
    }

    /// <summary>
    /// 색을 받아서 텍스트 색을 변경해줌.
    /// </summary>
    /// <param name="_color"></param>
    public void SetTextColor(Color _color)
    {
        alpha = _color;
    }
}
