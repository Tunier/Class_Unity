using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    [HideInInspector]
    public GameObject player;

    public string npcName;
    [Tooltip("����Ʈ�� ���� Npc�� ��� ��ĭ���� ����μ���.")]
    public string questUIDCODE;

    void Awake()
    {
        player = GameObject.Find("Player");
    }
}
