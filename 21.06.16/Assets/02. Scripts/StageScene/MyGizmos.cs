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
        // �ش� ��ġ�� _radius ũ�⸸ŭ ����� �׷���
        // DrawSphere�̹Ƿ� ��ü������� �׸�.
        Gizmos.DrawSphere(transform.position, _radius);
    }
}
