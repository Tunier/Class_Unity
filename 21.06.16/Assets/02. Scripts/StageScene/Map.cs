using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public bool isHere = false;

    public List<GameObject> containObj = new List<GameObject>();

    GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("PLAYER");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("PLAYER"))
        {
            if (!containObj.Contains(other.gameObject))
            {
                containObj.Add(other.gameObject);
            }
        }
    }

    private void Update()
    {
        DeleteNullObj();

        if (containObj.Contains(player))
            isHere = true;
        else
            isHere = false;
    }

    void DeleteNullObj()
    {
        for (int i = 0; i < containObj.Count; ++i)
        {
            if (containObj[i] == null)
                containObj.RemoveAt(i);
        }
    }
}
