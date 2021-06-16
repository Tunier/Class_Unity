using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetic : MonoBehaviour
{
    public GameObject player;

    void Update()
    {
        player = GameObject.FindGameObjectWithTag("PLAYER");

        float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);

        if (distance <= 3)
            transform.position = Vector3.MoveTowards(gameObject.transform.position, player.transform.position, 3f * Time.deltaTime);
    }
}
