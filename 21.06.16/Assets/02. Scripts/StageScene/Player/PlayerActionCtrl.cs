using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionCtrl : MonoBehaviour
{
    [SerializeField]
    float range;

    [SerializeField]
    bool pickupActivated = false;

    [SerializeField]
    List<GameObject> item;

    [SerializeField]
    Inventory inven;

    GameObject neareastItem = null;

    Text actionText;

    void Start()
    {
        range = 5f;
        item = new List<GameObject>();
    }

    void Update()
    {
        FindItem();

        if (item != null)
        {
            item = item.OrderBy(obj => Vector3.Distance(transform.position, obj.transform.position)).ToList();
            neareastItem = item.FirstOrDefault();
        }

        pickupActivated = neareastItem == null ? false : true;

        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FindItem();
            GetItem();
        }
    }

    private void FindItem()
    {
        List<GameObject> fistList = new List<GameObject>();

        fistList.AddRange(GameObject.FindGameObjectsWithTag("ITEM"));

        foreach (GameObject obj in fistList)
        {
            if (Vector3.Distance(transform.position, obj.transform.position) <= range)
            {
                if (!item.Contains(obj))
                {
                    item.Add(obj);
                }
            }
            else
            {
                if (item.Contains(obj))
                {
                    item.Remove(obj);
                }
            }
        }
    }

    private void GetItem()
    {
        if (pickupActivated)
        {
            inven.GetItem(neareastItem.GetComponent<ItemPickUp>().item);
            item.Remove(neareastItem);
            Destroy(neareastItem);
        }
    }


}
