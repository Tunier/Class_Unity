using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LookAt))]
[CanEditMultipleObjects]
public class LookPointEditor : Editor
{
    SerializedProperty lookPos;
    SerializedProperty go_1;
    SerializedProperty go_2;

    Vector3 go_1_tr;
    Vector3 go_2_tr;

    LookAt lookAt = null;

    float angle;

    private void OnEnable()
    {
        lookAt = (target as LookAt);
        lookPos = serializedObject.FindProperty("lookPos");
        go_1 = serializedObject.FindProperty("go_1");
        go_2 = serializedObject.FindProperty("go_2");
    }

    public override void OnInspectorGUI()
    {
        //serializedObject.Update();

        EditorGUILayout.PropertyField(go_1);
        EditorGUILayout.PropertyField(go_2);
        EditorGUILayout.Space();

        go_1_tr = lookAt.go_1 != null ? lookAt.go_1.transform.position : Vector3.zero;
        go_2_tr = lookAt.go_2 != null ? lookAt.go_2.transform.position : Vector3.zero;

        Vector3 rd_angle = go_2_tr - go_1_tr;
        angle = Mathf.Atan2(rd_angle.z, rd_angle.x) * Mathf.Rad2Deg;
        EditorGUILayout.LabelField("두 오브젝트간의 각도", $"{angle}도");

        //if (lookAt.go_2 != null)
        //    go_2_tr = lookAt.go_2.transform.position;
        //else
        //    go_2_tr = Vector3.zero;



        //GUI.enabled = false;
        //EditorGUILayout.PropertyField(lookPos);
        //GUI.enabled = true;
        EditorGUILayout.PropertyField(lookPos);
        EditorGUILayout.Space();

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
