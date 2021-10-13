using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapButton : MonoBehaviour
{
    public Camera minimapCamera;

    public void OnClickPlus()
    {
        if (minimapCamera.orthographicSize > 40)
        {
            minimapCamera.orthographicSize -= 15;
        }
    }
  
    public void OnClickMinus()
    {
        if (minimapCamera.orthographicSize < 90)
        {
            minimapCamera.orthographicSize += 15;
        }
    }
}
