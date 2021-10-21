using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float x => Input.GetAxisRaw("Horizontal");
    public float z => Input.GetAxisRaw("Vertical");

    float speed = 5f;

    public int score = 0;

    public bool canEat = false;
    public bool isDie = false;

    Coroutine spiCo;

    void FixedUpdate()
    {
        transform.Translate(new Vector3(x, 0, z).normalized * Time.fixedDeltaTime * speed);
    }

    public void UseSpecial_Item()
    {
        if (spiCo != null) { StopCoroutine(spiCo); }
        spiCo = StartCoroutine(Special_Item_Co());
    }

    IEnumerator Special_Item_Co()
    {
        canEat = true;

        Stage_Manager.Instance.ChageAllEnemyState(Enemy.eState.Run);

        yield return new WaitForSeconds(8f);

        canEat = false;

        Stage_Manager.Instance.ChageAllEnemyState(Enemy.eState.Attack);
    }
}
