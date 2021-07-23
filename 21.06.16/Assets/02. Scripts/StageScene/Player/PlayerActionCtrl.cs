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
    [SerializeField]
    GameObject wayPointPanel;

    List<GameObject> merchants = new List<GameObject>();
    List<GameObject> wayPoints = new List<GameObject>();

    float recognitionRange; // 상점창 킬수있는 거리

    private void Awake()
    {
        items = new List<GameObject>();

        range = 5f;
        recognitionRange = 2.5f;
    }

    void Start()
    {
        merchants.AddRange(GameObject.FindGameObjectsWithTag("MERCHANT"));
        wayPoints.AddRange(GameObject.FindGameObjectsWithTag("WAYPOINT"));
    }

    void Update()
    {
        DeleteNullSlot();

        FindItem();

        if (items != null)
        {
            // 거리에따라 오름차순으로 리스트 정렬해서 대입해줌.
            items = items.OrderBy(obj => Vector3.Distance(transform.position, obj.transform.position)).ToList();
            // 리스트의 첫번째(가장 가까운 아이템)을 neareastItem
            neareastItem = items.FirstOrDefault();
        }

        pickupActivated = neareastItem == null ? false : true;

        TryAction();

        foreach (GameObject _merchant in merchants)
        {
            if (Vector3.Distance(transform.position, _merchant.transform.position) <= recognitionRange)
            {
                UIManager.instance.hotKeyGuid.SetActive(true);
                UIManager.instance.hotKeyGuidTarget = _merchant;
                return;
            }
            else
            {
                UIManager.instance.hotKeyGuid.SetActive(false);
            }
        }

        foreach (GameObject _wayPoint in wayPoints)
        {
            if (Vector3.Distance(transform.position, _wayPoint.transform.position) <= recognitionRange)
            {
                UIManager.instance.hotKeyGuid.SetActive(true);
                UIManager.instance.hotKeyGuidTarget = _wayPoint;
                return;
            }
            else
            {
                UIManager.instance.hotKeyGuid.SetActive(false);
            }
        }

    }

    /// <summary>
    /// 스페이스바를 누르면 FindItem함수와 GetItem함수를 호출함.
    /// </summary>
    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (UIManager.instance.hotKeyGuid.activeSelf && merchants.Contains(UIManager.instance.hotKeyGuidTarget))
            {
                shopPanel.SetActive(true);
                GameManager.instance.inventoryUI.SetActive(true);
            }
            else if (UIManager.instance.hotKeyGuid.activeSelf && wayPoints.Contains(UIManager.instance.hotKeyGuidTarget))
            {
                wayPointPanel.SetActive(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            FindItem();
            GetItem();
        }
    }

    /// <summary>
    /// firstList 에 모든 아이템을 받아서 일정거리 안에 있는 아이템만 items리스트에 넘겨주고<br/>
    /// 거리 넘어가는 아이템은 리스트에서 삭제함.
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
    /// 인벤토리가 꽉찼는지 확인하고 아니면 넣어주고, 퀵슬롯 확인후 넣어주고<br/>
    /// 둘다 꽉차면 "인벤토리가 꽉찼습니다" 텍스트 출력
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
                    StartCoroutine(UIManager.instance.PrintActionText("인벤토리가 꽉찼습니다."));
            }
        }
    }

    /// <summary>
    /// 아이템이 Destroy되면(Null이 되면) 해당 아이템 리스트에서 삭제
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
