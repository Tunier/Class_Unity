using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Transform player;

    public float offsetY;
    public float offsetZ;

    Vector3 cameraPosition;

    void Start()
    {
        player = GameObject.FindWithTag("PLAYER").GetComponent<Transform>();

        offsetY = 10f;
        offsetZ = -3.5f;
    }

    void LateUpdate()
    {
        cameraPosition.x = player.transform.position.x;
        cameraPosition.y = player.transform.position.y + offsetY;

        if (player.transform.position.z + offsetZ <= 5f && player.transform.position.z + offsetZ >= 0)
            cameraPosition.z = player.transform.position.z + offsetZ;
        //else if (cameraPosition.z > 5f)
        //    cameraPosition.z = 5f;
        //else if (cameraPosition.z < 0f)
        //    cameraPosition.z = 0f;

        transform.position = cameraPosition;
    }
}
