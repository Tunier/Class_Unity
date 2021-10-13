using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingEffect : MonoBehaviour
{
    GameObject healEffectBox;

    private void Awake()
    {
        healEffectBox = GameObject.Find("EffectBox");
        transform.localPosition = new Vector3(healEffectBox.transform.position.x, healEffectBox.transform.position.y + 2, healEffectBox.transform.position.z);
    }

    void Update()
    {
        transform.localPosition = new Vector3(healEffectBox.transform.position.x, healEffectBox.transform.position.y + 2, healEffectBox.transform.position.z);
    }
}
