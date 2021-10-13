using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    Renderer[] renderers;

    int iniHp = 100;
    public int currHp = 100;

    Animator anim;
    CharacterController cC;

    readonly int hashDie = Animator.StringToHash("Die");
    readonly int hashRespawn = Animator.StringToHash("Respawn");

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        anim = GetComponent<Animator>();
        cC = GetComponent<CharacterController>();
        currHp = iniHp;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (currHp > 0 && collision.collider.CompareTag("BULLET"))
        {
            currHp -= 20;
            if (currHp <= 0)
            {
                StartCoroutine(PlayerDie());
            }
        }
    }

    IEnumerator PlayerDie()
    {
        cC.enabled = false;
        anim.SetBool(hashRespawn, false);
        anim.SetTrigger(hashDie);

        yield return new WaitForSeconds(3f);

        anim.SetBool(hashRespawn, true);
        SetPlayerVisible(false);

        yield return new WaitForSeconds(1.5f);

        currHp = 100;
        SetPlayerVisible(true);
        cC.enabled = true;
    }

    void SetPlayerVisible(bool isVisible)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            // 파라메터 값으로 껐다가 켰다가 하기.
            renderers[i].enabled = isVisible;
        }
    }
}
