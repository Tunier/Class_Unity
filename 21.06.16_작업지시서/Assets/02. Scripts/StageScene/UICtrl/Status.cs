using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [SerializeField]
    GameObject slotsParent;

    [SerializeField]
    public Slot[] slots;

    void Start()
    {
        slots = slotsParent.GetComponentsInChildren<Slot>();
    }

    void Update()
    {

    }
}
