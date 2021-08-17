using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Test : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    Inventory inven;

    PlayerTest player;
    SkillDatabase skillDB;

    public string _s;

    void Start()
    {
        player = FindObjectOfType<PlayerTest>();
        skillDB = FindObjectOfType<SkillDatabase>();
    }

    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GetItem(_s);
        }
    }

    public void GetItem(string _s)
    {
        var item = ItemDatabase.instance.newItem(_s);

        inven.GetItem(item);
    }
}
