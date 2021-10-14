using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class CustomMenu
{
    [MenuItem("CONTEXT/PlayerInfo/컨텍메뉴연습")]
    public static void CustomMenuUI()
    {
        Debug.Log("컨텍스트 메뉴 연습");
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
        EditorGUILayout.ObjectField("스크립트", playerInfo, typeof(Object), false);
        GUI.enabled = true;

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("플레이어 스텟", EditorStyles.boldLabel);
        GUI.enabled = false;
        EditorGUILayout.IntField("플레이어 레벨", playerInfo.stats.Level);
        EditorGUILayout.FloatField("최대 Hp", playerInfo.finalMaxHp);
        EditorGUILayout.PropertyField(curHp, new GUIContent("현재 Hp"));
        EditorGUILayout.FloatField("초당 Hp 리젠", playerInfo.finalHpRegen);
        EditorGUILayout.FloatField("최대 Mp", playerInfo.finalMaxMp);
        EditorGUILayout.PropertyField(curMp, new GUIContent("현재 Mp"));
        EditorGUILayout.FloatField("초당 Mp 리젠", playerInfo.finalMpRegen);
        EditorGUILayout.FloatField("최대 경험치", playerInfo.stats.MaxExp);
        EditorGUILayout.FloatField("현재 경험치", playerInfo.stats.CurExp);
        //playerInfo.state = (STATE)EditorGUILayout.EnumPopup("플레이어 상태", playerInfo.state);
        EditorGUILayout.LabelField("플레이어 상태", $"{playerInfo.state}");
        GUI.enabled = true;

        EditorGUILayout.Space();

        playerInfo.debugMode = GUILayout.Toggle(playerInfo.debugMode, "디버그 모드");
        if (playerInfo.debugMode)
        {
            EditorGUILayout.Space(6);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Hp 리셋"))
            {
                curHp.floatValue = playerInfo.finalMaxHp;
            }
            if (GUILayout.Button("Mp 리셋"))
            {
                curMp.floatValue = playerInfo.finalMaxMp;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(4);

            if (playerInfo.infinityMana = GUILayout.Toggle(playerInfo.infinityMana, "마나무한"))
            {
                playerInfo.curMp = playerInfo.finalMaxMp;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
