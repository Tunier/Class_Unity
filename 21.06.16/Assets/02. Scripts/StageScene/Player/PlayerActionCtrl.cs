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
    List<GameObject> items;

    [SerializeField]
    Inventory inven;
    [SerializeField]
    QuickSlot qSlot;

    GameObject neareastItem = null;

    private void Awake()
    {
        items = new List<GameObject>();

        range = 5f;
    }

    void Start()
    {

    }

    void Update()
    {
        DeleteNullSlot();

        FindItem();

        if (items != null)
        {
            items = items.OrderBy(obj => Vector3.Distance(transform.position, obj.transform.position)).ToList();
            neareastItem = items.FirstOrDefault();
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
                if (!items.Contains(obj))
                {
                    items.Add(obj);
                }
            }
            else
            {
                if (items.Contains(obj))
                {
                    items.Remove(obj);
                }
            }
        }
    }

    private void GetItem(int count = 1)
    {
        if (pickupActivated)
        {
            if (!inven.isFull)
            {
                inven.GetItem(neareastItem.GetComponent<ItemPickUp>().item);
                items.Remove(neareastItem);
                Destroy(neareastItem);
            }
            else if (!qSlot.isFull)
            {
                qSlot.GetItem(neareastItem.GetComponent<ItemPickUp>().item);
                items.Remove(neareastItem);
                Destroy(neareastItem);
            }
            else
            {
                if (!UIManager.instance.actionText.gameObject.activeSelf)
                    StartCoroutine(UIManager.instance.PrintActionText("¿Œ∫•≈‰∏Æ∞° ≤À√°Ω¿¥œ¥Ÿ."));
            }
        }
    }

    

    void DeleteNullSlot()
    {
        for (int i = 0; i < items.Count; ++i)
        {
            if (items[i] == null)
                items.RemoveAt(i);
        }
    }
}
