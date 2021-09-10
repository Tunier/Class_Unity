using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooling : MonoBehaviour
{
    // 오브젝트 풀링(Object Pooling)
    // 프로그램에서는 기본적으로 오브젝트의 생성과 삭제를
    // 하는데 생각보다 많은 처리가 필요하다.
    // 때문에 만약 특정 오브젝트가 생성과 삭제를
    // 빠른 주기로 반복한다면 cpu에 가해지는 부하가 많아져서
    // 전체적인 게임 퍼포먼스가 떨어지게 된다.
    // 그래서 오브젝트를 매번 삭제하고 새로 만드는 것이 아니라
    // 삭제 대신 대상을 비활성화 시키고
    // 생성 대신, 비활성화됐던 대상을 다시 가져와서
    // 활성화시켜 재활용할수 있도록 만드는 방식을
    // 오브젝트 풀링이라고 부른다.

    private static Pooling instance = null;

    public GameObject bulletPref;

    Queue<GameObject> bullets = new Queue<GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static Pooling Instance
    {
        get
        {
            if (instance == null)
                return null;
            else
                return instance;
        }
    }

    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            var b = Instantiate(bulletPref);

            bullets.Enqueue(b);
            b.SetActive(false);
            b.transform.SetParent(transform);
        }
    }

    public GameObject GetBullet()
    {
        if (bullets.Count == 0)
        {
            var bullet = Instantiate(bulletPref);
            bullet.transform.SetParent(null);

            return bullet;
        }
        else
        {
            var bullet = bullets.Dequeue();
            bullet.SetActive(true);
            bullet.transform.SetParent(null);

            return bullet;
        }
    }

    public void ReturnBullet(GameObject _bullet)
    {
        _bullet.SetActive(false);
        bullets.Enqueue(_bullet);
        _bullet.transform.SetParent(transform);
    }

    void Update()
    {

    }
}
