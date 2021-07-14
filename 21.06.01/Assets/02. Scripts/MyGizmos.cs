using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    public enum Type
    {
        NORMAL,
        SPAWNPOINT,
    }

    // Gizmos 폴더 내에 있는 활용하고자 하는 리소스의 파일명.
    const string wayPointFile = "Enemy";
    public Type type = Type.NORMAL;

    public Color _color = Color.yellow;
    public float _radius = 0.5f;

    private void OnDrawGizmos()
    {
        if (type == Type.NORMAL)
        {
            Gizmos.color = _color;
            // 해당 위치에 _radius 크기만큼 기즈모를 그려라
            // DrawSphere이므로 구체모양으로 그림.

            Gizmos.DrawSphere(transform.position, _radius);
        }
        else
        {
            Gizmos.color = _color;

            // allowScaling = 카메라 줌인/아웃에 따라 아이콘크기 변경가능.
            Gizmos.DrawIcon(transform.position + Vector3.up, wayPointFile, true);

            Gizmos.DrawWireSphere(transform.position, _radius);
        }

    }
}
