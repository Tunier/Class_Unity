using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    float a;

    void Update()
    {
        a = Mathf.Sin(Time.time*5) * 0.6f;
        transform.Rotate(new Vector3(0, 180) * Time.deltaTime, Space.World);
        transform.Translate(new Vector3(0, a, 0) * Time.deltaTime, Space.World);
    }    
}
