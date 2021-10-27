using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreatePrefab : EditorWindow
{
    [MenuItem("Prefab/Create Prefab(s)"), MenuItem("GameObject/Create Other/Create Prefab(s)")]
    static void CreateNewPrefab()
    {
        GameObject[] obj = Selection.gameObjects;

        foreach (var go in obj)
        {
            string localPath = $"Assets/03. Prefabs/{go.name}.prefab";

            if (AssetDatabase.LoadAssetAtPath(localPath, typeof(GameObject)))
            {
                if (EditorUtility.DisplayDialog("Are you sure?", "�������� �̹� �����մϴ�. ����ðڽ��ϱ�?", "��", "�ƴϿ�"))
                {
                    CreateNew(go, localPath);
                }
            }
            else
            {
                CreateNew(go, localPath);
            }
        }
    }

    [MenuItem("Prefab/Create Prefab(s)", true)]
    static bool ValidateCreatePrefeb()
    {
        return Selection.activeGameObject != null;
    }

    static void CreateNew(GameObject _obj, string _localPath)
    {
        PrefabUtility.SaveAsPrefabAsset(_obj, _localPath);

        AssetDatabase.LoadAssetAtPath(_localPath, typeof(GameObject));
    }
}
