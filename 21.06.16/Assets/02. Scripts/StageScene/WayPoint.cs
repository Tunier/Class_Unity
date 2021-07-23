using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    GameObject player;
    [SerializeField]
    Map[] maps;

    Vector3 map0 = new Vector3(-8, 0.05f, 2.5f);
    Vector3 map1 = new Vector3(42, 0.05f, 2.5f);

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("PLAYER");
        maps = FindObjectsOfType<Map>();
    }

    void Update()
    {
        
    }

    public void OnClickMap0()
    {
        player.transform.position = map0;

        maps[1].containObj.Remove(player);
    }

    public void OnClickMap1()
    {
        player.transform.position = map1;
        
        maps[0].containObj.Remove(player);
    }

    public void OnclickExit()
    {
        GameManager.instance.wayPointUI.SetActive(false);
    }
}
