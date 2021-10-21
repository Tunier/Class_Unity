using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject Enemy;
    
    public float curHp = 100;

    float x => Input.GetAxis("Horizontal");
    float z => Input.GetAxis("Vertical");

    float dist => Vector3.Distance(transform.position, Enemy.transform.position);

    bool pushAttackKey => Input.GetKeyDown(KeyCode.Z) || Input.GetKey(KeyCode.Z);

    float attackDelay = 1f;
    float attackAfterTime = 0;
    bool canAttack => attackAfterTime >= attackDelay;

    void FixedUpdate()
    {
        transform.Translate(new Vector3(x, 0, z).normalized * 5 * Time.fixedDeltaTime);
    }

    void Update()
    {
        if (attackAfterTime < attackDelay)
            attackAfterTime += Time.deltaTime;

        if (canAttack && pushAttackKey && dist <= 1.5f)
        {
            attackAfterTime = 0;
            var enemy = Enemy.GetComponent<Enemy>();
            enemy.curHp -= 10;
        }
    }
}
