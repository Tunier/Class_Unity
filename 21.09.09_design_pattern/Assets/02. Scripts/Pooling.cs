using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooling : MonoBehaviour
{
    // ������Ʈ Ǯ��(Object Pooling)
    // ���α׷������� �⺻������ ������Ʈ�� ������ ������
    // �ϴµ� �������� ���� ó���� �ʿ��ϴ�.
    // ������ ���� Ư�� ������Ʈ�� ������ ������
    // ���� �ֱ�� �ݺ��Ѵٸ� cpu�� �������� ���ϰ� ��������
    // ��ü���� ���� �����ս��� �������� �ȴ�.
    // �׷��� ������Ʈ�� �Ź� �����ϰ� ���� ����� ���� �ƴ϶�
    // ���� ��� ����� ��Ȱ��ȭ ��Ű��
    // ���� ���, ��Ȱ��ȭ�ƴ� ����� �ٽ� �����ͼ�
    // Ȱ��ȭ���� ��Ȱ���Ҽ� �ֵ��� ����� �����
    // ������Ʈ Ǯ���̶�� �θ���.

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
