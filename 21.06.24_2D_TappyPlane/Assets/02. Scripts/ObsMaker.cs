using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsMaker : MonoBehaviour
{
    public GameObject down_obs; // 아래쪽 장에물 프리펩.
    public GameObject down_obs2;

    [SerializeField]
    float obs_delay; // 생성 주기.

    float obs_timer; // 생성후 지난 시간.

    float obsHeight;

    int obsType;

    void Start()
    {
        obs_timer = 0f;
    }

    void Update()
    {
        obs_timer += Time.deltaTime;

        obsType = Random.Range(0, 2);
        obsHeight = Random.Range(-0.6f, 0.2f);

        if (obs_timer >= obs_delay)
        {
            obs_timer -= obs_delay;

            if (obsType == 0)
                Instantiate(down_obs, new Vector3(transform.position.x, transform.position.y + obsHeight, transform.position.z), Quaternion.identity);
            else if (obsType == 1)
                Instantiate(down_obs2, new Vector3(transform.position.x, transform.position.y + obsHeight, transform.position.z), Quaternion.identity);

            obs_delay = Random.Range(1f, 2f);
        }
    }
}
