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

    [SerializeField]
    GameObject shopPanel;

    GameObject merchant;
    float recognitionRange; // ����â ų���ִ� �Ÿ�

    private void Awake()
    {
        items = new List<GameObject>();

        range = 5f;
        recognitionRange = 2f;
    }

    void Start()
    {
        merchant = GameObject.FindGameObjectWithTag("MERCHANT");
    }

    void Update()
    {
        DeleteNullSlot();

        FindItem();

        if (items != null)
        {
            // �Ÿ������� ������������ ����Ʈ �����ؼ� ��������.
            items = items.OrderBy(obj => Vector3.Distance(transform.position, obj.transform.position)).ToList();
            // ����Ʈ�� ù��°(���� ����� ������)�� neareastItem
            neareastItem = items.FirstOrDefault();
        }

        pickupActivated = neareastItem == null ? false : true;

        TryAction();
    }

    /// <summary>
    /// �����̽��ٸ� ������ FindItem�Լ��� GetItem�Լ��� ȣ����.
    /// </summary>
    private void TryAction()
    {
        if (Vector3.Distance(transform.position, merchant.transform.position) <= recognitionRange)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                shopPanel.SetActive(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            FindItem();
            GetItem();
        }
    }

    /// <summary>
    /// firstList �� ��� �������� �޾Ƽ� �����Ÿ� �ȿ� �ִ� �����۸� items����Ʈ�� �Ѱ��ְ�<br/>
    /// �Ÿ� �Ѿ�� �������� ����Ʈ���� ������.
    /// </summary>
    private void FindItem()
    {
        List<GameObject> firstList = new List<GameObject>();

        firstList.AddRange(GameObject.FindGameObjectsWithTag("ITEM"));

        foreach (GameObject obj in firstList)
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


    /// <summary>
    /// �κ��丮�� ��á���� Ȯ���ϰ� �ƴϸ� �־��ְ�, ������ Ȯ���� �־��ְ�<br/>
    /// �Ѵ� ������ "�κ��丮�� ��á���ϴ�" �ؽ�Ʈ ���
    /// </summary>
    /// <param name="count"></param>
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
                    StartCoroutine(UIManager.instance.PrintActionText("�κ��丮�� ��á���ϴ�."));
            }
        }
    }

    /// <summary>
    /// �������� Destroy�Ǹ�(Null�� �Ǹ�) �ش� ������ ����Ʈ���� ����
    /// </summary>
    void DeleteNullSlot()
    {
        for (int i = 0; i < items.Count; ++i)
        {
            if (items[i] == null)
                items.RemoveAt(i);
        }
    }
}
