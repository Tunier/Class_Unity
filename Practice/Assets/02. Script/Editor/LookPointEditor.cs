using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LookAt))]
[CanEditMultipleObjects]
public class LookPointEditor : Editor
{
    SerializedProperty lookPos;
    LookAt lookAt = null;

    private void OnEnable()
    {
        lookAt = (target as LookAt);
        lookPos = serializedObject.FindProperty("lookPos");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //GUI.enabled = false;
        //EditorGUILayout.PropertyField(lookPos);
        //GUI.enabled = true;
        EditorGUILayout.PropertyField(lookPos);
        //EditorGUILayout.HelpBox("hi", MessageType.Info);
        //EditorGUILayout.LabelField("Ÿ��������", lookAt.lookPos.ToString());

        if (lookAt.lookPos.y > (target as LookAt).transform.position.y)
            EditorGUILayout.LabelField("Ÿ���������� ������.");
        else if (lookAt.lookPos.y < (target as LookAt).transform.position.y)
            EditorGUILayout.LabelField("Ÿ���������� �Ʒ�����.");
        else
            EditorGUILayout.LabelField("Ÿ���������� �� �����ǰ� ������.");

        serializedObject.ApplyModifiedProperties();
    }

    public void OnSceneGUI()
    {
        EditorGUI.BeginChangeCheck();
        Vector3 pos = Handles.PositionHandle(lookAt.lookPos, Quaternion.identity);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Move Point");
            lookAt.lookPos = pos;
            lookAt.Update();
        }
    }
}
