using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Editor Ŭ������ �ν����ͳ� ������ ȭ�� ����
// �����Ӱ� �����ϰų� Ȯ���� �� �ַ� �ϱ� ���� Ŭ����
// ���� ���� ����ڰ� Ŀ������ ������ ���ۿ� ����

// EnemyFOV ��ũ��Ʈ�� �����ϴ� Ŀ���� �����ʹ� ��� ���
[CustomEditor(typeof(EnemyFOV))]
public class FOVEditor : Editor
{
    private void OnSceneGUI()
    {
        // �����Ͱ� ������ ����� ����
        // EnemyFov Ŭ���� ����
        EnemyFOV fov = (EnemyFOV)target;

        // ���� ������ �������� ��ǥ�� ���(�þ߰��� 1/2)
        Vector3 fromAnglePos = fov.CirclePoint(-fov.viewAngle * 0.5f);

        // ������ ������ ������� ����
        Handles.color = Color.white;

        // �ܰ����� �ִ� ���� �׸�
        Handles.DrawWireDisc(fov.transform.position, Vector3.up, fov.viewRange); // ������ǥ, ��ֺ���, ���� ������

        // ��ä��(�þ߰��� ǥ��)
        Handles.color = new Color(1, 0, 0, 0.2f);
        // ���� ��ǥ, ��ֺ���, ��ä���� ���� ����, �׸��� ����, ��ä���� ������
        Handles.DrawSolidArc(fov.transform.position, Vector3.up, fromAnglePos, fov.viewAngle, fov.viewRange);
        // �þ߰� �󺧸�
        Handles.Label(fov.transform.position + fov.transform.forward * 2f, fov.viewAngle.ToString());
    }
}
