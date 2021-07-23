using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Transform player;

    public float offsetY;
    public float offsetZ;

    Vector3 cameraPosition;
    public Vector3 offset;

    void Start()
    {
        player = GameObject.FindWithTag("PLAYER").GetComponent<Transform>();

        offsetY = 10f;
        offsetZ = -3.5f;

        offset = new Vector3(0, offsetY, offsetZ);
    }

    void LateUpdate()
    {
        cameraPosition.x = player.transform.position.x;
        cameraPosition.y = player.transform.position.y + offsetY;

        if (player.transform.position.z + offsetZ <= 7f && player.transform.position.z + offsetZ >= -3)
            cameraPosition.z = player.transform.position.z + offsetZ;

        transform.position = cameraPosition;
    }
}
