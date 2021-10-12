using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.limitTime += 5f;

            if (GameManager.Instance.limitTime >= 30)
                GameManager.Instance.limitTime = 30f;

            gameObject.SetActive(false);
        }
    }
}
