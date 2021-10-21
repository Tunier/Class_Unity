using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Score : MonoBehaviour
{
    int score = 10;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player>();
            player.score += score;

            if (Stage_Manager.Instance.items.Contains(gameObject))
                Stage_Manager.Instance.items.Remove(gameObject);

            Destroy(gameObject);
        }
    }
}
