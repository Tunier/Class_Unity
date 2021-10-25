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
        //EditorGUILayout.LabelField("타겟포지션", lookAt.lookPos.ToString());

        if (lookAt.lookPos.y > (target as LookAt).transform.position.y)
            EditorGUILayout.LabelField("타겟포지션이 위쪽임.");
        else if (lookAt.lookPos.y < (target as LookAt).transform.position.y)
            EditorGUILayout.LabelField("타겟포지션이 아래쪽임.");
        else
            EditorGUILayout.LabelField("타겟포지션이 내 포지션과 동일함.");

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
