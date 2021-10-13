using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    [HideInInspector]
    public GameObject player;

    public string npcName;
    [Tooltip("퀘스트가 없는 Npc의 경우 빈칸으로 비워두세요.")]
    public string questUIDCODE;

    void Awake()
    {
        player = GameObject.Find("Player");
    }
}
