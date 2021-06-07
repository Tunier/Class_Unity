using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // 충돌이 발생한 것들 중에서 BULLET 테그만 검출.
        if (collision.collider.tag == "BULLET")
        {
            // 충돌이 발생한 오브젝트 삭제
            Destroy(collision.gameObject); // 바로 삭제.
            // Destroy(collision.gameObject, 5f); // 딜레이후 삭제.
            // collision.gameObject.SetActive(false); // 충돌하면 오브젝트를 비활성화함.
        }
    }
}
