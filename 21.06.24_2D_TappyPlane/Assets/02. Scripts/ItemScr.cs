using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScr : MonoBehaviour
{
    GameManager gm;
    PlayerScr player;

    float speed;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Plane").GetComponent<PlayerScr>();
        speed = 5f;
    }

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x <= -8)
            Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "Plane")
        {
            if (gameObject.name == "starBronze(Clone)")
            {
                gm.score += 10f;
            }
            else if (gameObject.name == "starSilver(Clone)")
            {
                gm.score += 20f;
            }
            else if (gameObject.name == "starGold(Clone)")
            {
                gm.score += 30f;
            }
            else if (gameObject.name == "Heal_Item(Clone)")
            {
                player.hp += 1;
                player.Healing();
            }
            Destroy(gameObject);
        }
    }
}
