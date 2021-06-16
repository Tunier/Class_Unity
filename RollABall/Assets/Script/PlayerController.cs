using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Text countText;
    public Text winText;

    private Rigidbody rb;
    private int countcoin;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        countcoin = 0;
        SetCountText();
        winText.text = "";        
    }
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);

        rb.AddForce(movement * speed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            other.gameObject.SetActive(false);
            countcoin++;
            SetCountText();
        }
    }
    void SetCountText()
    {
        countText.text = "Count : " + countcoin.ToString();
        if (countcoin >= 5)
        {
            winText.text = "You Win!";
        }
    }
}
