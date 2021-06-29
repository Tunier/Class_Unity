using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    public Color _color = Color.green;
    public float _radius = 0.5f;

    private void OnDrawGizmos()
    {
        Gizmos.color = _color;
        // 해당 위치에 _radius 크기만큼 기즈모를 그려라
        // DrawSphere이므로 구체모양으로 그림.
        Gizmos.DrawSphere(transform.position, _radius);
    }
}
