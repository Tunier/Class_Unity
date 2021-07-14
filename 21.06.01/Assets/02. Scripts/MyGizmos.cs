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

    // Gizmos ���� ���� �ִ� Ȱ���ϰ��� �ϴ� ���ҽ��� ���ϸ�.
    const string wayPointFile = "Enemy";
    public Type type = Type.NORMAL;

    public Color _color = Color.yellow;
    public float _radius = 0.5f;

    private void OnDrawGizmos()
    {
        if (type == Type.NORMAL)
        {
            Gizmos.color = _color;
            // �ش� ��ġ�� _radius ũ�⸸ŭ ����� �׷���
            // DrawSphere�̹Ƿ� ��ü������� �׸�.

            Gizmos.DrawSphere(transform.position, _radius);
        }
        else
        {
            Gizmos.color = _color;

            // allowScaling = ī�޶� ����/�ƿ��� ���� ������ũ�� ���氡��.
            Gizmos.DrawIcon(transform.position + Vector3.up, wayPointFile, true);

            Gizmos.DrawWireSphere(transform.position, _radius);
        }

    }
}
