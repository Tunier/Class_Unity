using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Special : MonoBehaviour
{
    int score = 50;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player>();
            player.score += score;

            player.UseSpecial_Item();

            if (Stage_Manager.Instance.items.Contains(gameObject))
                Stage_Manager.Instance.items.Remove(gameObject);

            Destroy(gameObject);
        }
    }
}
