using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextUI : MonoBehaviour
{
    PlayerInfo player;

    public GameObject go_DamageText;
    public Text damageText;
    Color textColor = Color.black;

    public GameObject mob;
    float moveSpeed;
    float alphaSpeed;
    float destroyTime;
    Vector3 offset;
    float v;

    private void Awake()
    {
        player = FindObjectOfType<PlayerInfo>();

        moveSpeed = 45f;
        alphaSpeed = 1.2f;
        destroyTime = 2f;
        offset = new Vector3(0, 90f, 0);
        v = 0f;
    }

    void Start()
    {
        //if (player.targetMonster != null)
        //    mob = player.targetMonster.gameObject;

        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        if (mob != null)
        {
            Vector3 mobPos = Camera.main.WorldToScreenPoint(mob.transform.position);

            Vector3 printPos = mobPos;

            v += moveSpeed * Time.deltaTime;

            printPos = printPos + new Vector3(0, v, 0) + offset;

            transform.position = printPos;

            textColor.a = Mathf.Lerp(textColor.a, 0, Time.deltaTime * alphaSpeed);
            damageText.color = textColor;
        }
    }

    /// <summary>
    /// 플롯값을 받아서 반올림해서 텍스트를 변경해줌.
    /// </summary>
    /// <param name="_damage"></param>
    public void SetDamageText(float _damage)
    {
        damageText.text = Mathf.RoundToInt(_damage) + "";
    }

    /// <summary>
    /// 색을 받아서 텍스트 색을 변경해줌.
    /// </summary>
    /// <param name="_color"></param>
    public void SetTextColor(Color _color)
    {
        textColor = _color;
    }

    public void SetTextSize(int _size)
    {
        damageText.fontSize = _size;
    }
}
