using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMaker : MonoBehaviour
{
    public GameObject[] item;
    public GameObject allStars; // �ϳ��� ��ü�� �������� ������ ����

    public Sprite[] starSprite; // ���� ��������Ʈ��

    float item_delay; // ���� �ֱ�.

    float item_timer; // ������ ���� �ð�.

    float itemHeight; // ���� ����

    float itemRandom;
    int itemType; // ������ ����

    void Start()
    {
        item_delay = Random.Range(2f, 3f);
        item_timer = 0f;
    }

    void Update()
    {
        item_timer += Time.deltaTime;

        itemRandom = Random.Range(0, 11f);

        if (itemRandom < 4.5f)
            itemType = 0;
        else if (itemRandom < 8f)
            itemType = 1;
        else if (itemRandom < 10f)
            itemType = 2;
        else if (itemRandom <= 11f)
            itemType = 3;

        //itemType = Random.Range(0, 3);
        itemHeight = Random.Range(-0.9f, 0.9f);

        if (item_timer >= item_delay)
        {
            item_timer -= item_delay;

            Instantiate(item[itemType], new Vector3(transform.position.x, transform.position.y + itemHeight, transform.position.z), Quaternion.identity);
            //GameObject target = Instantiate(allStars, new Vector3(transform.position.x, transform.position.y + itemHeight, transform.position.z), Quaternion.identity);
            //// �ν��Ͻÿ���Ʈ �� ��ü ����
            //target.GetComponent<Stars>().increase_Score = 50 + 50 * itemType;
            //target.GetComponent<SpriteRenderer>().sprite = starSprite[itemType];
            //target.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("starGold");
            //target.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Letters/letterA");

            item_delay = Random.Range(2f, 3f);
        }
    }
}
