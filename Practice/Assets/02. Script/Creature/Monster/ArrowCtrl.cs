using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCtrl : MonoBehaviour
{
    public MonsterHunter hunter;
    GameObject player;

    float delayTime = 0;
    Rigidbody rb;

    Vector3 shotRot;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        delayTime = 0;
        shotRot = (new Vector3(player.transform.position.x, 0, player.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z)).normalized;
    }

    void Update()
    {
        BackPooling();
    }

    private void FixedUpdate()
    {
        rb.AddForce(shotRot * 1f, ForceMode.Impulse);
    }

    public void BackPooling()
    {
        delayTime += Time.deltaTime;
        if (delayTime >= 2f)
        {
            delayTime -= delayTime;
            rb.velocity = Vector3.zero;
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerCreature = player.GetComponent<Creature>();
            playerCreature.Hit(hunter.finalNormalAtk);

            rb.velocity = Vector3.zero;
            this.gameObject.SetActive(false);
        }
    }
}
