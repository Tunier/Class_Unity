using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    public GameObject expEffect; // ���� ����Ʈ ������ ����.
    int hitCount = 0; // �Ѿ� ���� Ƚ��.
    Rigidbody rb;

    public Mesh[] meshes; // ����� ����ϴ� �޽�.
    MeshFilter meshFilter; // �޽��� �������� �޽�����.

    public Texture[] textures; // �����⸦ ����ϴ� �ؽ�ó.
    MeshRenderer _renderer; // �ؽ�ó�� �������� �޽� ������.

    public float expRadius = 10f; // ���� �ݰ�.

    public GameObject barrel;

    AudioSource _audio;
    public AudioClip expSfx;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        meshFilter = GetComponent<MeshFilter>();
        _renderer = GetComponent<MeshRenderer>();

        int idx = Random.Range(0, textures.Length);
        _renderer.material.mainTexture = textures[idx];

        _audio = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BULLET"))
        {
            hitCount++;
            if (hitCount == 3)
            {
                // ���� ȿ�� �Լ� ȣ��
                ExpBarrel();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IndirectDamage(Vector3 pos)
    {
        // OverlapSphere �޼ҵ�� ������ ���� ���ؼ� �����ȿ� �ִ� ��� ������Ʈ�� ��� �����ؼ� �������.
        Collider[] colls = Physics.OverlapSphere(pos, // ���߿���.
                                                 expRadius, // ���߹ݰ�.
                                                 1 << 8); // ������ �� ���̾�.
        //����� ������Ʈ�� ���������� �ϳ��� ���õǵ��� ��, 1�� �����ϴ� for ���� ������. (�̹� ���� coll���� colls����)
        foreach (var coll in colls)
        {
            var _rb = coll.GetComponent<Rigidbody>();
            _rb.mass = 1;
            // �������� ���߷��� �ƴ϶� ����, �Ʒ��� ���߷��� �ֱ����ؼ� �����.
            // AddExplosionForce(Ⱦ(����) ���߷�, ���� ����, ���� �ݰ�, ��(����))
            _rb.AddExplosionForce(600f, pos, expRadius, 500f);
        }
    }

    void ExpBarrel()
    {
        // ���������� �Ǵ� ���� effect��� ��ü(����) �̸��� �ο�����.
        // ���� effect��� ��ü���� ���ؼ� �����.
        GameObject effect = Instantiate(expEffect, transform.position, Quaternion.identity);
        // ���� �����ð��� �ο�, 2���� ���� ����Ʈ ����.
        Destroy(effect, 2f);
        //rb.mass = 1f;
        //rb.AddForce(Vector3.up * 500f);
        IndirectDamage(transform.position);

        // ��ϵ� �޽� �߿��� �ϳ��� �����ϱ� ���Ͽ� ������ ���ڸ� ����.
        int idx = Random.Range(0, meshes.Length);
        // ���� �ε����� �ش��ϴ� �޽��� �����ؼ� �޽����Ϳ� ����.
        meshFilter.sharedMesh = meshes[idx];

        _audio.PlayOneShot(expSfx, 1f);
    }
}
