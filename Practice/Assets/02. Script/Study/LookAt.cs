using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LookAt : MonoBehaviour
{
    [ContextMenuItem("������ ����", "ResetPos")]
    public Vector3 lookPos;

    public void Update()
    {
        transform.LookAt(lookPos);
    }

    public void ResetPos()
    {
        lookPos = transform.position;
    }
}
