using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class CustomMenu
{
    [MenuItem("CONTEXT/PlayerInfo/���ظ޴�����")]
    public static void CustomMenuUI()
    {
        Debug.Log("���ؽ�Ʈ �޴� ����");
    }
}

[CustomEditor(typeof(PlayerInfo))]
public class PlayerInfoEditor : Editor
{
    PlayerInfo playerInfo;

    SerializedProperty curHp;
    SerializedProperty curMp;
    SerializedProperty state;

    private void OnEnable()
    {
        playerInfo = target as PlayerInfo;
        curHp = serializedObject.FindProperty("curHp");
        curMp = serializedObject.FindProperty("curMp");
        state = serializedObject.FindProperty("State");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        //EditorGUILayout.Space();

        GUI.enabled = false;
        EditorGUILayout.ObjectField("��ũ��Ʈ", playerInfo, typeof(Object), false);
        GUI.enabled = true;

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("�÷��̾� ����", EditorStyles.boldLabel);
        GUI.enabled = false;
        EditorGUILayout.IntField("�÷��̾� ����", playerInfo.stats.Level);
        EditorGUILayout.FloatField("�ִ� Hp", playerInfo.finalMaxHp);
        EditorGUILayout.PropertyField(curHp, new GUIContent("���� Hp"));
        EditorGUILayout.FloatField("�ʴ� Hp ����", playerInfo.finalHpRegen);
        EditorGUILayout.FloatField("�ִ� Mp", playerInfo.finalMaxMp);
        EditorGUILayout.PropertyField(curMp, new GUIContent("���� Mp"));
        EditorGUILayout.FloatField("�ʴ� Mp ����", playerInfo.finalMpRegen);
        EditorGUILayout.FloatField("�ִ� ����ġ", playerInfo.stats.MaxExp);
        EditorGUILayout.FloatField("���� ����ġ", playerInfo.stats.CurExp);
        //playerInfo.state = (STATE)EditorGUILayout.EnumPopup("�÷��̾� ����", playerInfo.state);
        EditorGUILayout.LabelField("�÷��̾� ����", $"{playerInfo.state}");
        GUI.enabled = true;

        EditorGUILayout.Space();

        playerInfo.debugMode = GUILayout.Toggle(playerInfo.debugMode, "����� ���");
        if (playerInfo.debugMode)
        {
            EditorGUILayout.Space(6);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Hp ����"))
            {
                curHp.floatValue = playerInfo.finalMaxHp;
            }
            if (GUILayout.Button("Mp ����"))
            {
                curMp.floatValue = playerInfo.finalMaxMp;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(4);

            if (playerInfo.infinityMana = GUILayout.Toggle(playerInfo.infinityMana, "��������"))
            {
                playerInfo.curMp = playerInfo.finalMaxMp;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
