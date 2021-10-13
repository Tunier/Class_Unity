using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingPointCtrl : MonoBehaviour
{
    public Transform goblinKing;
    void LateUpdate()
    {
        if (goblinKing != null)
        {
            transform.position = goblinKing.position;
        }
        
    }
}
