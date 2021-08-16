using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMovement : MonoBehaviour
{
    void Update()
    {
        if (transform.position.y <= 10 && transform.position.y >= -1)
            transform.Translate(Vector3.down * 2f * Time.deltaTime, Space.World);

        if (transform.position.y <= -1)
            gameObject.SetActive(false);
    }
}
