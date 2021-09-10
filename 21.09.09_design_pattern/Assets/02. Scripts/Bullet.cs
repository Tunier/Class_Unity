using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float delayTime = 3f;

    void Start()
    {
        StartCoroutine(IDie(delayTime));
    }

    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * 8;
    }

    void Die()
    {
        Pooling.Instance.ReturnBullet(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bullet"))
            Die();
    }

    IEnumerator IDie(float _delay)
    {
        yield return new WaitForSeconds(_delay);

        Die();
    }
}
