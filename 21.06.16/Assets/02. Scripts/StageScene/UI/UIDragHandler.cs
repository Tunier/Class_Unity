using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public static GameObject beingDragged;

    Vector3 beginMousePos;
    Vector3 curMousePos;    

    private void Start()
    {

    }

    private void Update()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetSiblingIndex(7);
        beginMousePos = Input.mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        curMousePos = Input.mousePosition;
        Vector3 mouseMove = curMousePos - beginMousePos;
        transform.position = transform.position + mouseMove;

        beginMousePos = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.SetSiblingIndex(7);
    }
}
