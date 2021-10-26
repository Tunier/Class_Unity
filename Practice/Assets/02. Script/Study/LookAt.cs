using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LookAt : MonoBehaviour
{
    [ContextMenuItem("포지션 리셋", "ResetPos")]
    public Vector3 lookPos;

    public GameObject go_1;
    public GameObject go_2;

    public void Update()
    {
        transform.LookAt(lookPos);
    }

    public void ResetPos()
    {
        lookPos = transform.position;
    }
}
