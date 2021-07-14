using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    public GameObject sparkEffect; // 스파크 프리팹.

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌이 발생한 것들 중에서 BULLET 테그만 검출.
        if (collision.collider.tag == "BULLET")
        {
            // 스파크 이펙트 함수 호출.
            ShowEffect(collision);
            // 충돌이 발생한 오브젝트 삭제
            // Destroy(collision.gameObject); // 바로 삭제.
            // Destroy(collision.gameObject, 5f); // 딜레이후 삭제.
            collision.gameObject.SetActive(false); // 충돌하면 오브젝트를 비활성화함.
        }

    }
    void ShowEffect(Collision coll)
    {
        // 충돌 지점의 정보를 가지고옴.
        // 충돌 시 발생한 최초의 위치 정보.
        ContactPoint contact = coll.contacts[0];
        // FromToRotation(회전시키고자 하는 벡터, 타겟 벡터)
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);
        // 충돌이 난 후 이펙트의 효과 방향을 법선벡터(총알이 날아온 방향)으로 생성.
        // 법선벡터(총알이 날아온 방향 -Z)방향 으로 돌려서 생성.
        // 총알이 발사된 위치로 이동(드럼통에서 조금 띄워서 총알자국 생성)
        Vector3 point = contact.point + (-contact.normal * 0.01f);
        GameObject spark = Instantiate(sparkEffect, point, rot);
        // 동적생성된 이펙트의 부모로 드럼통을 설정.
        spark.transform.SetParent(this.transform);
    }
}
