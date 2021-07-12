using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextCtrl : MonoBehaviour
{
    public PlayerWeaponCtrl Pwp;
    MonsterCtrl mob;

    float moveSpeed;
    float alphaSpeed;
    float destroyTime;
    Vector3 offset;
    float v;


    Text floatingText;
    Color alpha;
    public float damage;

    private void Awake()
    {
        Pwp = GameObject.Find("PlayerSword").GetComponent<PlayerWeaponCtrl>();
    }

    void Start()
    {
        moveSpeed = 45f;
        alphaSpeed = 1.2f;
        destroyTime = 2f;
        offset = new Vector3(0, 100f, 0);
        v = 0f;
       

        floatingText = GetComponent<Text>();
        alpha = floatingText.color;
        floatingText.text = damage.ToString();

        if (Pwp.hitmob != null)
        {
            mob = Pwp.hitmob;
        }

        Invoke("DestroyObject", destroyTime);
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

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
