using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraCtrl : MonoBehaviour
{
    public GameObject player;

    public float offsetY = 45f;
    public float offsetZ = -45f;

    Vector3 cameraPos;

    void LateUpdate()
    {
        cameraPos.x = player.transform.position.x;
        cameraPos.y = player.transform.position.y + offsetY;
        cameraPos.z = player.transform.position.z + offsetZ;

        transform.position = cameraPos;
    }
}
