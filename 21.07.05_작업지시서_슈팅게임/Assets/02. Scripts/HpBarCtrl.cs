using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBarCtrl : MonoBehaviour
{
    [SerializeField]
    PlayerCtrl player;

    Vector3 offset;

    void Start()
    {
        offset = new Vector3(0, 0.5f, 0);
    }

    void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(player.transform.position + offset);
    }
}
