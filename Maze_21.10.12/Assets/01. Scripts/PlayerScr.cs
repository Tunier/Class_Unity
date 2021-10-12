using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScr : MonoBehaviour
{
    CharacterController cC;

    float x => Input.GetAxisRaw("Horizontal");
    float z => Input.GetAxisRaw("Vertical");
    float moveSpeed = 5f;

    private void Awake()
    {
        cC = GetComponent<CharacterController>();
    }

    void Update()
    {
        cC.SimpleMove(new Vector3(x, 0, z).normalized * moveSpeed);

        if (z <= -1)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else if (z >= 1)
            transform.eulerAngles = Vector3.zero;
        else if (x <= -1)
            transform.eulerAngles = new Vector3(0, 270, 0);
        else if (x >= 1)
            transform.eulerAngles = new Vector3(0, 90, 0);
    }
}
